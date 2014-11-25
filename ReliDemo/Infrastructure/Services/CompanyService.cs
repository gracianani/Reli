using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ReliDemo.Infrastructure.Services
{
    public class CompanyService
    {
        private xz2013Entities _db = new xz2013Entities();

        private const string 有效监测站数Sql = @"select count(*)
                                        from xz2013.dbo.Stations a
                                        join xz2013.dbo.StationAccuHistory b on b.日期 = @day and b.热力站id = a.itemId
                                        WHERE
	                                        b.投入面积>300 and 
                                            b.报警 is null and
                                            a.公司ID = @companyId and
                                            b.瞬流avg  < 3000 and b.采暖GJ < a.GJ_Limit 
                                        ";

        private const string 监测站供热面积Sql = @"select sum(b.[投入面积]/10000)
                                        from xz2013.dbo.Stations a
                                        join xz2013.dbo.StationAccuHistory b on b.日期 = @day and b.热力站id = a.itemId
                                        WHERE
	                                        b.投入面积>300 and 
                                            b.报警 is null and
                                            a.公司ID = @companyId and
                                            b.瞬流avg  < 3000 and b.采暖GJ < a.GJ_Limit 
                                        ";

        private const string 回温超标45数Sql = @"select count(*)
                                        from xz2013.dbo.Stations a
                                        join xz2013.dbo.StationAccuHistory b on b.日期 = @day and b.热力站id = a.itemId
                                        WHERE
	                                        b.投入面积>300 and 
                                            b.报警 is null and
                                            a.公司ID = @companyId and
                                            b.回温avg>45  and
                                            b.瞬流avg  < 3000 and b.采暖GJ < a.GJ_Limit 
                                        ";

        private const string 实际超核算数Sql = @"select count(*)
                                        from xz2013.dbo.Stations a
                                        join xz2013.dbo.StationAccuHistory b on b.日期 = @day and b.热力站id = a.itemId
                                        WHERE
	                                        b.投入面积>300 and 
                                            b.报警 is null and
                                            a.公司ID = @companyId and
                                            b.核算GJ < b.采暖GJ and
                                            b.瞬流avg  < 3000 and b.采暖GJ < a.GJ_Limit 
                                        ";

        private const string 实际超核算面积Sql = @"select sum(b.[投入面积]/10000)
                                        from xz2013.dbo.Stations a
                                        join xz2013.dbo.StationAccuHistory b on b.日期 = @day and b.热力站id = a.itemId
                                        WHERE
	                                        b.投入面积>300 and 
                                            b.报警 is null and
                                            a.公司ID = @companyId and
                                            b.核算GJ < b.采暖GJ and
                                            b.瞬流avg  < 3000 and b.采暖GJ < a.GJ_Limit ";

        private const string 实际超计划数Sql = @"select count(*)
                                        from xz2013.dbo.Stations a
                                        join xz2013.dbo.StationAccuHistory b on b.日期 = @day and b.热力站id = a.itemId
                                        WHERE
	                                        b.投入面积>300 and 
                                            b.报警 is null and
                                            a.公司ID = @companyId and
                                            b.计划GJ < b.采暖GJ and
                                            b.瞬流avg  < 3000 and b.采暖GJ < a.GJ_Limit 
                                        ";

        private const string 实际超计划面积Sql = @"select sum(b.[投入面积]/10000)
                                        from xz2013.dbo.Stations a
                                        join xz2013.dbo.StationAccuHistory b on b.日期 = @day and b.热力站id = a.itemId
                                        WHERE
	                                        b.投入面积>300 and 
                                            b.报警 is null and
                                            a.公司ID = @companyId and
                                            b.计划GJ < b.采暖GJ and
                                            b.瞬流avg  < 3000 and b.采暖GJ < a.GJ_Limit ";

        private const string 非重点站Sql = "a.是否重点站=0 and a.收费性质  not like '%计量%' ";

        private const string 执行到位率表Sql = @"select 
	        sum(b.[投入面积]/10000) as 总, 
	        sum(case when b.核算GJ < b.采暖GJ then b.[投入面积] else 0 end / 10000) as 实际超核算, 
	        sum(case when b.计划GJ < b.采暖GJ then b.[投入面积] else 0 end / 10000) as 实际超计划,
	        b.日期
            from xz2013.dbo.Stations a
            join xz2013.dbo.StationAccuHistory b on b.热力站id = a.itemId
            WHERE
                b.投入面积>300 and 
                b.报警 is null and
                a.公司ID = @companyId and
                b.日期 > @fromDay and b.日期 <= @toDay  and
                                            b.瞬流avg  < 3000 and b.采暖GJ < a.GJ_Limit 
            group by b.日期
            order by b.日期 asc";


        public int 有效监测站数(int companyId, DateTime day, bool is非重点)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 有效监测站数Sql + (is非重点 ? " and " + 非重点站Sql : "");
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("companyId", companyId));
                    cmd.Parameters.Add(new SqlParameter("day", day.ToString("yyyy-MM-dd")));
                    cmd.CommandTimeout = 200;
                    var count = cmd.ExecuteScalar();
                    if (count != null && !string.IsNullOrEmpty(count.ToString()))
                    {
                        return int.Parse(count.ToString());
                    }
                }
            }
            
            return 0;
        }

        public decimal 监测站供热面积(int companyId, DateTime day, bool is非重点)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 监测站供热面积Sql + (is非重点 ? " and " + 非重点站Sql : "");
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("companyId", companyId));
                    cmd.Parameters.Add(new SqlParameter("day", day.ToString("yyyy-MM-dd")));
                    cmd.CommandTimeout = 200;
                    var count = cmd.ExecuteScalar();
                    if (count != null && !string.IsNullOrEmpty(count.ToString()))
                    {
                        return decimal.Round( decimal.Parse(count.ToString()), 2);
                    }
                }
            }

            return 0.00m;
        }

        public int 回温超标45数(int companyId, DateTime day, bool is非重点)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 回温超标45数Sql + (is非重点 ? " and " + 非重点站Sql : "");
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("companyId", companyId));
                    cmd.Parameters.Add(new SqlParameter("day", day.ToString("yyyy-MM-dd")));
                    cmd.CommandTimeout = 200;
                    var count = cmd.ExecuteScalar();
                    if (count != null && !string.IsNullOrEmpty(count.ToString()))
                    {
                        return int.Parse(count.ToString());
                    }
                }
            }

            return 0;
        }

        public int 实际超核算数(int companyId, DateTime day, bool is非重点)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 实际超核算数Sql + (is非重点 ? " and " + 非重点站Sql : "");
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("companyId", companyId));
                    cmd.Parameters.Add(new SqlParameter("day", day.ToString("yyyy-MM-dd")));
                    cmd.CommandTimeout = 200;
                    var count = cmd.ExecuteScalar();
                    if (count != null && !string.IsNullOrEmpty(count.ToString()))
                    {
                        return int.Parse(count.ToString());
                    }
                }
            }

            return 0;
        }

        public decimal 实际超核算面积(int companyId, DateTime day, bool is非重点)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 实际超核算面积Sql + (is非重点 ? " and " + 非重点站Sql : "");
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("companyId", companyId));
                    cmd.Parameters.Add(new SqlParameter("day", day.ToString("yyyy-MM-dd")));
                    cmd.CommandTimeout = 200;
                    var count = cmd.ExecuteScalar();
                    if (count != null && !string.IsNullOrEmpty(count.ToString()))
                    {
                        return decimal.Round( decimal.Parse(count.ToString()), 2) ;
                    }
                }
            }
            return 0.00m;
        }

        public int 实际超计划数(int companyId, DateTime day, bool is非重点)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 实际超计划数Sql + (is非重点 ? " and " + 非重点站Sql : "");
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("companyId", companyId));
                    cmd.Parameters.Add(new SqlParameter("day", day.ToString("yyyy-MM-dd")));
                    cmd.CommandTimeout = 200;
                    var count = cmd.ExecuteScalar();
                    if (count != null && !string.IsNullOrEmpty(count.ToString()))
                    {
                        return int.Parse(count.ToString());
                    }
                }
            }

            return 0;
        }

        public decimal 实际超计划面积(int companyId, DateTime day, bool is非重点)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 实际超计划面积Sql + (is非重点 ? " and " + 非重点站Sql : "");
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("companyId", companyId));
                    cmd.Parameters.Add(new SqlParameter("day", day.ToString("yyyy-MM-dd")));
                    cmd.CommandTimeout = 200;
                    var count = cmd.ExecuteScalar();
                    if (count != null && !string.IsNullOrEmpty(count.ToString()))
                    {
                        return decimal.Round(decimal.Parse(count.ToString()), 2);
                    }
                }
            }
            return 0.00m;
        }

        public decimal 核算执行到位率(int companyId, DateTime day, bool is非重点)
        {
            return decimal.Round(
                this.监测站供热面积(companyId, day, is非重点) != 0.0m ? 
                (this.监测站供热面积(companyId, day, is非重点) - this.实际超核算面积(companyId, day, is非重点)) / this.监测站供热面积(companyId, day, is非重点) : 0.0m, 4);
        }

        public decimal 计划执行到位率(int companyId, DateTime day, bool is非重点)
        {
            return decimal.Round(
                this.监测站供热面积(companyId, day, is非重点) != 0.0m ?
                (this.监测站供热面积(companyId, day, is非重点) - this.实际超计划面积(companyId, day, is非重点)) / this.监测站供热面积(companyId, day, is非重点) : 0.0m, 4);
        }

        public DataTable 到位率图表 (int companyId, DateTime fromDate, DateTime toDate, bool is非重点) {
            DataTable data = new DataTable();
            
            data.Columns.Add(new DataColumn("总投入面积", typeof(decimal)));
            data.Columns.Add(new DataColumn("实际超核算面积", typeof(decimal)));
            data.Columns.Add(new DataColumn("实际超计划面积", typeof(decimal)));
            data.Columns.Add(new DataColumn("日期", typeof(DateTime)));

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 执行到位率表Sql.Insert( 执行到位率表Sql.IndexOf("group") - 1,   (is非重点 ? " and " + 非重点站Sql : "" ));
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("companyId", companyId));
                    cmd.Parameters.Add(new SqlParameter("fromDay", fromDate.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("toDay", toDate.ToString("yyyy-MM-dd")));
                    cmd.CommandTimeout = 200;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = data.NewRow();
                            row["总投入面积"] = reader.GetDecimal(reader.GetOrdinal("总"));
                            row["日期"] = reader.GetDateTime(reader.GetOrdinal("日期"));
                            row["实际超核算面积"] = reader.GetDecimal(reader.GetOrdinal("实际超核算"));
                            row["实际超计划面积"] = reader.GetDecimal(reader.GetOrdinal("实际超计划"));
                            data.Rows.Add(row);
                        }
                        return data;
                    }
                }
                
            }
        }
    }
}