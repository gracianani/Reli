using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Data.Objects;

namespace ReliDemo.Infrastructure.Services
{
    public static class QuerriesUtility
    {
        public static Func<xz2013Entities, DateTime, DateTime, int?, int?, int?, int?, string, bool?, string, IEnumerable<StationAccuHistory>>
          GetStationAccuHistories
        {
            get
            {
                Func<xz2013Entities, DateTime, DateTime, int?, int?, int?, int?, string, bool?, string, IEnumerable<StationAccuHistory>> func =
                  CompiledQuery.Compile<xz2013Entities, DateTime, DateTime, int?, int?, int?, int?, string, bool?, string, IEnumerable<StationAccuHistory>>
                  (
                    (xz2013Entities context, DateTime fromDate, DateTime toDate, int? 实际比核算From, int? 实际比核算To, int? 实际比计划From, int? 实际比计划To,
                        string 收费性质, bool? 是否重点站, string 数据来源 ) => 
                            context.StationAccuHistories.Where<StationAccuHistory>(i => 
                                (i.日期 >= fromDate && i.日期 <= toDate ) &&
                                (!实际比核算From.HasValue || i.核算多耗 >  实际比核算From) &&
                                (!实际比核算To.HasValue || i.核算多耗 < 实际比核算To) &&
                                (!实际比计划From.HasValue || i.计划多耗 > 实际比计划From) &&
                                (!实际比计划To.HasValue || i.计划多耗 < 实际比计划To) &&
                                (string.IsNullOrEmpty(收费性质) || i.Station.收费性质 == 收费性质) &&
                                (!是否重点站.HasValue || i.Station.是否重点站 == 是否重点站) &&
                                (string.IsNullOrEmpty(数据来源) || (i.Station.数据来源 == 数据来源 || ( 数据来源 == "人工抄表" && i.Station.数据来源 == null) ) ) ));
                return func;
            }
        }

        public static Func<xz2013Entities, IEnumerable<StationAccuHistory>>
          GetYesterdayStationAccuHistories
        {
            get
            {
                Func<xz2013Entities, IEnumerable<StationAccuHistory>> func =
                  CompiledQuery.Compile<xz2013Entities, IEnumerable<StationAccuHistory>>
                  (
                    (xz2013Entities context ) =>
                            context.StationAccuHistories.Where<StationAccuHistory>(i =>
                                i.日期 >= DateTime.Today.AddDays(-2) && i.日期 <= DateTime.Today.AddDays(-1)));
                return func;
            }
        }
    }
}