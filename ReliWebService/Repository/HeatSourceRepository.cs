using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace ReliWebService.Repository
{
    public class HeatSourceRepository
    {
        private xz2013Entities db = new xz2013Entities();
        private ReliMobileEntities dbm = new ReliMobileEntities();

        private IEnumerable<ReliMobileHeatSource> _heatSources;
        public IEnumerable<ReliMobileHeatSource> HeatSources
        {
            get
            {
                return _heatSources;
            }
            set
            {
                _heatSources = value;
            }
        }

        private IEnumerable<ReliMobileHeatSourceRecent> _heatSourceRecents;
        public IEnumerable<ReliMobileHeatSourceRecent> HeatSourceRecents
        {
            get
            {
                if (_heatSourceRecents == null)
                {
                    _heatSourceRecents = new List<ReliMobileHeatSourceRecent>();
                }
                return _heatSourceRecents;
            }
            set
            {
                _heatSourceRecents = value;
            }
        }
        private static string 按小时查询 = @"select 
                Min(时间) as 时间,
                ISNull( avg(供温), 0) as 供温,
                ISNull( avg(回温), 0) as 回温,
                ISNull( avg(供压), 0) as 供压,
                ISNull( avg(回压), 0) as 回压

                from heatsourcehistory 
                where 时间 >= @fromTime and 时间<@toTime 
                and 生产热源ID = @heatsourceId
                and 机组号 = @unitId
                group by datepart(hour, 时间)";
        private static string 按日期查询 = @"select 
                Min(时间) as 时间,
                ISNull( avg(供温), 0) as 供温,
                ISNull( avg(回温), 0) as 回温,
                ISNull( avg(供压), 0) as 供压,
                ISNull( avg(回压), 0) as 回压

                from heatSourceHistory 
                where 时间 >= @fromTime and 时间<@toTime 
                and 生产热源ID = @heatsourceId
                and 机组号 = @unitId
                group by datepart(day, 时间)";

        public IEnumerable<HeatSourceGJ> FindAccuHistoryByDate(int heatSourceId, int unitId, DateTime fromDate, DateTime toDate)
        {
            var histories = from history in db.HeatSourceAccuHistories
                            where history.日期 >= fromDate && history.日期 <= toDate && history.生产热源ID == heatSourceId && history.机组号 == unitId
                            orderby history.日期 descending
                            select new
                            {
                                时间 = history.日期,
                                日累计GJ = history.日累计热量 
                            };
            return histories.ToList().Select(i => new HeatSourceGJ()
            {
                日期 = i.时间,
                dailyGJ = Convert.ToDecimal( i.日累计GJ )
            });
        }

        public IEnumerable<ReliMobileHeatSourceHistory> FindHistoryByDate(int heatSourceId, int unitId, DateTime fromDate, DateTime toDate)
        {
            var result = new List<ReliMobileHeatSourceHistory>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (Math.Abs((fromDate - toDate).Days) <= 1)
                    {
                        cmd.CommandText = 按小时查询;
                    }
                    else {
                        cmd.CommandText = 按日期查询;
                    }
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("fromTime", fromDate.ToString("yyyy-MM-dd HH:mm:ss")));
                    cmd.Parameters.Add(new SqlParameter("toTime", toDate.ToString("yyyy-MM-dd HH:mm:ss")));
                    cmd.Parameters.Add(new SqlParameter("heatSourceId", heatSourceId));
                    cmd.Parameters.Add(new SqlParameter("unitId", unitId));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new ReliMobileHeatSourceHistory(reader));
                        }
                    }
                }
            }
            return result;
        }

        public HeatSourceRepository()
        {
            _heatSources = new List<ReliMobileHeatSource>();
            _heatSources = db.HeatSourceRecents.GroupBy(i => i.生产热源ID).Select(i => new ReliMobileHeatSource() { DBHeatSource = db.HeatSources.FirstOrDefault(j=>j.ItemID == i.Key) });
            _heatSourceRecents = db.HeatSourceRecents.Select(i => new ReliMobileHeatSourceRecent() { DBHeatSourceRecent = i });
        }
    }
}