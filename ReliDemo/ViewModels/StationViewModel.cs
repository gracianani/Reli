using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ReliDemo.Models;

namespace ReliDemo.ViewModels
{
    public class HeatIndexHistoryViewModel
    {
        public IEnumerable<HeatIndexAudit> HeatIndexes { get; set; }
        public int StationId { get; set; }
    }

    public class ManualDataInputViewModel
    {
        public PaginatedList<StationManualInput> StationManualInput { get; set; }
        public IEnumerable<Station> Stations { get; set; }
        public string Selected生产编号 { get; set; }
        public string 热力站名称 { get; set; }
        public int Selected热表编号 { get; set; }
        public string 故障状态 { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class RoomTemperatureViewModel
    {
        public IEnumerable<RoomTemperature> RoomTemperatures { get; set; }
        public bool HasNewData { get; set; }
        public DateTime LastUploadedTime { get; set; }
        public int LastUploadedCount { get; set; }
        public IEnumerable<string> fileNames { get; set; } 
    }

    public class StationRoomTemperatureViewModel
    {
        public IEnumerable<RoomTemperature> RoomTemperatures { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; }
        public string 数据来源 { get; set; }
        public IEnumerable<StationTemperatureStats> Stats { get; set; }
    }

    public class StationRealTimeViewModel
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public string 数据来源 { get; set; }
        public GJHistoriesViewModel Histories { get; set; }
        public IEnumerable<Station2ndRecent> Station2ndRecents { get; set; }
        public IList<HeatConsumptionTotalItem> TodayAndYesterdayGJ { get; set; }
        public decimal? 一次供温 {get;set;}
        public decimal? 一次回温 {get;set;}
        public decimal? 一次供压 {get;set;}
        public decimal? 一次回压 {get;set;}
        public decimal? 总累计热量 {get;set;}
        public decimal? 总累计流量 {get;set;}
        public decimal? 当前热指标 { get; set; }
    }

    public class StationsRealTimeViewModel
    {
        public DateTime? UpdatedAt { get; set; }
        public IEnumerable<HeatSource> HeatSources { get; set; }
        private IEnumerable<IdAndName> _companies;
        public IEnumerable<IdAndName> Companies
        {
            get
            {
                if (_companies == null)
                {
                    _companies = new List<IdAndName>();
                }
                return _companies;
            }
            set
            {
                _companies = value;
            }
        }
        private IEnumerable<IdAndName> _managerships;
        public IEnumerable<IdAndName> Managerships
        {
            get
            {
                if (_managerships == null)
                {
                    _managerships = new List<IdAndName>();
                }
                if (SelectedCompanyId.HasValue)
                {
                    _managerships = CompanyHelper.GetAllManagershipByCompanyId(SelectedCompanyId.Value);
                }
                return _managerships;
            }
            set
            {
                _managerships = value;
            }
        }

        public List<IdAndName> Regions
        {
            get
            {
                return new List<IdAndName>() {
                    new IdAndName() { Id = (int)Region.东部, Name= Region.东部.ToString() },
                    new IdAndName() { Id = (int)Region.西部, Name= Region.西部.ToString()}
                };
            }
        }

        public int? SelectedHeatSourceId { get; set; }
        public int? SelectedCompanyId { get; set; }
        public int? SelectedManagershipId { get; set; }
        public int? SelectedArea { get; set; }

        public PaginatedList<Station> StationsRealTime { get; set; }
        public StationsRealTimeViewModel(IEnumerable<Station> realtime, int startIndex, int pageSize)
        {
            StationsRealTime = new PaginatedList<Station>(realtime, startIndex, pageSize);
        }
        [PlaceHolder("请输入热力站名称...")]
        public string SearchKeyword { get; set; }

        public decimal? 单耗超标From { get; set; }
        public decimal? 单耗超标To { get; set; }
        public decimal? 流量超标From { get; set; }
        public decimal? 流量超标To { get; set; }
        public decimal? 回温超标From { get; set; }
        public decimal? 回温超标To { get; set; }
    }

    public class StationsAccuByDaysSpan
    {
        public string 热力站名称;
        public decimal? 计划GJ;
        public decimal? 核算GJ;
        public decimal? 实际GJ;
        public decimal? 热水GJ;
        public string 时间段;
        public string 公司;
        public string 中心;
        public string 收费性质;
        public string 是否重点站;
        public string 数据来源;
        public string 热源;
    }

