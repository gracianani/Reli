using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using ReliDemo.ViewModels;


namespace ReliDemo.Infrastructure.Services
{
    public class StationsService
    {
        private xz2013Entities db = new xz2013Entities();
        private const string 总明细Sql = @"select a.itemid,a.[热力站名称],a.spcname,a.[管理单位],a.公司,a.[参考热指标],a.[数据来源] ,a.[是否重点站],a.[收费性质],a.[生产热源], a.GJ_Limit
                        ,c.回温avg  as 一次回温平均 ,c.供温avg as 一次供温平均,c.供压avg as 一次供压平均, c.回压avg as 一次回压平均, c.*
                        from stations a
                        join StationAccuHistory c on a.itemID = c.热力站ID and c.日期>=@fromDate and c.日期<@toDate 
                        where (c.报警 is null or c.报警 = '') and c.采暖GJ < a.GJ_Limit and 
                            (a.[生产热源ID]=1 or a.[生产热源ID]=22) ";

        private const string 故障Sql = @"select a.itemid,a.[热力站名称],a.spcname,a.[管理单位],a.公司,a.[参考热指标],a.[数据来源] ,a.[是否重点站],a.[收费性质],a.[生产热源], a.GJ_Limit
                        ,c.回温avg  as 一次回温平均 ,c.供温avg as 一次供温平均,c.供压avg as 一次供压平均, c.回压avg as 一次回压平均, c.*
                        from stations a
                        join StationAccuHistory c on a.itemID = c.热力站ID and c.日期>=@fromDate and c.日期<@toDate 
                        where (c.报警 is not null and c.报警 <> '' or c.采暖GJ >= a.GJ_Limit ) and 
                            (a.[生产热源ID]=1 or a.[生产热源ID]=22)";
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

        public string 实际超核算Sql = @"select a.itemid,a.[热力站名称],a.spcname,a.[管理单位],a.公司,a.[参考热指标],a.[数据来源] ,a.[是否重点站],a.[收费性质],a.[生产热源], a.GJ_Limit
                        ,c.回温avg  as 一次回温平均 ,c.供温avg as 一次供温平均,c.供压avg as 一次供压平均, c.回压avg as 一次回压平均, c.*
                        from stations a
                        join StationAccuHistory c on a.itemID = c.热力站ID and c.日期>=@fromDate and c.日期<@toDate 
                        where (c.报警 is  null or c.报警 <> '') and c.采暖GJ >= a.GJ_Limit and 
                            (a.[生产热源ID]=1 or a.[生产热源ID]=22)  and  c.[核算GJ]<c.[采暖GJ]  
                                    order by a.itemid";

        public string 温度统计 = @"select 
	                            测温日期,
	                            热力站名称,
	                            sum( case when 室内温度 < 16 and 节能建筑 = '否' then 1 else 0 end ) as '16°C以下 非节能',
	                            sum( case when 室内温度 < 16 and 节能建筑 = '是' then 1 else 0 end ) as '16°C以下 节能',
	                            sum( case when 室内温度 >= 16 and 室内温度<18 and 节能建筑 = '否' then 1 else 0 end ) as '16°C-18°C 非节能',
	                            sum( case when 室内温度 >= 16 and 室内温度<18 and 节能建筑 = '是' then 1 else 0 end ) as '16°C-18°C 节能',
	                            sum( case when 室内温度 >= 18 and 室内温度<20 and 节能建筑 = '否' then 1 else 0 end ) as '18°C-20°C 非节能',
	                            sum( case when 室内温度 >= 18 and 室内温度<20 and 节能建筑 = '是' then 1 else 0 end ) as '18°C-20°C 节能',
	                            sum( case when 室内温度 >= 20 and 室内温度<22 and 节能建筑 = '否' then 1 else 0 end ) as '20°C-22°C 非节能',
	                            sum( case when 室内温度 >= 20 and 室内温度<22 and 节能建筑 = '是' then 1 else 0 end ) as '20°C-22°C 节能',
	                            sum( case when 室内温度 >= 22 and 室内温度<24 and 节能建筑 = '否' then 1 else 0 end ) as '22°C-24°C 非节能',
	                            sum( case when 室内温度 >= 22 and 室内温度<24 and 节能建筑 = '是' then 1 else 0 end ) as '22°C-24°C 节能',
	                            sum( case when 室内温度 >= 24 and 节能建筑 = '否' then 1 else 0 end ) as '24°C以上 非节能',
	                            sum( case when 室内温度 >= 24 and 节能建筑 = '是' then 1 else 0 end ) as '24°C以上 节能',
	                            count(*) 合计
	                            from [RoomTemperature]
                                where 热力站名称 like @stationName 
	                            group by 测温日期, 热力站名称";

        public IEnumerable<HeatIndexAudit> GetHeatIndexAudits(int stationId)
        {
            return db.HeatIndexAudits.Where(i => i.热力站ID == stationId).OrderByDescending(i => i.UpdatedAt);
        }

        public IEnumerable<Station> GetStationsRealTime()
        {
            return db.Stations.OrderByDescending(i => i.采集时间).ToList();
        }

