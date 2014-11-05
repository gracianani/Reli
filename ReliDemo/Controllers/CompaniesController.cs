using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ReliDemo.Infrastructure.Services;
using ReliDemo.ViewModels;
using ReliDemo.Core.Interfaces;
using ReliDemo.Models;

namespace ReliDemo.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private readonly IStationRepository _stationRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IHeatSourceRepository _heatSourceRepo;
        private readonly IManagershipRepository _managershipRepo;
        
        public CompaniesController(IStationRepository stationRepo, ICompanyRepository companyRepo, IHeatSourceRepository heatSourceRepo, IManagershipRepository managershipRepo)
        {
            _stationRepo = stationRepo;
            _companyRepo = companyRepo;
            _heatSourceRepo = heatSourceRepo;
            _managershipRepo = managershipRepo;
        }

        [Authorize(Roles = "生产部调度,分公司调度,系统管理员,系统维护员")]
        public  ActionResult Index(int companyId)
        {
            var service = new HeatConsumptionSummaryService();
            var weatherService = new WeatherService();
            var configurationService = new ConfigurationService();
            var stations = _stationRepo.SearchFor(i=>i.公司ID == companyId);
            var histories = service.GetHistoriesByCompany(companyId, DateTime.Today.AddDays(-7), DateTime.Today);
            var forecasts = weatherService.GetForecast(DateTime.Today.AddDays(-7), DateTime.Today);
            var actual = weatherService.GetActual(DateTime.Today.AddDays(-7), DateTime.Today);

            var area = service.GetTodaysHeatConsumptionSummaryByCompany(companyId);
            var company = _companyRepo.Find(companyId);
            
            var previousNextForecast = weatherService.GetNextDays(7).ToList();
            previousNextForecast.AddRange(weatherService.GetPreviousDays(2));
            previousNextForecast.Add(weatherService.GetToday());
            previousNextForecast = previousNextForecast.OrderBy(i => i.日期).ToList();
            var topStats = new TopStatsViewModel()
            {
                TodaysHighestTemperature = weatherService.GetToday().今日预报白天最高温.Value,
                TodaysLowestTemperature = weatherService.GetToday().今日预报夜间最低温.Value,
                TodaysAverageTemperature = weatherService.GetToday().今日预报一天平均温.Value,
                TodaysWeather = weatherService.GetToday().白天天气,
                TodaysWind = weatherService.GetToday().白天风力,
                YesterdayAverateTemperature = weatherService.GetToday().昨日实况一天平均温.Value,
                WeatherType = weatherService.GetWeatherTypesByText(weatherService.GetToday().白天天气),
                WeatherPublishedAt = weatherService.GetLatestPublishedAt(),
                投停面积信息 = area,
                当前时间 = DateTime.Now,
                当前用户 = MembershipService.CurrentUser,
                TotalHeatSupplyDays = configurationService.getTotalHeatSupplyDays(),
                起始供热时间 = configurationService.getStartHeatSupplyDate(),
                今日累计供热量 = Convert.ToInt32(company.今日累计GJ),
                昨日累计供热量 = Convert.ToInt32(company.昨日累计GJ),
                CompanyId=companyId,
                ManagershipId = -1
            };

            var companyViewModel = new CompanyViewModel()
            {
                Company = company,
                Centers = _managershipRepo.SearchFor(i => i.公司ID == companyId),
                TopStats = topStats,
                YesterdayGJ = service.GetYesterdaysGJByCompany(companyId),
                TodayGJ = service.GetTodaysGJByCompanyId(companyId),
                手抄GJ = service.GetManualGJByCompanyId(companyId),
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
                    预报温度 = "[" + string.Join(",", forecasts.Where(i => i.Temperature.HasValue).Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    实际温度 = "[" + string.Join(",", actual.Where(i => i.Temperature.HasValue).Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    类型 = Models.HeatConsumptionGraphType.Company,
                    CompanyId = companyId
                }, 
                手抄表站个数 = company.Stations.Count(i=>string.IsNullOrEmpty(i.数据来源)),
                智能卡站个数 = company.Stations.Count(i => string.Compare(i.数据来源, "智能卡") == 0),
                监控站个数 = company.Stations.Count(i => string.Compare(i.数据来源, "监控") == 0),
                手抄表最近更新时间 = company.Stations.Where(i => string.IsNullOrEmpty(i.数据来源)).OrderByDescending(i => i.采集时间).FirstOrDefault() != null ? company.Stations.Where(i => string.IsNullOrEmpty(i.数据来源)).OrderByDescending(i => i.采集时间).FirstOrDefault().采集时间 : null,
                WeatherGraph = previousNextForecast,
                GJ总 = service.GetTodaysGJ总ByCompanyId(companyId)
            };
            return View(companyViewModel);
        }

        [Authorize(Roles = "生产部调度,分公司调度,供热中心调度,系统管理员,系统维护员")]
        public ActionResult ManagershipInfo(int managershipId)
        {
            var service = new HeatConsumptionSummaryService();
            var weatherService = new WeatherService();
            var configurationService = new ConfigurationService();

            var managership = _managershipRepo.Find(managershipId);
            var histories = service.GetHistoriesByManagership(managershipId, DateTime.Today.AddDays(-7), DateTime.Today);
            var d = DateTime.Today.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

            var forecasts = weatherService.GetForecast(DateTime.Today.AddDays(-7), DateTime.Today);
            var actual = weatherService.GetActual(DateTime.Today.AddDays(-7), DateTime.Today);

            var area = service.GetTodaysHeatConsumptionSummaryByManagership(managershipId);
            var previousNextForecast = weatherService.GetNextDays(7).ToList();
            previousNextForecast.AddRange(weatherService.GetPreviousDays(2));
            previousNextForecast.Add(weatherService.GetToday());
            previousNextForecast = previousNextForecast.OrderBy(i => i.日期).ToList();
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
                投停面积信息 = area,
                TotalHeatSupplyDays = configurationService.getTotalHeatSupplyDays(),
                起始供热时间 = configurationService.getStartHeatSupplyDate(),
                今日累计供热量 = Convert.ToInt32(managership.今日累计GJ),
                昨日累计供热量 = Convert.ToInt32(managership.昨日累计GJ),
                当前时间 = DateTime.Now,
                当前用户 = MembershipService.CurrentUser,
                ManagershipId = managershipId ,
                CompanyId = -1
            };

            var managershipViewModel = new ManagershipViewModel()
            {
                Managership = managership ,
                TodayGJ = service.GetTodaysGJByManagershipId(managershipId),
                YesterdayGJ = service.GetYesterdaysGJByManagership(managershipId),
                手抄GJ = service.GetManualGJByManagershipId(managershipId),
                TopStats = topStats,
                Histories = new GJHistoriesViewModel
                {
                    FromDate = DateTime.Today.AddDays(-7),
                    ToDate = DateTime.Today,
                    实际Data = "[" + string.Join(",", histories.Select((i,j) => string.Format("[{1},{0}]", i.采暖GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970,1,1)).TotalMilliseconds)).ToArray()) + "]",
                    核算Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.核算GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    计划Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.计划GJ ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    计划热指标 = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.计划热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    核算热指标 = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.核算热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    实际运行热指标 = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.实际运行热指标 ?? 0.0m, i.日期.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    预报温度 = "[" + string.Join(",", forecasts.Where(i => i.Temperature.HasValue).Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    实际温度 = "[" + string.Join(",", actual.Where(i => i.Temperature.HasValue).Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    类型= Models.HeatConsumptionGraphType.Managership,
                    ManagershipId = managershipId
                },
                智能卡站个数 = managership.Stations.Count(i => string.Compare(i.数据来源, "智能卡") == 0 && i.是否为大网),
                监控站个数 = managership.Stations.Count(i => string.Compare(i.数据来源, "监控") == 0 && i.是否为大网),
                手抄表站个数 = managership.Stations.Count(i => string.IsNullOrEmpty(i.数据来源) && i.是否为大网),
                手抄表最近更新时间 = managership.Stations.Where(i => string.IsNullOrEmpty(i.数据来源)).OrderByDescending(i => i.采集时间).FirstOrDefault() != null ? managership.Stations.Where(i => string.IsNullOrEmpty(i.数据来源)).OrderByDescending(i => i.采集时间).FirstOrDefault().采集时间 : null,
                WeatherGraph = previousNextForecast,
                GJ总 = service.GetTodaysGJ总ByManagershipId(managershipId)
            };
            return View(managershipViewModel);
        }
    }
}