    public class StationsStatisticsViewModel
    {
        public PaginatedList<StationAccuHistory> StationsStatistic { get; set; }
        public PaginatedList<StationsAccuByDaysSpan> StationsMerged { get; set; }
        public StationsStatisticsViewModel(IEnumerable<StationAccuHistory> histories, string searchSpan, int startIndex, int pageSize, int totalCount)
        {
            _totalCount = totalCount;
            SearchSpan = searchSpan;
            //if (searchSpan == "stat")
            //{
            //    var merged = histories.GroupBy(i => i.热力站ID).OrderBy(i => i.Key).Skip(startIndex * pageSize).Take(pageSize).Select(i => new StationsAccuByDaysSpan()
            //    {
            //        Station = i.First().Station,
            //        实际GJ = i.Sum(j => j.采暖GJ),
            //        核算GJ = i.Sum(j => j.核算GJ),
            //        计划GJ = i.Sum(j => j.计划GJ),
            //        热水GJ = i.Sum(j => j.热水GJ),
            //        热力站名称 = i.First().Station.热力站名称,
            //        时间段 = string.Format("{0:yyyy-MM-dd} - {1:yyyy-MM-dd}", i.Min(j => j.日期), i.Max(j => j.日期))
            //    });
            //    var count = histories.GroupBy(i => i.热力站ID).Count();
            //    StationsMerged = 
            //}
            StationsStatistic = new PaginatedList<StationAccuHistory>(histories, startIndex, pageSize, TotalCount);
        }

        public StationsStatisticsViewModel(IEnumerable<StationsAccuByDaysSpan> merged, string searchSpan, int startIndex, int pageSize, int totalCount )
        {
            _totalCount = totalCount;
            SearchSpan = searchSpan;
            StationsMerged = new PaginatedList<StationsAccuByDaysSpan>(merged, startIndex, pageSize, totalCount);
        }
        public int? SelectedSearchMethod { get; set; }
        public string SearchSpan { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate {get;set;}
        private int _totalCount;
        public int TotalCount
        {
            get
            {
                if (SearchSpan == "stat")
                {
                    return StationsMerged.TotalCount;
                }
                else
                {
                    return _totalCount;
                }
            }
        }
        public int PageSize
        {
            get
            {
                if (SearchSpan == "stat")
                {
                    return StationsMerged.PageSize;
                }
                else
                {
                    return StationsStatistic.PageSize;
                }
            }
        }
        public int PageIndex { 
            get {
                if (SearchSpan == "stat")
                {
                    return StationsMerged.PageIndex;
                }
                else
                {
                    return StationsStatistic.PageIndex;
                }
            } 
        }
        public int TotalPages
        {
            get
            {
                if (SearchSpan == "stat")
                {
                    return StationsMerged.TotalPages;
                }
                else
                {
                    return (int)Math.Ceiling(TotalCount / (double)PageSize);
                }
            }
        }

        public List<IdAndName> Regions { 
            get
            {
                return new List<IdAndName>()
                {
                    new IdAndName() { Id=1, Name="东部" },
                    new IdAndName() { Id=2, Name="西部" }
                };
            }
        }
        public int? SelectedRegion { get; set; }
        private IEnumerable<IdAndName> _companies;
        public IEnumerable<IdAndName> Companies
        {
            get
            {
                if (_companies == null)
                {
                    _companies = new List<IdAndName>();
                }
                return _companies;
            }
            set
            {
                _companies = value;
            }
        }
        public IEnumerable<IdAndName> 重点站
        {
            get
            {
                return new List<IdAndName>() { new IdAndName() { Id=1, Name="是" } , new IdAndName() { Id=0, Name="否" } }; 
            }
        }
        private IEnumerable<IdAndName> _managerships;
        public IEnumerable<IdAndName> Managerships
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
        private IEnumerable<IdAndName> _heatsources;
        public IEnumerable<IdAndName> HeatSources
        {
            get
            {
                if (_heatsources == null)
                {
                    _heatsources = new List<IdAndName>();
                }
                return _heatsources;
            }
            set
            {
                _heatsources = value;
            }
        }
        public int? SelectedCompanyId { get; set; }
        public int? SelectedManagershipId { get; set; }
        public int? SelectedHeatSourceId { get; set; }
        public int? 实际比核算From { get; set; }
        public int? 实际比核算To { get; set; }
        public int? 实际比计划From { get; set; }
        public int? 实际比计划To { get; set; }
        public string 收费性质 { get; set; }
        public int 是否重点站 { get; set; }
        public string 数据来源 { get; set; }
    }

