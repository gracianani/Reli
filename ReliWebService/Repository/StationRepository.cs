using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace ReliWebService.Repository
{
    public class StationRepository
    {
        private ReliMobileEntities db = new ReliMobileEntities();
        private xz2013Entities db2 = new xz2013Entities();
        private List<ReliMobileStation> _stations;
        private static string 按小时查询 = @"select 
                    Min(时间) as 时间, 
                    ISNULL( avg(一次供温), 0 ) as 一次供温, 
                    ISNULL( avg(一次回温), 0 ) as 一次回温, 
                    ISNULL( avg(一次供压), 0 ) as 一次供压, 
                    ISNULL( avg(一次回压), 0 ) as 一次回压
                    from station1sthistory history
                    where history.时间 >= @fromTime and history.时间<=@toTime and 热力站ID=@stationId
                    group by DATEPART(HOUR, 时间)";
        private static string 按日期查询 = @"select 
                    日期 as 时间,
                    ISNull( 供温avg, 0) as 一次供温,
                    ISNull( 回温avg, 0) as 一次回温,
                    ISNull( 供压avg, 0) as 一次供压,
                    ISNull( 回压avg, 0) as 一次回压
                    from StationAccuHistory 
                    where 日期 >= @fromTime and 日期<@toTime 
                    and 热力站ID = @stationId";
        public List<ReliMobileStation> Stations
        {
            get
            {
                return _stations;
            }
            set
            {
                _stations = value;
            }
        }
        public IEnumerable<ReliMobileStationHistory> FindHistoryByDate(int stationId, DateTime fromDate, DateTime toDate)
        {
            var result = new List<ReliMobileStationHistory>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (Math.Abs((fromDate - toDate).Days ) <= 1)
                    {
                        cmd.CommandText = 按小时查询;
                    }else {
                        cmd.CommandText = 按日期查询;
                    }
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("fromTime", fromDate.ToString("yyyy-MM-dd HH:mm:ss")));
                    cmd.Parameters.Add(new SqlParameter("toTime", toDate.ToString("yyyy-MM-dd HH:mm:ss")));
                    cmd.Parameters.Add(new SqlParameter("stationId", stationId));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new ReliMobileStationHistory(reader));
                        }
                    }
                }
            }
            return result;
        }

        public StationRepository()
        {
            _stations = new List<ReliMobileStation>();
            _stations = db.Stations.Select(i=> new ReliMobileStation() { DBStation = i }).ToList();
        }
    }
}