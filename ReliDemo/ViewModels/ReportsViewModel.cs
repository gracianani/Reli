using ReliDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.ViewModels
{
    public class ReportIndexViewModel
    {
        public string ReportStartAt { get; set; }
        public string ReportEndAt { get; set; }
        public string ReportAt { get; set; }
    }

    public class ReportsViewModel
    {
        public IEnumerable<StationDetailReport> Report { get; set; }
        public DateTime Day { get; set; }
        public string Title { get; set; }
        public int ReportTypeId { get; set; }

        private int _totalCount;
        public int TotalCount
        {
            get
            {
                return _totalCount;
            }
            set
            {
                _totalCount = value;
            }
        }
        private int _pageSize;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        private int _pageIndex;
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalCount / (double)PageSize);
            }
        }
    }

    public class StatViewModel
    {
        public List<CompanyStat> Report { get; set; }
        public DateTime Day { get; set; }
        public string Title { get; set; }
        public int ReportTypeId { get; set; }
    }

    public class RangeStatViewModel
    {
        public Dictionary<DateTime, List<CompanyStat>> Report { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Dictionary<DateTime, decimal> Temperature { get; set; }
        public string Title { get; set; }
        public int ReportTypeId { get; set; }
    }

    public class DailyReportViewModel
    {
        public IList<DailyReportItem> ReportData { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Title { get; set; }
        public int ReportTypeId { get; set; }
        public IList<IdAndName> Companies { get; set; }
        private IList<IdAndName> _managerships;
        public IList<IdAndName> Managerships
        {
            get
            {
                if (_managerships == null)
                {
                    _managerships = new List<IdAndName>();
                }
                return _managerships;
            }
            set
            {
                _managerships = value;
            }
        }
        public int 是否重点站 { get; set; }
        public string 收费性质 { get; set; }
        public string 数据来源 { get; set; }
        public string 热源 { get; set; }
        public int? SelectedManagershipId { get; set; }
        public int? SelectedCompanyId { get; set; }

    }

    public class CompletionReportViewModel
    {
        public IList<CompletionReportItem> ReportData { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Title { get; set; }
        public int ReportTypeId { get; set; }
        public IList<IdAndName> Companies { get; set; }
        private IList<IdAndName> _managerships;
        public IList<IdAndName> Managerships
        {
            get
            {
                if (_managerships == null)
                {
                    _managerships = new List<IdAndName>();
                }
                return _managerships;
            }
            set
            {
                _managerships = value;
            }
        }
        public int 是否重点站 { get; set; }
        public string 收费性质 { get; set; }
        public string 数据来源 { get; set; }
        public string 热源 { get; set; }
        public int? SelectedManagershipId { get; set; }
        public int? SelectedCompanyId { get; set; }
    }

    public class StationAnalizeViewModel
    {
        public IList<StationAnalizeReportItem> ReportData { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Title { get; set; }
        public int ReportTypeId { get; set; }
        public int CompanyId { get; set; }
        public IList<IdAndName> Companies { get; set; }
        private IList<IdAndName> _managerships;
        public IList<IdAndName> Managerships
        {
            get
            {
                if (_managerships == null)
                {
                    _managerships = new List<IdAndName>();
                }
                return _managerships;
            }
            set
            {
                _managerships = value;
            }
        }
        public int 是否重点站 { get; set; }
        public string 收费性质 { get; set; }
        public string 数据来源 { get; set; }
        public string 热源 { get; set; }
        public int? SelectedManagershipId { get; set; }
        public int? SelectedCompanyId { get; set; }

        private int _totalCount;
        public int TotalCount
        {
            get
            {
                return _totalCount;
            }
            set
            {
                _totalCount = value;
            }
        }
        private int _pageSize;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        private int _pageIndex;
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalCount / (double)PageSize);
            }
        }
    }

}