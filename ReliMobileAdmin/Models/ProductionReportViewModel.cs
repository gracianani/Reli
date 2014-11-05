using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliMobileAdmin.Models
{
    public class ProductionReportViewModel
    {
        public IEnumerable<ReportItem> Reports { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public ReportType ReportType { get; set; }
    }
}