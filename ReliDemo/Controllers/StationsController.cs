using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReliDemo.Models;
using ReliDemo.ViewModels;
using ReliDemo.Core.Interfaces;
using ReliDemo.Infrastructure.Repositories;
using ReliDemo.Infrastructure.Services;
using System.IO;
using OfficeOpenXml;
using ReliDemo.Infrastructure.Helpers;
using System.Configuration;

namespace ReliDemo.Controllers
{
    [Authorize]
    public class StationsController : Controller
    {
        private readonly IStationRepository _stationRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IHeatSourceRepository _heatSourceRepo;
        private readonly IManagershipRepository _managershipRepo;
        private StationsService _stationsService;

        public StationsController(IStationRepository stationRepo, ICompanyRepository companyRepo, IHeatSourceRepository heatSourceRepo, IManagershipRepository managershipRepo)
        {
            _stationRepo = stationRepo;
            _companyRepo = companyRepo;
            _heatSourceRepo = heatSourceRepo;
            _managershipRepo = managershipRepo;
            _stationsService = new StationsService();

        }

        [HttpPost]
        public string GetManagershipsByCompanyId(int companyId)
        {
            var currentUser = MembershipService.CurrentUser;
            var allManagerships = _managershipRepo.GetAllManagerships();
            if (currentUser != null)
            {
                if (currentUser.是否有供热中心权限)
                {
                    allManagerships = allManagerships.Where(i => currentUser.VisibleManagerships.Exists(j => j.Id == i.ItemID));
                }
            }
            var managers = allManagerships.Where(i => i.公司ID == companyId || companyId == -1 ).Select(i => new { i.ItemID, i.管理单位 }).AsEnumerable().Select(i => string.Format("<option value='{0}' >{1}</option>", i.ItemID, i.管理单位));
            return string.Join("",managers);
        }

        public ActionResult Paged(int startIndex = 0, int pageSize = 10, int? area = null, int? heatSources = null, int? companies = null, int? managerships = null , 
            string keyword="", string 数据来源="", string 收费性质 = "", string 是否重点站 = "", string 是否故障站 = "")
        {
            var currentUser = MembershipService.CurrentUser;
            var allStations = _stationRepo.GetAllStations();
            if (currentUser != null)
            {
                if(currentUser.是否有分公司权限) {
                     allStations = allStations.Where(i=> currentUser.VisibleCompanies.Exists(j=>j.Id == i.公司ID));
                }
                else if(currentUser.是否有供热中心权限) {
                    allStations = allStations.Where(i=> currentUser.VisibleManagerships.Exists(j=>j.Id == i.管理单位ID));
                }
            }
            bool? nullable是否重点站 = null;
            if (是否重点站 == "0") 
            {
                nullable是否重点站 = false;
            }
            else if (是否重点站 == "1")
            {
                nullable是否重点站 = true;
            }

            bool? nullable是否故障站 = null;
            if (是否故障站 == "0")
            {
                nullable是否故障站 = false;
            }
            else if (是否故障站 == "1")
            {
                nullable是否故障站 = true;
            }

            var filteredStations = allStations.Where(i => (!heatSources.HasValue || (heatSources.HasValue && i.生产热源ID == heatSources)) &&
                                                          (!companies.HasValue || (companies.HasValue && i.公司ID == companies)) &&
                                                          (!managerships.HasValue || (managerships.HasValue && i.管理单位ID == managerships)) &&
                                                          (!area.HasValue || (i.东西部 == Enum.GetName( typeof(Region), area.Value)) ) &&
                                                          (string.IsNullOrEmpty(数据来源) || (i.数据来源 == 数据来源 || (i.数据来源 == null && 数据来源 == "人工抄表" ) ) ) &&
                                                          (string.IsNullOrEmpty(收费性质) || 收费性质 == i.收费性质) &&
                                                          (!nullable是否重点站.HasValue || nullable是否重点站.Value == i.是否重点站) &&
                                                          (!nullable是否故障站.HasValue || nullable是否故障站.Value == !string.IsNullOrEmpty(i.报警)) &&
                                                          (string.IsNullOrEmpty(keyword) || (!string.IsNullOrEmpty(keyword) &&
                                                          i.热力站名称!= null && i.热力站名称.IndexOf(keyword) >=0 )));
            
            var stationsViewModel = new StationsViewModel(filteredStations, startIndex, pageSize) 
            { 
                Companies = currentUser.VisibleCompanies, 
                HeatSources = _heatSourceRepo.GetAllHeatSources() ,
                SelectedManagershipId = managerships,
                SelectedCompanyId = companies,
                SelectedHeatSourceId = heatSources,
                SelectedRegion = area,
                SearchKeyword=keyword,
                是否重点站 = 是否重点站,
                数据来源 = 数据来源,
                收费性质 = 收费性质,
                是否故障站 = 是否故障站
            };
            ViewBag.Regions = Enum.GetValues(typeof(Region)).Cast<Region>().Select(i => new SelectListItem { Text = i.ToString(), Value = Convert.ToInt32(i).ToString(), Selected=Convert.ToInt32(i) == 2}).ToList();
            return View(stationsViewModel);
        }

