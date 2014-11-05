using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class DailyReports : List<DailyReport>
    {
        public DailyReports() { }
        public DailyReports(List<DailyReport> dailyReports)
            : base(dailyReports) { }
    }
}