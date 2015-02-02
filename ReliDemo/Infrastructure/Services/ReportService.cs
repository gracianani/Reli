using ReliDemo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ReliDemo.Infrastructure.Services
{
    public class ReportService
    {
        private xz2013Entities db = new xz2013Entities();
        public string 回温Sql = @"select a.*,b.*
                                 from 
                                (select 
	                                a.itemid,
	                                a.[热力站名称],
	                                a.spcname,
	                                a.[管理单位],
	                                a.公司,
	                                a.[参考热指标],
	                                a.[数据来源],
	                                a.[是否重点站] ,
	                                a.[收费性质],
	                                a.[生产热源], 
	                                a.GJ_Limit,
	                                AVG(c.[一次回温]) as 一次回温平均 ,
	                                avg(c.[一次供温]) as 一次供温平均,
	                                avg(c.[一次供压]) as 一次供压平均,
	                                avg(c.[一次回压]) as 一次回压平均
	                                from stations a 
	                                join Station1stHistory c on c.热力站ID = a.itemId and c.时间 >= @fromDate and c.时间 < @toDate
	                                where  
		                                c.总瞬时流量<3000 and 
		                                c.总累计热量<(c.总累计流量*2) and 
		                                c.[一次回温]>45 and 
		                                c.[一次回温]<900 and 
		                                (a.[生产热源ID]=1 or a.[生产热源ID]=22)
	                                GROUP BY a.spcname,a.[热力站名称],a.itemid,a.[管理单位],a.公司,a.[参考热指标],a.[数据来源],a.[是否重点站] ,a.[收费性质],a.[生产热源],a.GJ_Limit
	                                )a
                                join StationAccuHistory b on a.itemid=b.热力站ID and b.日期=@fromDate  
                                where b.采暖GJ < a.GJ_Limit
                                order by a.itemid";

        public List<DailyReportItem> GetDailyReportData(DateTime fromDate, DateTime toDate, int 是否重点站, string 热源, string 收费性质, string 数据来源)
        {
            var result = new List<DailyReportItem>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Proc_Rpt_Plan_analyse";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("start_date", fromDate));
                    cmd.Parameters.Add(new SqlParameter("end_date", toDate));
                    cmd.Parameters.Add(new SqlParameter("是否重点站", 是否重点站));
                    cmd.Parameters.Add(new SqlParameter("热源", 热源));
                    cmd.Parameters.Add(new SqlParameter("收费性质", 收费性质));
                    cmd.Parameters.Add(new SqlParameter("数据来源", 数据来源));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dailyReportItem = new DailyReportItem();

                            if (!reader.IsDBNull(reader.GetOrdinal("日期")))
                                dailyReportItem.日期 = reader.GetDateTime(reader.GetOrdinal("日期"));

                            if (!reader.IsDBNull(reader.GetOrdinal("室外温度")))
                                dailyReportItem.室外温度 = reader.GetDecimal(reader.GetOrdinal("室外温度"));

                            if (!reader.IsDBNull(reader.GetOrdinal("销售1")))
                                dailyReportItem.销售执行到位率 = reader.GetDecimal(reader.GetOrdinal("销售1"));
                            if (!reader.IsDBNull(reader.GetOrdinal("特力昆1")))
                                dailyReportItem.特力昆执行到位率 = reader.GetDecimal(reader.GetOrdinal("特力昆1"));
                            if (!reader.IsDBNull(reader.GetOrdinal("天禹1")))
                                dailyReportItem.天禹执行到位率 = reader.GetDecimal(reader.GetOrdinal("天禹1"));
                            if (!reader.IsDBNull(reader.GetOrdinal("创合1")))
                                dailyReportItem.创合执行到位率 = reader.GetDecimal(reader.GetOrdinal("创合1"));
                            if (!reader.IsDBNull(reader.GetOrdinal("合计1")))
                                dailyReportItem.合计执行到位率 = reader.GetDecimal(reader.GetOrdinal("合计1"));

                            if (!reader.IsDBNull(reader.GetOrdinal("销售2")))
                                dailyReportItem.销售超标站总面积 = reader.GetDecimal(reader.GetOrdinal("销售2"));
                            if (!reader.IsDBNull(reader.GetOrdinal("特力昆2")))
                                dailyReportItem.特力昆超标站总面积 = reader.GetDecimal(reader.GetOrdinal("特力昆2"));
                            if (!reader.IsDBNull(reader.GetOrdinal("天禹2")))
                                dailyReportItem.天禹超标站总面积 = reader.GetDecimal(reader.GetOrdinal("天禹2"));
                            if (!reader.IsDBNull(reader.GetOrdinal("创合2")))
                                dailyReportItem.创合超标站总面积 = reader.GetDecimal(reader.GetOrdinal("创合2"));
                            if (!reader.IsDBNull(reader.GetOrdinal("合计2")))
                                dailyReportItem.合计超标站总面积 = reader.GetDecimal(reader.GetOrdinal("合计2"));

                            if (!reader.IsDBNull(reader.GetOrdinal("销售3")))
                                dailyReportItem.销售有效站总面积 = reader.GetDecimal(reader.GetOrdinal("销售3"));
                            if (!reader.IsDBNull(reader.GetOrdinal("特力昆3")))
                                dailyReportItem.特力昆有效站总面积 = reader.GetDecimal(reader.GetOrdinal("特力昆3"));
                            if (!reader.IsDBNull(reader.GetOrdinal("天禹3")))
                                dailyReportItem.天禹有效站总面积 = reader.GetDecimal(reader.GetOrdinal("天禹3"));
                            if (!reader.IsDBNull(reader.GetOrdinal("创合3")))
                                dailyReportItem.创合有效站总面积 = reader.GetDecimal(reader.GetOrdinal("创合3"));
                            if (!reader.IsDBNull(reader.GetOrdinal("合计3")))
                                dailyReportItem.合计有效站总面积 = reader.GetDecimal(reader.GetOrdinal("合计3"));

                            if (!reader.IsDBNull(reader.GetOrdinal("销售4")))
                                dailyReportItem.销售有效站数 = reader.GetInt16(reader.GetOrdinal("销售4"));
                            if (!reader.IsDBNull(reader.GetOrdinal("特力昆4")))
                                dailyReportItem.特力昆有效站数 = reader.GetInt16(reader.GetOrdinal("特力昆4"));
                            if (!reader.IsDBNull(reader.GetOrdinal("天禹4")))
                                dailyReportItem.天禹有效站数 = reader.GetInt16(reader.GetOrdinal("天禹4"));
                            if (!reader.IsDBNull(reader.GetOrdinal("创合4")))
                                dailyReportItem.创合有效站数 = reader.GetInt16(reader.GetOrdinal("创合4"));
                            if (!reader.IsDBNull(reader.GetOrdinal("合计4")))
                                dailyReportItem.合计有效站数 = reader.GetInt16(reader.GetOrdinal("合计4"));

                            result.Add(dailyReportItem);

                        }
                    }
                }
            }
            return result;
        }

        public List<CompletionReportItem> GetCompletionReportData(DateTime fromDate, DateTime toDate, int 是否重点站 = 2, string 热源 = "ALL", string 收费性质 = "ALL", string 数据来源 = "ALL",
            int companyId = -1, bool 是否按分公司显示 = false)
        {
            var result = new List<CompletionReportItem>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Proc_Rpt_Rate_analyse";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("start_date", fromDate));
                    cmd.Parameters.Add(new SqlParameter("end_date", toDate));
                    cmd.Parameters.Add(new SqlParameter("公司", companyId == -1 ? "ALL" : CompanyHelper.GetCompanyById(companyId)));
                    cmd.Parameters.Add(new SqlParameter("分公司", 是否按分公司显示));
                    cmd.Parameters.Add(new SqlParameter("热源", 热源));
                    cmd.Parameters.Add(new SqlParameter("收费性质", 收费性质));
                    cmd.Parameters.Add(new SqlParameter("是否重点站", 是否重点站));
                    cmd.Parameters.Add(new SqlParameter("数据来源", 数据来源));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var completionReportItem = new CompletionReportItem();
                            completionReportItem.公司名 = reader.GetString(reader.GetOrdinal("公司"));
                            if (!reader.IsDBNull(reader.GetOrdinal("有效日供热总面积")))
                                completionReportItem.有效日总供热面积 = reader.GetDecimal(reader.GetOrdinal("有效日供热总面积"));
                            if (!reader.IsDBNull(reader.GetOrdinal("超标日总供热面积")))
                                completionReportItem.超标日总供热面积 = reader.GetDecimal(reader.GetOrdinal("超标日总供热面积"));
                            if (!reader.IsDBNull(reader.GetOrdinal("未超标日总供热面积")))
                                completionReportItem.未超标日总供热面积 = reader.GetDecimal(reader.GetOrdinal("未超标日总供热面积"));
                            if (!reader.IsDBNull(reader.GetOrdinal("采暖季执行到位率")))
                                completionReportItem.采暖季执行到位率 = reader.GetDecimal(reader.GetOrdinal("采暖季执行到位率"));
                            result.Add(completionReportItem);
                        }
                    }
                }
            }
            return result;
        }

        public List<StationAnalizeReportItem> GetStationAnalizeReportData(DateTime fromDate, DateTime toDate, int startIndex, int pageSize, out int total, int 是否重点站 = 2, string 热源 = "ALL", string 收费性质 = "ALL", 
            string 数据来源 = "ALL",
            int companyId = -1, int managershipId = -1)
        {
            var result = new List<StationAnalizeReportItem>();
            total = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Proc_Rpt_Station_analyse";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("start_date", fromDate));
                    cmd.Parameters.Add(new SqlParameter("end_date", toDate));
                    cmd.Parameters.Add(new SqlParameter("公司", companyId == -1 ? "ALL" : CompanyHelper.GetCompanyById(companyId)));
                    cmd.Parameters.Add(new SqlParameter("分公司", managershipId == -1 ? "ALL" : CompanyHelper.GetManagershipById(managershipId)));
                    cmd.Parameters.Add(new SqlParameter("热源", 热源));
                    cmd.Parameters.Add(new SqlParameter("收费性质", 收费性质));
                    cmd.Parameters.Add(new SqlParameter("是否重点站", 是否重点站));
                    cmd.Parameters.Add(new SqlParameter("数据来源", 数据来源));
                    cmd.Parameters.Add(new SqlParameter("rowNumber", startIndex * pageSize + 1));
                    cmd.Parameters.Add(new SqlParameter("pageSize", pageSize));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(reader.GetOrdinal("站面积")) || reader.IsDBNull(reader.GetOrdinal("收费性质")))
                            {
                                continue;
                            }
                            var stationAnalizeReportItem = new StationAnalizeReportItem();
                            stationAnalizeReportItem.热力站名称 = reader.GetString(reader.GetOrdinal("热力站名称"));
                            stationAnalizeReportItem.分公司 = reader.GetString(reader.GetOrdinal("分公司"));
                            stationAnalizeReportItem.管理单位 = reader.GetString(reader.GetOrdinal("管理单位"));
                            stationAnalizeReportItem.站面积 = reader.GetDecimal(reader.GetOrdinal("站面积"));
                            stationAnalizeReportItem.参考热指标 = reader.GetDecimal(reader.GetOrdinal("参考热指标"));
                            stationAnalizeReportItem.数据来源 = reader.GetString(reader.GetOrdinal("数据来源"));
                            stationAnalizeReportItem.是否重点站 = reader.GetBoolean(reader.GetOrdinal("是否重点站"));
                            stationAnalizeReportItem.收费性质 = reader.GetString(reader.GetOrdinal("收费性质"));
                            stationAnalizeReportItem.生产热源 = reader.GetString(reader.GetOrdinal("生产热源"));
                            if (!reader.IsDBNull(reader.GetOrdinal("统计天数")))
                                stationAnalizeReportItem.统计天数 = reader.GetInt16(reader.GetOrdinal("统计天数"));
                            if (!reader.IsDBNull(reader.GetOrdinal("有效天数")))
                                stationAnalizeReportItem.有效天数 = reader.GetInt16(reader.GetOrdinal("有效天数"));
                            if (!reader.IsDBNull(reader.GetOrdinal("无效天数")))
                                stationAnalizeReportItem.无效天数 = reader.GetInt16(reader.GetOrdinal("无效天数"));
                            if (!reader.IsDBNull(reader.GetOrdinal("超标天数")))
                                stationAnalizeReportItem.超标天数 = reader.GetInt16(reader.GetOrdinal("超标天数"));
                            if (!reader.IsDBNull(reader.GetOrdinal("未超标天数")))
                                stationAnalizeReportItem.未超标天数 = reader.GetInt16(reader.GetOrdinal("未超标天数"));

                            if (!reader.IsDBNull(reader.GetOrdinal("站天数到位率")))
                                stationAnalizeReportItem.站天数到位率 = reader.GetDecimal(reader.GetOrdinal("站天数到位率"));
                            if (!reader.IsDBNull(reader.GetOrdinal("站有效数据率")))
                                stationAnalizeReportItem.站有效数据率 = reader.GetDecimal(reader.GetOrdinal("站有效数据率"));
                            if (!reader.IsDBNull(reader.GetOrdinal("站无效数据率")))
                                stationAnalizeReportItem.站无效数据率 = reader.GetDecimal(reader.GetOrdinal("站无效数据率"));
                            if (!reader.IsDBNull(reader.GetOrdinal("站天数超标率")))
                                stationAnalizeReportItem.站天数超标率 = reader.GetDecimal(reader.GetOrdinal("站天数超标率"));

                            if (!reader.IsDBNull(reader.GetOrdinal("有效日计划供热量")))
                                stationAnalizeReportItem.有效日计划供热量 = reader.GetDecimal(reader.GetOrdinal("有效日计划供热量"));
                            if (!reader.IsDBNull(reader.GetOrdinal("有效日核算供热量")))
                                stationAnalizeReportItem.有效日核算供热量 = reader.GetDecimal(reader.GetOrdinal("有效日核算供热量"));
                            if (!reader.IsDBNull(reader.GetOrdinal("有效日实际供热量")))
                                stationAnalizeReportItem.有效日实际供热量 = reader.GetDecimal(reader.GetOrdinal("有效日实际供热量"));

                            if (!reader.IsDBNull(reader.GetOrdinal("未超标日总供热面积")))
                                stationAnalizeReportItem.未超标日总供热面积 = reader.GetDecimal(reader.GetOrdinal("未超标日总供热面积"));
                            if (!reader.IsDBNull(reader.GetOrdinal("超标日总供热面积")))
                                stationAnalizeReportItem.超标日总供热面积 = reader.GetDecimal(reader.GetOrdinal("超标日总供热面积"));
                            if (!reader.IsDBNull(reader.GetOrdinal("有效日供热总面积")))
                                stationAnalizeReportItem.有效日供热总面积 = reader.GetDecimal(reader.GetOrdinal("有效日供热总面积"));

                            result.Add(stationAnalizeReportItem);
                        }

                        if (reader.NextResult())
                        {
                            reader.Read();
                            total = reader.GetInt32(0);
                        }
                    }
                }

            }
            return result;
        }
        public IEnumerable<StationDetailReport> GetDailyReport(DateTime day, int startIndex, int pageSize, out int total)
        {
            var result = new List<StationDetailReport>();
            total = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[Proc_Rpt_总明细]";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("start_date", day.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("end_date", day.AddDays(1).ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("rowNumber", startIndex * pageSize+ 1));
                    cmd.Parameters.Add(new SqlParameter("pageSize", pageSize));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new StationDetailReport(reader));
                        }
                        if (reader.NextResult())
                        {
                            reader.Read();
                            total = reader.GetInt32(0);
                        }
                    }
                }
            }
            return result;
        }


        public IEnumerable<StationDetailReport> GetFailureStations(DateTime day, int startIndex, int pageSize, out int total)
        {
            var result = new List<StationDetailReport>();
            total = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[Proc_Rpt_故障明细]";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("start_date", day.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("end_date", day.AddDays(1).ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("rowNumber", startIndex * pageSize + 1));
                    cmd.Parameters.Add(new SqlParameter("pageSize", pageSize));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new StationDetailReport(reader));
                        }
                        if (reader.NextResult())
                        {
                            reader.Read();
                            total = reader.GetInt32(0);
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<StationDetailReport> Get超核算Stations(DateTime day, int startIndex, int pageSize, out int total)
        {
            var result = new List<StationDetailReport>();
            total = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[Proc_Rpt_实际超核算明细]";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("start_date", day.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("end_date", day.AddDays(1).ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("rowNumber", startIndex * pageSize + 1));
                    cmd.Parameters.Add(new SqlParameter("pageSize", pageSize));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new StationDetailReport(reader));
                        }
                        if (reader.NextResult())
                        {
                            reader.Read();
                            total = reader.GetInt32(0);
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<StationDetailReport> GetExceed45Stations(DateTime day, int startIndex, int pageSize, out int total)
        {
            var result = new List<StationDetailReport>();
            total = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[dbo].[Proc_Rpt_回温超标明细]";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("start_date", day.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("end_date", day.AddDays(1).ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("rowNumber", startIndex * pageSize + 1));
                    cmd.Parameters.Add(new SqlParameter("pageSize", pageSize));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new StationDetailReport(reader));
                        } 
                        if (reader.NextResult())
                        {
                            reader.Read();
                            total = reader.GetInt32(0);
                        }
                    }
                }
            }
            return result;
        }
    }

}