        public ActionResult RealTime(int startIndex = 0, int pageSize = 10, int? regions = null, int? selectedHeatSourceId = null, int? selectedCompanyId = null, 
            int? managerships = null, int? selectedArea = null, string searchKeyWord="" , 
            decimal? exceedHeatFrom = null, decimal? exceedHeatTo = null, decimal? exceedWaterFrom = null, decimal? exceedWaterTo = null, decimal? exceedTemperatureFrom = null, decimal? exceedTemperatureTo = null)
        {
            var histories = _stationsService.GetStationsRealTime();

            var currentUser = MembershipService.CurrentUser;
            if (currentUser != null)
            {
                if (currentUser.是否有分公司权限)
                {
                    histories = histories.Where(i => currentUser.VisibleCompanies.Exists(j => j.Id == i.公司ID));
                }
                else if (currentUser.是否有供热中心权限)
                {
                    histories = histories.Where(i => currentUser.VisibleManagerships.Exists(j => j.Id == i.管理单位ID));
                }
            }
            
            histories = histories.Where(i => (!selectedHeatSourceId.HasValue || (selectedHeatSourceId.HasValue && i.生产热源ID == selectedHeatSourceId)) &&
                                                          (!selectedCompanyId.HasValue || (selectedCompanyId.HasValue && i.公司ID == selectedCompanyId)) &&
                                                          (!managerships.HasValue || (managerships.HasValue && i.管理单位ID == managerships)) &&
                                                          (!selectedArea.HasValue || (i.东西部 == Enum.GetName(typeof(Region), selectedArea.Value))) &&
                                                          (string.IsNullOrEmpty(searchKeyWord) || (!string.IsNullOrEmpty(searchKeyWord) && i.热力站名称 != null && i.热力站名称.IndexOf(searchKeyWord) >= 0)) &&
                                                          ((!exceedTemperatureFrom.HasValue) || (i.一次回温 >= exceedTemperatureFrom.Value && (exceedTemperatureTo == null || i.一次回温 <= exceedTemperatureTo.Value))));

            var stationsRealTimeViewModel = new StationsRealTimeViewModel(histories, startIndex, pageSize);
            stationsRealTimeViewModel.Companies = currentUser.VisibleCompanies;
            stationsRealTimeViewModel.HeatSources = _heatSourceRepo.GetAllHeatSources();
            stationsRealTimeViewModel.SelectedManagershipId = managerships;
            stationsRealTimeViewModel.SelectedCompanyId = selectedCompanyId;
            stationsRealTimeViewModel.SelectedHeatSourceId = selectedHeatSourceId;
            stationsRealTimeViewModel.SelectedArea = selectedArea;
            stationsRealTimeViewModel.SearchKeyword = searchKeyWord;
            stationsRealTimeViewModel.UpdatedAt = histories.Count() > 0 ? histories.First().采集时间 : null;
            stationsRealTimeViewModel.单耗超标From = exceedHeatFrom;
            stationsRealTimeViewModel.单耗超标To = exceedHeatTo;
            stationsRealTimeViewModel.流量超标From = exceedWaterFrom;
            stationsRealTimeViewModel.流量超标To = exceedWaterTo;
            stationsRealTimeViewModel.回温超标From = exceedTemperatureFrom;
            stationsRealTimeViewModel.回温超标To = exceedTemperatureTo;
            return View(stationsRealTimeViewModel);
        }

        

