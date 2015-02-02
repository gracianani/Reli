using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ReliWebService.Repository
{
    public class ReportRepository
    {
        private string DailyReportsRoot {
            get {
                return System.Web.HttpContext.Current.Server.MapPath("~/DailyReports");
            }
        }

        private string CustomerServiceReportsRoot
        {
            get
            {
                return System.Web.HttpContext.Current.Server.MapPath("~/CustomerServiceReports");
            }
        }
 
        private List<DailyReport> _dailyReports;
        public List<DailyReport> DailyReports
        {
            get
            {
                return _dailyReports;
            }
            set
            {
                _dailyReports = value;
            }
        }

        private List<DailyReport> _customerServiceReports;
        public List<DailyReport> CustomerServiceReports
        {
            get
            {
                return _customerServiceReports;
            }
            set
            {
                _customerServiceReports = value;
            }
        }

        public ReportRepository()
        {
            var files =
                new DirectoryInfo(DailyReportsRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select( (f, index) => new DailyReport( index, f.Name, f.CreationTime ) )
                    .ToArray();
            _dailyReports = files.ToList();

            files =
                new DirectoryInfo(CustomerServiceReportsRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select((f, index) => new DailyReport(index, f.Name, f.CreationTime))
                    .ToArray();
            _customerServiceReports = files.ToList();
        }
    }
}