    public class StationsViewModel
    {
        public PaginatedList<Station> Stations { get;  set; }
        public IEnumerable<HeatSource> HeatSources { get;  set; }
        private IEnumerable<IdAndName> _companies;
        public IEnumerable<IdAndName> Companies 
        { 
            get
            {
                if (_companies == null)
                {
                    _companies = new List<IdAndName>();
                }
                return _companies;
            }
            set
            {
                _companies = value;
            }
        }
        private IEnumerable<IdAndName> _managerships;
        public IEnumerable<IdAndName> Managerships
        {
            get
            {
                if (_managerships == null)
                {
                    _managerships = new List<IdAndName>();
                }
                if (SelectedCompanyId.HasValue)
                {
                    _managerships = CompanyHelper.GetAllManagershipByCompanyId(SelectedCompanyId.Value);
                }
                return _managerships;
            }
             set
            {
                _managerships = value;
            }
        }
        public List<IdAndName> Regions
        {
            get
            {
                return new List<IdAndName>()
                {
                    new IdAndName() { Id=1, Name="东部" },
                    new IdAndName() { Id=2, Name="西部" }
                };
            }
        }
        public IEnumerable<IdAndName> 重点站
        {
            get
            {
                return new List<IdAndName>() { new IdAndName() { Id = 1, Name = "是" }, new IdAndName() { Id = 0, Name = "否" } };
            }
        }
        public IEnumerable<IdAndName> 故障站
        {
            get
            {
                return new List<IdAndName>() { new IdAndName() { Id = 1, Name = "是" }, new IdAndName() { Id = 0, Name = "否" } };
            }
        }
        public int? SelectedHeatSourceId { get; set; }
        public int? SelectedCompanyId { get; set; }
        public int? SelectedManagershipId { get; set; }
        public int? SelectedRegion { get; set; }
        public string SearchKeyword { get; set; }
        public string 数据来源 { get; set; }
        public string 是否重点站 { get; set; }
        public string 是否故障站
        {
            get;
            set;
        }
        public string 收费性质 { get; set; }
        public StationsViewModel(IEnumerable<Station> stations, int startIndex, int pageSize)
        {
            Stations = new PaginatedList<Station>(stations, startIndex, pageSize);
        }
    }
    
    public class StationCustomersViewModel {
        public List<Customer> Customers { get; private set; }
        public int StationId { get; private set; }
        public string StationName { get; private set; }
        public string 数据来源 { get; set; }
        public StationCustomersViewModel(IQueryable<Customer> source, int stationId, string stationName, string stationType)
        {
            Customers = source.ToList();
            StationId = stationId;
            StationName = stationName;
            数据来源 = stationType;
        }
    }

    public class StationHistory
    {
        public int StationId { get; set; }
        public DateTime Date { get; set; }
        public decimal 计划供热量 { get; set; }
        public decimal 核算供热量 { get; set; }
        public decimal 实际供热量 { get { return 实际采暖; } }

        public decimal 实际采暖 { get; set; }
        public decimal 实际热水 { get; set; }

        public decimal 预报温度 { get; set; }
        public decimal 实际温度 { get; set; }

        public decimal 实际比核算多耗 { 
            get {
                return 实际供热量 - 核算供热量;
            } 
        }

        public decimal 实际比核算多耗percent
        {
            get
            {
                return 核算供热量 != 0 ? (实际比核算多耗 / Convert.ToDecimal(核算供热量)) * 100.0m : 0.0m;
            }
        }

        public decimal 实际比计划多耗
        {
            get
            {
                return 实际供热量 - 计划供热量;
            }
        }
        public decimal 实际比计划多耗percent
        {
            get
            {
                return 计划供热量 != 0 ? (实际比计划多耗 / Convert.ToDecimal(计划供热量)) * 100.0m : 0.0m;
            }
        }

        public decimal 投入面积 { get; set; }

        public decimal 计算热指标 { get; set; }
        public decimal 计划热指标 { get; set; }
        public decimal 实际热指标 { get; set; }
        public decimal 核算热指标 { get; set; }

        public StationHistory(StationAccuHistory accu)
        {
            
            StationId = accu.Station.ItemID;
            Date = accu.日期;
            计划供热量 = Convert.ToDecimal(accu.计划GJ);
            预报温度 = Convert.ToDecimal(accu.预报温度);
            实际热水 = Convert.ToDecimal(accu.热水GJ);
            实际采暖 = Convert.ToDecimal(accu.采暖GJ);
            实际温度 = Convert.ToDecimal(accu.实际温度);
            核算供热量 = Convert.ToDecimal(accu.核算GJ);
            投入面积 = Convert.ToDecimal(accu.投入面积);
            计算热指标 = Convert.ToDecimal( accu.Station.参考热指标);
            计划热指标 = Convert.ToDecimal( accu.计划热指标);
            实际热指标 = Convert.ToDecimal(accu.实际热指标);
            核算热指标 = Convert.ToDecimal(accu.核算热指标);
        }
    }
    public class StationHistoriesViewModel
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public string SearchSpan { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string 计算热指标 { get; set; }
        public string 计划热指标 { get; set; }
        public string 实际运行热指标 { get; set; }
        public string 核算热指标 { get; set; }
        public string 计划供热量GJ { get; set; }
        public string 实际供热量GJ { get; set; }
        public string 核算供热量GJ { get; set; }
        public string 预报温度 { get; set; }
        public string 实际温度 { get; set; }
        public string 数据来源 { get; set; }
        public PaginatedList<StationHistory> StationAccuHistories { get; set; }
        public StationHistoriesViewModel(int stationId, string stationName, int pageIndex, int pageSize, IEnumerable<StationHistory> history)
        {
            StationId = stationId;
            StationName = stationName;
            StationAccuHistories = new PaginatedList<StationHistory>(history, pageIndex, pageSize);
        }
        
    }
}