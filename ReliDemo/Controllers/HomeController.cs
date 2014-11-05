using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReliDemo.Infrastructure.Services;
using ReliDemo.ViewModels;
using ReliDemo.Core.Interfaces;
using ReliDemo.Models;

namespace ReliDemo.Controllers
{
    [Authorize(Roles = "生产部调度,系统管理员,系统维护员")]
    public class HomeController : Controller
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly IStationRepository _stationRepo;
        public HomeController( ICompanyRepository companyRepo, IStationRepository stationRepo)
        {
            _companyRepo = companyRepo;
            _stationRepo = stationRepo;
        }
        

        public ActionResult Index()
        {
            var service = new HeatConsumptionSummaryService();
            var weatherService = new WeatherService();
            var configurationService = new ConfigurationService();
            var histories = service.GetHistories( DateTime.Today.AddDays(-7), DateTime.Today);
            var forecasts = weatherService.GetForecast(DateTime.Today.AddDays(-7), DateTime.Today);
            var actual = weatherService.GetActual(DateTime.Today.AddDays(-7), DateTime.Today);
            var companies = _companyRepo.GetAllCompanies();
            var summary = service.GetTodaysHeatConsumptionSummary();
            var todaysGJ =  service.GetTodaysGJ();
            var yesterdaysGJ = service.GetYesterdaysGJ();
            var stations = _stationRepo.GetAllStations();
            var topStats = new TopStatsViewModel()
            {
                TodaysHighestTemperature = weatherService.GetToday().今日预报白天最高温.Value,
                TodaysLowestTemperature = weatherService.GetToday().今日预报夜间最低温.Value,
                TodaysAverageTemperature = weatherService.GetToday().今日预报一天平均温.Value,
                TodaysWeather = weatherService.GetToday().白天天气,
                TodaysWind = weatherService.GetToday().白天风力,
                WeatherType = weatherService.GetWeatherTypesByText(weatherService.GetToday().白天天气),
                YesterdayAverateTemperature = weatherService.GetToday().昨日实况一天平均温.Value,
                WeatherPublishedAt = weatherService.GetLatestPublishedAt(),
                投停面积信息 = summary,
                TotalHeatSupplyDays = configurationService.getTotalHeatSupplyDays(),
                起始供热时间 = configurationService.getStartHeatSupplyDate(),
                当前时间 = DateTime.Now,
                当前用户 = MembershipService.CurrentUser,
                今日累计供热量 = service.GetHeatConsumptionAccuByDate(Region.全网, DateTime.Today),
                昨日累计供热量 = service.GetHeatConsumptionAccuByDate(Region.全网, DateTime.Today.AddDays(-1)),
                CompanyId = -1,
                ManagershipId = -1
            };
            var previousNextForecast = weatherService.GetNextDays(7).ToList();
            previousNextForecast.AddRange(weatherService.GetPreviousDays(2));
            previousNextForecast.Add(weatherService.GetToday());
            previousNextForecast = previousNextForecast.OrderBy(i => i.日期).ToList();
            var heatConsumptionSummaryViewModel = new HeatConsumptionSummaryViewModel()
            {
                YesterdayGJ = yesterdaysGJ,
                TodayGJ =todaysGJ,
                TopStats = topStats,
                Histories = new GJHistoriesViewModel
                {
                    FromDate = DateTime.Today.AddDays(-7),
                    ToDate = DateTime.Today,
                    实际Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.采暖GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    核算Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.核算GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    计划Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.计划GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    计划热指标 = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.计划热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    核算热指标 = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.核算热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    实际运行热指标 = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.实际运行热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    预报温度 = "[" + string.Join(",", forecasts.Where(i=>i.Temperature.HasValue).Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    实际温度 = "[" + string.Join(",", actual.Where(i => i.Temperature.HasValue).Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    类型 = Models.HeatConsumptionGraphType.Total
                },
                CompanyAreaOnOff = new CompanyAreaOnOffViewModel
                {
                    CompanyAreaTotalData = "[" + string.Join(",", companies.Select(i => string.Format("[\"{0}\", {1}]", i.公司, i.总面积)).ToArray()) + "]",
                    CompanyAreaOnData = "[" + string.Join(",", companies.Select(i => string.Format("[\"{0}\", {1}]", i.公司, i.实际供热面积)).ToArray()) + "]"
                },
                Companies = _companyRepo.GetAllCompanies(),
                智能卡站个数 = stations.Count(i => string.Compare(i.数据来源, "智能卡") == 0),
                监控站个数 = stations.Count(i=>string.Compare(i.数据来源, "监控") == 0 ),
                手抄表站个数 = stations.Count(i => string.IsNullOrEmpty(i.数据来源)),
                手抄表最近更新时间 = stations.Where(i => string.IsNullOrEmpty(i.数据来源)).OrderByDescending(i => i.采集时间).FirstOrDefault().采集时间,
                WeatherGraph = previousNextForecast,
                有效站个数 = Convert.ToInt32(companies.Sum(i=>i.有效监控站数))
            };
            return View(heatConsumptionSummaryViewModel);
        }

    }
}