        public ActionResult Statistics(string searchSpan="yesterday", string date_customday="", string daterange="",string stat ="", int startIndex = 0, int pageSize = 10,
             string 热源="ALL", int? companies = null, int? managerships = null,
            int? 实际比核算From = null, int? 实际比核算To = null, int? 实际比计划From = null, int? 实际比计划To = null,
            string 收费性质="", int 是否重点站=2, string 数据来源="ALL")
        {
            int selectedSearchMethod = 1;
            DateTime fromdate = DateTime.Today;
            DateTime todate = DateTime.Today.AddDays(1);
            if (searchSpan == "yesterday")
            {
                fromdate = DateTime.Today.AddDays(-1);
                todate = fromdate;
            }
            else if (searchSpan == "last2day")
            {
                fromdate = DateTime.Today.AddDays(-2);
                todate = fromdate;
            }
            else if (searchSpan == "customday")
            {
                if (!string.IsNullOrEmpty(date_customday))
                {
                    fromdate = DateTime.Parse(date_customday);
                    todate = fromdate;
                }
            }
            else if (searchSpan == "customAll")
            {
                if (!string.IsNullOrEmpty(daterange))
                {
                    var fromdate_todate = daterange.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    fromdate = DateTime.Parse(fromdate_todate[0]);
                    todate = DateTime.Parse(fromdate_todate[1]);
                }
            }
            else if (searchSpan == "stat")
            {
                if (!string.IsNullOrEmpty(stat))
                {
                    var fromdate_todate = stat.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    fromdate = DateTime.Parse(fromdate_todate[0]);
                    todate = DateTime.Parse(fromdate_todate[1]);
                }
                selectedSearchMethod = 4;
            }

            if (searchSpan != "stat")
            {
                int total = 0;
                var statistics = _stationsService.GetStationsAccuHistory(fromdate, todate, 实际比核算From, 实际比核算To, 实际比计划From, 实际比计划To, companies, managerships, 热源, 收费性质, 是否重点站, 数据来源, startIndex, pageSize, out total);
                var currentUser = MembershipService.CurrentUser;
                var allHeatSources = new HeatSourcesService().GetDisplyHeatSources();
                var stationsStatisticsTimeViewModel = new StationsStatisticsViewModel(statistics, searchSpan, startIndex, pageSize, total)
                {
                    SearchSpan = searchSpan,
                    SelectedSearchMethod = selectedSearchMethod,
                    FromDate = fromdate,
                    ToDate = todate,
                    SelectedCompanyId = companies,
                    SelectedManagershipId = managerships,
                    SelectedHeatSourceId = null,
                    Companies = currentUser.含可见所有公司,
                    Managerships = companies.HasValue && companies.Value != -1 ? CompanyHelper.GetAllManagershipByCompanyId(companies.Value) : currentUser.含所有可见中心,
                    HeatSources = allHeatSources,
                    实际比核算From = 实际比核算From,
                    实际比核算To = 实际比核算To,
                    实际比计划From = 实际比计划From,
                    实际比计划To = 实际比计划To,
                    收费性质 = 收费性质,
                    是否重点站 = 是否重点站,
                    数据来源 = 数据来源
                };
                return View(stationsStatisticsTimeViewModel);
            }
            else
            {
                int total = 0;
                var statistics = _stationsService.GetStationsStat(fromdate, todate, companies, managerships, 热源, 收费性质, 是否重点站, 数据来源);
                var currentUser = MembershipService.CurrentUser;
                var allHeatSources = new HeatSourcesService().GetDisplyHeatSources();
                var stationsStatisticsTimeViewModel = new StationsStatisticsViewModel(statistics.Skip(startIndex * pageSize).Take(pageSize), searchSpan, startIndex, pageSize, statistics.Count())
                {
                    SearchSpan = searchSpan,
                    SelectedSearchMethod = selectedSearchMethod,
                    FromDate = fromdate,
                    ToDate = todate,
                    SelectedCompanyId = companies,
                    SelectedManagershipId = managerships,
                    SelectedHeatSourceId = null,
                    Companies = currentUser.含可见所有公司,
                    Managerships = companies.HasValue && companies.Value != -1 ? CompanyHelper.GetAllManagershipByCompanyId(companies.Value) : currentUser.含所有可见中心,
                    HeatSources = allHeatSources,
                    实际比核算From = 实际比核算From,
                    实际比核算To = 实际比核算To,
                    实际比计划From = 实际比计划From,
                    实际比计划To = 实际比计划To,
                    收费性质 = 收费性质,
                    是否重点站 = 是否重点站,
                    数据来源 = 数据来源
                };
                return View(stationsStatisticsTimeViewModel);
            }
        }