        public IEnumerable<Station> GetStationsRealTime(DateTime from, DateTime to)
        {
            return db.Stations.Where(i=>i.采集时间 > from && i.采集时间 <= to).OrderByDescending(i=>i.采集时间).ToList();
        }
        public IEnumerable<StationAccuHistory> GetStationsAccuHistory()
        {
            return 
                from accuHistory in db.StationAccuHistories select  accuHistory;
        }
        public IEnumerable<StationAccuHistory> GetStationsAccuHistory(DateTime from, DateTime to)
        {
            return db.StationAccuHistories.Where(i => i.日期 >= from && i.日期 <= to && i.Station != null );
        }
        public IEnumerable<StationAccuHistory> GetStationsAccuHistory(DateTime from, DateTime to, 
                                                                        int? 实际比核算From, int? 实际比核算To, int? 实际比计划From, int? 实际比计划To, 
                                                                         int? companyId, int? managershipId, string 热源, string 收费性质, int 是否重点站, string 数据来源, 
                                                                     int startPageIndex, int pageSize, out int total)
        {
            total = 0;
            var result = new List<StationAccuHistory>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[Proc_Rpt_Stations_Detail]";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("start_date", from.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("end_date", to.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("公司", companyId.HasValue? CompanyHelper.GetCompanyById(companyId.Value) : "ALL" ));
                    cmd.Parameters.Add(new SqlParameter("分公司", managershipId.HasValue ? CompanyHelper.GetManagershipById(managershipId.Value) : "ALL"));
                    cmd.Parameters.Add(new SqlParameter("热源", 热源));
                    cmd.Parameters.Add(new SqlParameter("收费性质", 收费性质));
                    cmd.Parameters.Add(new SqlParameter("是否重点站", 是否重点站));
                    cmd.Parameters.Add(new SqlParameter("数据来源", 数据来源));
                    if (实际比核算From.HasValue)
                    {
                        cmd.Parameters.Add(new SqlParameter("实际比核算From", 实际比核算From));
                    }
                    if (实际比核算To.HasValue)
                    {
                        cmd.Parameters.Add(new SqlParameter("实际比核算To", 实际比核算To));
                    }
                    if (实际比计划From.HasValue)
                    {
                        cmd.Parameters.Add(new SqlParameter("实际比计划From", 实际比计划From));
                    }
                    if (实际比计划To.HasValue)
                    {
                        cmd.Parameters.Add(new SqlParameter("实际比计划To", 实际比计划To));
                    }
                    cmd.Parameters.Add(new SqlParameter("startIndex", startPageIndex * pageSize + 1));
                    cmd.Parameters.Add(new SqlParameter("pageSize", pageSize));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new StationAccuHistory(reader));
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
            //return QuerriesUtility.GetStationAccuHistories(db, from, to, 实际比核算From, 实际比核算To, 实际比计划From, 实际比计划To, 收费性质, 是否重点站, 数据来源);
        }

        public IEnumerable<StationsAccuByDaysSpan> GetStationsStat(DateTime from, DateTime to,
                                                                         int? companyId, int? managershipId, string 热源, string 收费性质, int 是否重点站, string 数据来源)
        {
            var result = new List<StationsAccuByDaysSpan>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[Proc_Rpt_Stations_GJ3]";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("start_date", from.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("end_date", to.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("公司", companyId.HasValue ? CompanyHelper.GetCompanyById(companyId.Value) : "ALL"));
                    cmd.Parameters.Add(new SqlParameter("分公司", managershipId.HasValue ? CompanyHelper.GetManagershipById(managershipId.Value) : "ALL"));
                    cmd.Parameters.Add(new SqlParameter("热源", 热源));
                    cmd.Parameters.Add(new SqlParameter("收费性质", 收费性质));
                    cmd.Parameters.Add(new SqlParameter("是否重点站", 是否重点站));
                    cmd.Parameters.Add(new SqlParameter("数据来源", 数据来源));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var StationsAccuByDaysSpanItem = new StationsAccuByDaysSpan();
                            StationsAccuByDaysSpanItem.热力站名称 = reader.GetString(reader.GetOrdinal("热力站名称"));
                            if (!reader.IsDBNull(reader.GetOrdinal("计划供热量")))
                                StationsAccuByDaysSpanItem.计划GJ = reader.GetDecimal(reader.GetOrdinal("计划供热量"));
                            if (!reader.IsDBNull(reader.GetOrdinal("核算供热量")))
                                StationsAccuByDaysSpanItem.核算GJ = reader.GetDecimal(reader.GetOrdinal("核算供热量"));
                            if (!reader.IsDBNull(reader.GetOrdinal("实际供热量")))
                                StationsAccuByDaysSpanItem.实际GJ = reader.GetDecimal(reader.GetOrdinal("实际供热量"));
                            if (!reader.IsDBNull(reader.GetOrdinal("公司")))
                                StationsAccuByDaysSpanItem.公司 = reader.GetString(reader.GetOrdinal("公司"));
                            if (!reader.IsDBNull(reader.GetOrdinal("中心")))
                                StationsAccuByDaysSpanItem.中心 = reader.GetString(reader.GetOrdinal("中心"));
                            if (!reader.IsDBNull(reader.GetOrdinal("收费性质")))
                                StationsAccuByDaysSpanItem.收费性质 = reader.GetString(reader.GetOrdinal("收费性质"));
                            if (!reader.IsDBNull(reader.GetOrdinal("是否重点站")))
                                StationsAccuByDaysSpanItem.是否重点站 = reader.GetString(reader.GetOrdinal("是否重点站"));
                            if (!reader.IsDBNull(reader.GetOrdinal("数据来源")))
                                StationsAccuByDaysSpanItem.数据来源 = reader.GetString(reader.GetOrdinal("数据来源"));
                            StationsAccuByDaysSpanItem.时间段 = string.Format("{0:yyyy-MM-dd} - {1:yyyy-MM-dd}", from, to);
                            if (!reader.IsDBNull(reader.GetOrdinal("热源")))
                                StationsAccuByDaysSpanItem.热源 = reader.GetString(reader.GetOrdinal("热源"));
                            result.Add(StationsAccuByDaysSpanItem);
                        }
                    }
                }
            }
            return result;
            //return QuerriesUtility.GetStationAccuHistories(db, from, to, 实际比核算From, 实际比核算To, 实际比计划From, 实际比计划To, 收费性质, 是否重点站, 数据来源);
        }
        public IEnumerable<StationAccuHistory> GetStationsAccuHistory(int stationId, DateTime from, DateTime to)
        {
            return db.StationAccuHistories.Where(i => i.日期 >= from && i.日期 <= to && i.Station != null && i.Station.ItemID == stationId);
        }

        public IEnumerable<StationDetailReport> GetDailyReport(DateTime day)
        {
            var result = new List<StationDetailReport>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 总明细Sql;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("fromDate", day.ToString("yyyy-MM-dd") ));
                    cmd.Parameters.Add(new SqlParameter("toDate", day.AddDays(1).ToString("yyyy-MM-dd")));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new StationDetailReport(reader));
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<StationTemperatureStats> GetTemperatureStats(string stationName)
        {
            var result = new List<StationTemperatureStats>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 温度统计;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("stationName", stationName));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new StationTemperatureStats(reader));
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<string> GetTemperatureImportFileName()
        {
            var result = new List<string>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT distinct [导入文件名] FROM [xz2013].[dbo].[RoomTemperature]";
                    cmd.CommandType = System.Data.CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<StationDetailReport> GetFailureStations(DateTime day)
        {
            var result = new List<StationDetailReport>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 故障Sql;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("fromDate", day.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("toDate", day.AddDays(1).ToString("yyyy-MM-dd")));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new StationDetailReport(reader));
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<StationDetailReport> Get超核算Stations(DateTime day)
        {
            var result = new List<StationDetailReport>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 实际超核算Sql;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("fromDate", day.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("toDate", day.AddDays(1).ToString("yyyy-MM-dd")));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new StationDetailReport(reader));
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<StationDetailReport> GetExceed45Stations(DateTime day)
        {
            var result = new List<StationDetailReport>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 回温Sql;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("fromDate", day.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("toDate", day.AddDays(1).ToString("yyyy-MM-dd")));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new StationDetailReport(reader));
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<RoomTemperature> GetRoomTemperature()
        {
            return db.RoomTemperatures.OrderByDescending(i=>i.ItemId);
        }

        public void InsertManualInputData(StationManualInput stationManualInput)
        {
            if (db.StationManualInputs.Count(i => i.生产编号 == stationManualInput.生产编号 && i.采集时间 == stationManualInput.采集时间) == 0)
            {
                db.StationManualInputs.AddObject(stationManualInput);
                db.SaveChanges();
            }
        }

        public bool InsertRoomTemperature(RoomTemperature roomTemperature)
        {
            if (db.RoomTemperatures.Count(i =>  i.测温日期.Value == roomTemperature.测温日期.Value && i.序号.Value == roomTemperature.序号.Value) == 0)
            {
                try
                {
                    db.RoomTemperatures.AddObject(roomTemperature);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    
                }
            }
            return false;
        }

        public void InsertToHeatIndexAudit(int 热力站ID, decimal heatIndex, int userId)
        {
            var heatIndexAudit = new HeatIndexAudit() { 热力站ID = 热力站ID, 参考热指标 = heatIndex, UpdatedByUserId = userId, UpdatedAt = DateTime.Now };
            db.HeatIndexAudits.AddObject(heatIndexAudit);
            db.SaveChanges();
        }

        public static string Get数据来源(int 数据来源ID)
        {
            if (数据来源ID == 1)
            {
                return
                    "监控";
            }
            else if (数据来源ID == 2)
            {
                return "智能卡";
            }
            else if (数据来源ID == 3)
            {
                return null;
            }
            else
            {
                return "";
            }
        }

        public static int? Get数据来源ID(string 数据来源)
        {
            if (数据来源 == "监控")
            {
                return 1;
            }
            else if (数据来源 == "智能卡")
            {
                return 2;
            }
            else if (string.IsNullOrEmpty(数据来源))
            {
                return 3;
            }
            else
            {
                return null;
            }
        }
    }
}