        public ActionResult DownloadReport(string searchSpan = "yesterday", string date_customday = "", string daterange = "", string stat = "", int startIndex = 0, int pageSize = 10,
             string 热源 = "ALL", int? companies = null, int? managerships = null,
            int? 实际比核算From = null, int? 实际比核算To = null, int? 实际比计划From = null, int? 实际比计划To = null,
            string 收费性质 = "", int 是否重点站 = 2, string 数据来源 = "")
        {
            int selectedSearchMethod = 1;
            DateTime fromdate = DateTime.Today;
            DateTime todate = DateTime.Today.AddDays(1);
            if (searchSpan == "yesterday")
            {
                fromdate = DateTime.Today.AddDays(-1);
                todate = fromdate;
            }
            else if (searchSpan == "last2day")
            {
                fromdate = DateTime.Today.AddDays(-2);
                todate = fromdate;
            }
            else if (searchSpan == "customday")
            {
                if (!string.IsNullOrEmpty(date_customday))
                {
                    fromdate = DateTime.Parse(date_customday);
                    todate = fromdate;
                }
            }
            else if (searchSpan == "customAll")
            {
                if (!string.IsNullOrEmpty(daterange))
                {
                    var fromdate_todate = daterange.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    fromdate = DateTime.Parse(fromdate_todate[0]);
                    todate = DateTime.Parse(fromdate_todate[1]);
                }
            }
            else if (searchSpan == "stat")
            {
                if (!string.IsNullOrEmpty(stat))
                {
                    var fromdate_todate = stat.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    fromdate = DateTime.Parse(fromdate_todate[0]);
                    todate = DateTime.Parse(fromdate_todate[1]);
                }
                selectedSearchMethod = 4;
            }

            if (searchSpan != "stat")
            {
                int total = 0;
                var report = (HeatConsumptionReportBase)ReportFactory.CreateReport(ReportType.热耗统计明细, fromdate);
                var statistics = _stationsService.GetStationsAccuHistory(fromdate, todate, 实际比核算From, 实际比核算To, 实际比计划From, 实际比计划To, companies, managerships, 热源, 收费性质, 是否重点站, 数据来源, 1, -1, out total);
                report.Statistics = statistics;
                var templateName = string.Format("{0}{1}.xlsx", ConfigurationManager.AppSettings["ReportTemplateDirectory"], report.TemplateName);
                FileInfo template = new FileInfo(templateName);
                var newFileName = string.Format("{0}{1:yyyy-MM-dd}.xlsx", report.TemplateName, fromdate);
                FileInfo newFile = new FileInfo(Server.MapPath("~/DownloadedReport/") + newFileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Ti3ned Goods"];
                    report.FillReport(worksheet);
                    xlPackage.Workbook.Properties.Title = report.TemplateName;
                    xlPackage.Save();
                }
                return Json(new { fileName = "/DownloadedReport/" + newFileName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int total = 0;
                var report = ReportFactory.CreateReport(ReportType.热耗统计汇总, fromdate) as HeatConsumptionSummaryReport;
                var statistics = _stationsService.GetStationsStat(fromdate, todate,companies, managerships, 热源, 收费性质, 是否重点站, 数据来源);
                report.Statistics = statistics;
                var templateName = string.Format("{0}{1}.xlsx", ConfigurationManager.AppSettings["ReportTemplateDirectory"], report.TemplateName);
                FileInfo template = new FileInfo(templateName);
                var newFileName = string.Format("{0}{1:yyyy-MM-dd}-{2:yyyy-MM-dd}.xlsx", report.TemplateName, fromdate, todate);
                FileInfo newFile = new FileInfo(Server.MapPath("~/DownloadedReport/") + newFileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Ti3ned Goods"];
                    report.FillReport(worksheet);
                    xlPackage.Workbook.Properties.Title = report.TemplateName;
                    xlPackage.Save();
                }
                return Json(new { fileName = "/DownloadedReport/" + newFileName }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult BasicInfo(int stationId = 0)
        {
            var currentUser = MembershipService.CurrentUser;

            var stations = _stationRepo.GetAllStations().Where(i=>i.公司ID.HasValue && currentUser.VisibleCompanies.Count(j=>j.Id == i.公司ID.Value)>0).Where(i=>i.ItemID == stationId);
            if (stations.Count() == 0)
            {
                return RedirectToAction("Paged");
            }
            return View(stations.First());
        }

        public ActionResult StationRealTime(int stationId = 0)
        {
            var currentUser = MembershipService.CurrentUser;

            var stations = _stationRepo.GetAllStations().Where(i => i.公司ID.HasValue && currentUser.VisibleCompanies.Count(j => j.Id == i.公司ID.Value) > 0).Where(i => i.ItemID == stationId);
            if (stations.Count() == 0)
            {
                return RedirectToAction("Paged");
            }

            Station station = stations.First();
            var service = new HeatConsumptionSummaryService();
            var weatherService = new WeatherService();
            var histories =  service.GetHistoriesByStation(stationId, DateTime.Today.AddDays(-7), DateTime.Today);
            var forecasts = weatherService.GetForecast(DateTime.Today.AddDays(-7), DateTime.Today);
            var actual = weatherService.GetActual(DateTime.Today.AddDays(-7), DateTime.Today);
            
            return View(new StationRealTimeViewModel()
            {
                StationId = station.ItemID,
                StationName = station.热力站名称,
                Histories = new GJHistoriesViewModel()
                {
                    FromDate = DateTime.Today.AddDays(-7),
                    ToDate = DateTime.Today,
                    实际Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.采暖GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    核算Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.核算GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    计划Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.计划GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    预报温度 = "[" + string.Join(",", forecasts.Where(i=>i.Temperature.HasValue).Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    实际温度 = "[" + string.Join(",", actual.Where(i => i.Temperature.HasValue).Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    计划热指标 = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.计划热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    核算热指标 = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.核算热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    实际运行热指标 = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.实际运行热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    类型 = Models.HeatConsumptionGraphType.Station,
                    StationId = stationId 
                },
                TodayAndYesterdayGJ = service.GetTodayAndYesterdaysGJByStation(stationId),
                Station2ndRecents = station.Station2ndRecents,
                一次供压 = station.一次供压,
                一次回压 = station.一次回压,
                一次供温 = station.一次供温,
                一次回温 = station.一次回温,
                总累计流量 = station.总累计流量,
                总累计热量 = station.总累计热量,
                数据来源 = station.数据来源,
                当前热指标 = station.参考热指标
            });
        }

        public ActionResult ChangeCalculatedHeatIndex(int stationId, decimal newHeatIndex)
        {
            var station = _stationRepo.GetAllStations().Single(i => i.ItemID == stationId);
            station.参考热指标 = newHeatIndex;
            _stationRepo.Update(station);
            _stationsService.InsertToHeatIndexAudit(stationId, newHeatIndex,MembershipService.CurrentUser.UserId);
            return RedirectToAction("StationRealTime", new { stationId = stationId });
        }

        public ActionResult History(int stationId = 0, int pageIndex = 0, int pageSize = 10, string daterange = "")
        {
            
            var currentUser = MembershipService.CurrentUser;
            var stations = _stationRepo.GetAllStations().Where(s => s.ItemID == stationId && s.公司ID.HasValue && currentUser.VisibleCompanies.AsEnumerable().Count(j => j.Id == s.公司ID.Value) > 0);
            if (stations.Count() == 0)
            {
                return RedirectToAction("Paged");
            }
            var station = stations.First();
            var fromDate = DateTime.Today.AddDays(-7);
            var toDate = DateTime.Today;
            if (!string.IsNullOrEmpty(daterange))
            {
                var fromAndToDate = daterange.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                fromDate = DateTime.Parse(fromAndToDate[0]);
                toDate = DateTime.Parse(fromAndToDate[1]);
            }
            var accus = new StationAccuHistoryRepository().SearchFor(i => i.热力站ID == stationId && i.日期 >= fromDate && i.日期 <= toDate).AsEnumerable().Select(i=>new StationHistory(i)).ToList();
            var historiesViewModel = new StationHistoriesViewModel(stationId, station.热力站名称, pageIndex, pageSize, accus) { FromDate = fromDate, ToDate = toDate, 数据来源 = station.数据来源 };
            return View(historiesViewModel);
        }

        public ActionResult HistoryData(int stationId = 0, string searchSpan = "", string daterange = "")
        {
            var weatherService = new WeatherService();
            
            var fromDate = DateTime.Today.AddDays(-7);
            var toDate = DateTime.Today;
            if (searchSpan == "week")
            {
                fromDate = DateTime.Today.AddDays(-7);
                toDate = DateTime.Today;
            }
            else if (searchSpan == "month")
            {
                fromDate = DateTime.Today.AddMonths(-1);
                toDate = DateTime.Today;
            }
            else if (searchSpan == "season")
            {
                fromDate = (new ConfigurationService()).getStartHeatSupplyDate();
                toDate = DateTime.Today;
            }
            else if (searchSpan == "customAll")
            {
                var fromdate_todate = daterange.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                fromDate = DateTime.Parse(fromdate_todate[0]);
                toDate = DateTime.Parse(fromdate_todate[1]);
            }
            else
            {
                fromDate = DateTime.Today.AddDays(-7);
                toDate = DateTime.Today;
                searchSpan = "week";
            }
            var forecasts = weatherService.GetForecast(fromDate, toDate);
            var actuals = weatherService.GetActual(fromDate, toDate);
            var v = from forecast in forecasts
                    join actual in actuals on forecast.时间 equals actual.时间
                    select new
                    {
                        预报温度 = forecast.Temperature,
                        实际温度 = actual.Temperature,
                        时间 = forecast.时间
                    };
            var service = new HeatConsumptionSummaryService();
            var histories = service.GetHistoriesByStation(stationId, fromDate, toDate);
            var planned = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.计划热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            var actualUnit = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.实际运行热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            var calculatedUnit = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.核算热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            var planned_gj = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.计划GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            var calculated_gj = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.核算GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            var actual_gj = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.采暖GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            var tempForecast = "[" + string.Join(",", v.Where(i => i.预报温度.HasValue).Select(i => string.Format("[{1},{0}]", i.预报温度, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            var tempActual = "[" + string.Join(",", v.Where(i => i.实际温度.HasValue).Select(i => string.Format("[{1},{0}]", i.实际温度, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";

            return Json(new { planned = planned, actual = actualUnit, calculated = calculatedUnit, tempForecast = tempForecast, 
                tempActual = tempActual, planned_gj = planned_gj, actual_gj = actual_gj, calculated_gj = calculated_gj,
                              fromDate = fromDate.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds,
                              toDate = toDate.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Graph(int stationId = 0, string strFromDate = "", string strToDate = "", string searchSpan = "", string daterange="")
        {
            var weatherService = new WeatherService();
            var forecasts = weatherService.GetForecast(DateTime.Today.AddDays(-7), DateTime.Today);
            var actual = weatherService.GetActual(DateTime.Today.AddDays(-7), DateTime.Today);
            var fromDate = DateTime.Today.AddDays(-7);
            var toDate = DateTime.Today;
            if (searchSpan == "week")
            {
                fromDate = DateTime.Today.AddDays(-7);
                toDate = DateTime.Today;
            }
            else if (searchSpan == "month")
            {
                fromDate = DateTime.Today.AddMonths(-1);
                toDate = DateTime.Today;
            }
            else if (searchSpan == "season")
            {
                fromDate = (new ConfigurationService()).getStartHeatSupplyDate();
                toDate = DateTime.Today;
            }
            else if (searchSpan == "customAll")
            {
                var fromdate_todate = daterange.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                fromDate = DateTime.Parse(fromdate_todate[0]);
                toDate = DateTime.Parse(fromdate_todate[1]);
            }
            else
            {
                fromDate = DateTime.Today.AddDays(-7);
                toDate = DateTime.Today;
                searchSpan = "week";
            }

            var histories = new StationAccuHistoryRepository().SearchFor(i => i.热力站ID == stationId && i.日期 > fromDate && i.日期 < toDate).AsEnumerable().Select(i=>new StationHistory(i)).ToList();
            var station = _stationRepo.GetAllStations().Single(s => s.ItemID == stationId);

            var service = new HeatConsumptionSummaryService();
            var hs = service.GetHistoriesByStation(stationId, fromDate, toDate);

            var historiesViewModel = new StationHistoriesViewModel(stationId, station.热力站名称, 0, 7,  histories);
            historiesViewModel.FromDate = DateTime.Today.AddDays(-7);
            historiesViewModel.ToDate = DateTime.Today;
            historiesViewModel.计划热指标 = "[" + string.Join(",", hs.Select((i, j) => string.Format("[{1},{0}]", i.计划热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            historiesViewModel.实际运行热指标 = "[" + string.Join(",", hs.Select((i, j) => string.Format("[{1},{0}]", i.实际运行热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            historiesViewModel.核算热指标 = "[" + string.Join(",", hs.Select((i, j) => string.Format("[{1},{0}]", i.核算热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            historiesViewModel.计划供热量GJ = "[" + string.Join(",", hs.Select((i, j) => string.Format("[{1},{0}]", i.计划GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            historiesViewModel.实际供热量GJ = "[" + string.Join(",", hs.Select((i, j) => string.Format("[{1},{0}]", i.采暖GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            historiesViewModel.核算供热量GJ = "[" + string.Join(",", hs.Select((i, j) => string.Format("[{1},{0}]", i.核算GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            historiesViewModel.预报温度 = "[" + string.Join(",", forecasts.Where(i=>i.Temperature.HasValue).Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            historiesViewModel.实际温度 = "[" + string.Join(",", actual.Where(i => i.Temperature.HasValue).Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]";
            historiesViewModel.SearchSpan = searchSpan;
            historiesViewModel.数据来源 = station.数据来源;
            return View(historiesViewModel);
        }

        //
        // GET: /Stations/

        public ActionResult Customers(int stationId = 0)
        {
            var customers = new CustomerRepository().SearchFor(i => i.热力站ID == stationId);
            var station = _stationRepo.GetAllStations().Single(i => i.ItemID == stationId);
            return View(new StationCustomersViewModel(customers, stationId, station.热力站名称, station.数据来源));
        }

        public ActionResult StationTemperature(int stationId)
        {
            var station = _stationRepo.GetAllStations().Single(i => i.ItemID == stationId);
            var stats = _stationsService.GetTemperatureStats(station.热力站名称);
            return View(new StationRoomTemperatureViewModel()
            {
                RoomTemperatures = _stationsService.GetRoomTemperature().Where(i => i.热力站名称 == station.热力站名称),
                StationId = stationId,
                StationName = station.热力站名称,
                数据来源 = station.数据来源,
                Stats = _stationsService.GetTemperatureStats(station.热力站名称)
            });
        }

        public ActionResult Index( int startIndex = 0, int pageSize=10)
        {
            return View(_stationRepo.GetAllStations().Skip(startIndex).Take(pageSize));
        }

        public ActionResult ManualDataInput(string code = "", string stationName = "", string errorDescription = "", string daterange = "", int pageIndex = 0, int pageSize= 10)
        {
            var manualDataInputViewModel = new ManualDataInputViewModel();
            var fromDate = DateTime.Today.AddDays(-7);
            var toDate = DateTime.Today;
            if (!string.IsNullOrEmpty(daterange))
            {
                var fromtodate = daterange.Split(new string[] {"-"}, StringSplitOptions.RemoveEmptyEntries );
                fromDate = DateTime.Parse(fromtodate[0]);
                toDate = DateTime.Parse(fromtodate[1]);
            }
            var manualInputs = _stationRepo.GetManualInput()
                .Where(i=> (string.IsNullOrEmpty(code) || i.生产编号 == code) &&
                           (string.IsNullOrEmpty(stationName) || i.站名 == stationName) &&
                           (string.IsNullOrEmpty(errorDescription) || i.故障.IndexOf(errorDescription) >= 0) &&
                           i.采集时间 >= fromDate && i.采集时间 <= toDate).OrderByDescending(i=>i.采集时间);
            manualDataInputViewModel.Stations = _stationRepo.GetAllStations();
            var pagedInputs = new PaginatedList<StationManualInput>(manualInputs, pageIndex, pageSize);
            manualDataInputViewModel.StationManualInput = pagedInputs;
            manualDataInputViewModel.FromDate = fromDate;
            manualDataInputViewModel.ToDate = toDate;
            return View(manualDataInputViewModel);
        }

        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult UploadManualDataInput(HttpPostedFileBase file)
        {

            int 生产编号ColId = 1, 站名ColId = 2, 采集时间ColId = 3, 供压ColId = 4, 回压ColId = 5, 供温ColId = 6, 回温ColId = 7, 瞬流ColId = 8, 瞬热ColId = 9, 累计流量ColId = 10,
                流量差量ColId = 11, 累计热量ColId = 12, 热量差量ColId = 13, 热指标ColId = 14, 站内面积ColId=15, 部门ColId = 16, 管理方式ColId = 17, 管理单位ColId = 18, 有无生活水ColId = 19, 故障ColId = 20;
            var dateFrom = DateTime.Now.AddYears(1);
            var dateTo = DateTime.Now.AddYears(-1);
            if (file != null && file.ContentLength > 0)
            {
                using (var package = new ExcelPackage(file.InputStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    for (int col = 1; worksheet.Cells[1, col].Value != null; col++)
                    {
                        var colVal = worksheet.Cells[1, col].Value.ToString();
                        switch (colVal)
                        {
                            case "生产编号":
                            {
                                生产编号ColId = col;
                                break;
                            }
                            case "站名":
                            {
                                站名ColId = col;
                                break;
                            }
                            case "采集时间":
                            {
                                采集时间ColId = col;
                                break;
                            }
                            case "供压":
                            {
                                供压ColId = col;
                                break;
                            }
                            case "回压":
                            {
                                回压ColId = col;
                                break;
                            }
                            case "供温":
                            {
                                供温ColId = col;
                                break;
                            }
                            case "回温" :
                            {
                                回温ColId = col;
                                break;
                            }
                            case "瞬流":
                            {
                                瞬流ColId = col;
                                break;
                            }
                            case "瞬热" :
                            {
                                瞬热ColId=col;
                                break;
                            }
                            case "累计流量":
                            {
                                累计流量ColId = col;
                                break;
                            }
                            case "流量差量" :
                            {
                                流量差量ColId = col;
                                break;
                            }
                            case "累计热量" :
                            {
                                累计热量ColId = col;
                                break;
                            }
                            case "热量差量" :
                            {
                                热量差量ColId = col;
                                break;
                            }
                            case "部门":
                            {
                                部门ColId = col;
                                break;
                            }
                            case "故障" :
                            {
                                故障ColId = col;
                                break;
                            }
                            case " 管理方式":
                            {
                                管理方式ColId = col;
                                break;
                            }
                            case "管理单位":
                            {
                                管理单位ColId = col;
                                break;
                            }
                            case "有无生活水":
                            {
                                有无生活水ColId = col;
                                break;
                            }
                            case "热指标":
                            {
                                热指标ColId = col;
                                break;
                            }
                            case "站内面积":
                            {
                                站内面积ColId = col;
                                break;
                            }
                        }
                    }
                    
                    
                    for (int row = 2; worksheet.Cells[row,1].Value != null; row++)
                    {
                        var 编号 = worksheet.Cells[row, 生产编号ColId].Value.ToString();
                        var station = _stationRepo.SearchFor(i => i.热力站编号 == 编号);
                        var stationId = 0;
                        if (station.Count() > 0)
                        {
                            stationId = station.First().StationId;
                        }
                        var dateInput = Convert.ToDateTime(worksheet.Cells[row, 采集时间ColId].Value);
                        if(dateInput < dateFrom) {
                            dateFrom = dateInput;
                        }
                        if(dateInput> dateTo) {
                            dateTo = dateInput;
                        }
                        var manualInput = new StationManualInput()
                        {
                            供压 = Convert.ToDecimal(worksheet.Cells[row, 供压ColId].Value),
                            回压 = Convert.ToDecimal(worksheet.Cells[row, 回压ColId].Value),
                            供温 = Convert.ToDecimal(worksheet.Cells[row, 供温ColId].Value),
                            回温 = Convert.ToDecimal(worksheet.Cells[row, 回温ColId].Value),
                            故障 = worksheet.Cells[row, 故障ColId].Value != null ? worksheet.Cells[row, 故障ColId].Value.ToString() :"",
                            流量差量 = Convert.ToDecimal(worksheet.Cells[row, 流量差量ColId].Value),
                            热量差量 = Convert.ToDecimal(worksheet.Cells[row, 热量差量ColId].Value),
                            瞬流 = Convert.ToDecimal(worksheet.Cells[row, 瞬流ColId].Value),
                            瞬热 = Convert.ToDecimal(worksheet.Cells[row, 瞬热ColId].Value),
                            部门 = worksheet.Cells[row, 部门ColId].Value.ToString(),
                            累计流量 = Convert.ToDecimal(worksheet.Cells[row, 累计流量ColId].Value),
                            累计热量 = Convert.ToDecimal(worksheet.Cells[row, 累计热量ColId].Value),
                            采集时间 = Convert.ToDateTime(worksheet.Cells[row, 采集时间ColId].Value),
                            管理单位 = worksheet.Cells[row, 管理单位ColId].Value.ToString(),
                            管理方式 = worksheet.Cells[row, 管理方式ColId].Value.ToString(),
                            有无生活水 = worksheet.Cells[row, 有无生活水ColId].Value.ToString(),
                            站内面积 = Convert.ToDecimal(worksheet.Cells[row, 站内面积ColId].Value),
                            热指标 = Convert.ToDecimal(worksheet.Cells[row, 热指标ColId].Value),
                            生产编号 = 编号,
                            站名 = worksheet.Cells[row, 站名ColId].Value.ToString()
                        };
                        _stationsService.InsertManualInputData(manualInput);               
                    }
                } // the using 

            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("ManualDataInput", new {daterange= string.Format("{0:yyyy年MM月dd日}-{1:yyyy年MM月dd日}",dateFrom, dateTo) });
        }


        public ActionResult GetNewDataCount(DateTime fromTime)
        {
            var newData = _stationsService.GetStationsRealTime(fromTime, DateTime.Now);
            return Json(new { newDataCount = newData.Count() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUnitHeatConsumption(int stationId, string dateRange)
        {
            var fromdate_todate = dateRange.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            var fromDate = DateTime.Parse(fromdate_todate[0]);
            var toDate = DateTime.Parse(fromdate_todate[1]);
            var histories = _stationsService.GetStationsAccuHistory(stationId, fromDate, toDate);
            return Json(new { GJ = histories.Sum(i => i.日单耗) }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RoomTemperature(int? uploadedRecords, DateTime? 测温日期, string fileName = "")
        {
            var roomTemperatureViewModel = new RoomTemperatureViewModel();
            if (uploadedRecords.HasValue && 测温日期.HasValue)
            {
                roomTemperatureViewModel.HasNewData = true;
                roomTemperatureViewModel.LastUploadedCount = uploadedRecords.Value;
                roomTemperatureViewModel.LastUploadedTime = 测温日期.Value;
                
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                roomTemperatureViewModel.RoomTemperatures = _stationsService.GetRoomTemperature().Where(i => i.导入文件名 == fileName);
            }
            else
            {
                roomTemperatureViewModel.RoomTemperatures = new List<RoomTemperature>();
            }
            roomTemperatureViewModel.fileNames = _stationsService.GetTemperatureImportFileName();
            return View(roomTemperatureViewModel);
        }

        [HttpPost]
        public ActionResult UploadRoomTemperature(HttpPostedFileBase file)
        {
            int 序号ColId = 2,
                测温日期ColId = 1,
                热力站名称ColId = 3,
                楼号ColId = 4,
                层次ColId = 5,
                朝向ColId = 6,
                房间号ColId = 7,
                暖气状况ColId = 8,
                建筑年代ColId = 9,
                节能建筑ColId = 10,
                开窗ColId = 11,
                室内温度ColId = 12,
                测试户姓名ColId = 13,
                联系电话ColId = 14,
                所属热源ColId = 15,
                测温单位ColId = 16;


            var inserted = new List<RoomTemperature>();

            if (file != null && file.ContentLength > 0)
            {
                using (var package = new ExcelPackage(file.InputStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    for (int col = 1; worksheet.Cells[1, col].Value != null; col++)
                    {
                        var colVal = worksheet.Cells[1, col].Value.ToString();
                        switch (colVal)
                        {
                            case "序号":
                                {
                                    序号ColId = col;
                                    break;
                                }
                            case "测温日期":
                                {
                                    测温日期ColId = col;
                                    break;
                                }
                            case "热力站名称":
                                {
                                    热力站名称ColId = col;
                                    break;
                                }
                            case "楼号":
                                {
                                    楼号ColId = col;
                                    break;
                                }
                            case "层次":
                                {
                                    层次ColId = col;
                                    break;
                                }
                            case "朝向":
                                {
                                    朝向ColId = col;
                                    break;
                                }
                            case "房间号":
                                {
                                    房间号ColId = col;
                                    break;
                                }
                            case "暖气状况":
                                {
                                    暖气状况ColId = col;
                                    break;
                                }
                            case "建筑年代":
                                {
                                    建筑年代ColId = col;
                                    break;
                                }
                            case "节能建筑":
                                {
                                    节能建筑ColId = col;
                                    break;
                                }
                            case "开窗":
                                {
                                    开窗ColId = col;
                                    break;
                                }
                            case "室温":
                                {
                                    室内温度ColId = col;
                                    break;
                                }
                            case "测试户姓名":
                                {
                                    测试户姓名ColId = col;
                                    break;
                                }
                            case "联系电话":
                                {
                                    联系电话ColId = col;
                                    break;
                                }
                            case "所属热源":
                                {
                                    所属热源ColId = col;
                                    break;
                                }
                            case "测温单位":
                                {
                                    测温单位ColId = col;
                                    break;
                                }
                        }
                    }


                    for (int row = 1; worksheet.Cells[row, 1].Value != null; row++)
                    {
                        var roomTemperaturess = new RoomTemperature();
                        var roomTemperature = new RoomTemperature()
                        {
                            测温日期 = DateTime.Parse( worksheet.Cells[row, 测温日期ColId].Value.ToString()),
                            热力站名称 = Convert.ToString(worksheet.Cells[row, 热力站名称ColId].Value).Trim(),
                            楼号 = Convert.ToString(worksheet.Cells[row, 楼号ColId].Value).Trim(),
                            层次 = Convert.ToString(worksheet.Cells[row, 层次ColId].Value).Trim(),
                            朝向 = Convert.ToString(worksheet.Cells[row, 朝向ColId].Value).Trim(),
                            房间号 = Convert.ToString(worksheet.Cells[row, 房间号ColId].Value).Trim(),
                            暖气状况 = Convert.ToString(worksheet.Cells[row, 暖气状况ColId].Value).Trim(),
                            建筑年代 = Convert.ToString(worksheet.Cells[row, 建筑年代ColId].Value).Trim(),
                            节能建筑 = Convert.ToString(worksheet.Cells[row, 节能建筑ColId].Value).Trim(),
                            开窗 = Convert.ToString(worksheet.Cells[row, 开窗ColId].Value).Trim(),
                            室内温度 = Math.Max(Math.Min( Convert.ToDecimal(worksheet.Cells[row, 室内温度ColId].Value), 50.0m), 0.0m),
                            测试户姓名 = Convert.ToString( worksheet.Cells[row, 测试户姓名ColId].Value).Trim(),
                            联系电话 = Convert.ToString(worksheet.Cells[row, 联系电话ColId].Value).Trim(),
                            所属热源 = Convert.ToString(worksheet.Cells[row, 所属热源ColId].Value).Trim(),
                            序号 = Convert.ToInt32(worksheet.Cells[row, 序号ColId].Value),
                            导入文件名 = file.FileName
                        };
                        if (_stationsService.InsertRoomTemperature(roomTemperature))
                        {
                            inserted.Add(roomTemperature); 
                        }
                        
                    }
                } // the using 

            }
            // redirect back to the index action to show the form once again
            return View("RoomTemperature", new RoomTemperatureViewModel() { HasNewData = inserted.Count > 0, LastUploadedCount = inserted.Count, LastUploadedTime = DateTime.Now, RoomTemperatures = inserted , fileNames =  _stationsService.GetTemperatureImportFileName()} );
        }


        public ActionResult HeatIndexHistory(int stationId)
        {
            return View(
                new HeatIndexHistoryViewModel()
                {
                    StationId = stationId,
                    HeatIndexes = new StationsService().GetHeatIndexAudits(stationId)
                }
            );
        }
    }
}