using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ReliDemo.Infrastructure.Services;
using ReliDemo.ViewModels;
using ReliDemo.Models;
using System.Configuration;
using System.IO;
using OfficeOpenXml;

namespace ReliDemo.Controllers
{
    [Authorize]
    public class WeatherController : Controller
    {
        private WeatherService _weatherService;

        public WeatherController()
        {
            _weatherService = new WeatherService();
        }

        public ActionResult Official()
        {
            var weatherStationsLastestViewModel = new WeatherStationsLastestViewModel();
            var lastestOfficials = _weatherService.GetLatestAllOfficial();
            var l = new List<WeatherStationViewModel>();
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "双榆树"),
                WeatherStationName = "双榆树",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.双榆树 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "古观象台"),
                WeatherStationName = "古观象台",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.古观象台 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "和平西桥"),
                WeatherStationName = "和平西桥",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.和平西桥 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "四惠桥"),
                WeatherStationName = "四惠桥",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.四惠桥 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "大观园"),
                WeatherStationName = "大观园",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.大观园 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "天安门"),
                WeatherStationName = "天安门",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.天安门 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "奥体中心"),
                WeatherStationName = "奥体中心",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.奥体中心 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "官园"),
                WeatherStationName = "官园",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.官园 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "宝能"),
                WeatherStationName = "宝能",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.宝能 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "左热"),
                WeatherStationName = "左热",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.左热 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "方庄"),
                WeatherStationName = "方庄",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.方庄 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "朝阳"),
                WeatherStationName = "朝阳",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.朝阳 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "海淀"),
                WeatherStationName = "海淀",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.海淀 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "玉渊潭"),
                WeatherStationName = "玉渊潭",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.玉渊潭 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "石景山"),
                WeatherStationName = "石景山",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.石景山 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "观象台"),
                WeatherStationName = "观象台",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.观象台 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "车道沟"),
                WeatherStationName = "车道沟",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.车道沟 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "雕塑园"),
                WeatherStationName = "雕塑园",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.雕塑园 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            l.Add(new WeatherStationViewModel()
            {
                WeatherStationId = (int)Enum.Parse(typeof(WeatherStations), "龙潭湖"),
                WeatherStationName = "龙潭湖",
                Temperatures = new List<TimeTemperature>() { new TimeTemperature() { Temperature = lastestOfficials.龙潭湖 ?? 0.0m, 时间 = lastestOfficials.时间, TemperatureType = TemperatureType.实时温度 } }
            });
            weatherStationsLastestViewModel.WeatherStations = l;
            weatherStationsLastestViewModel.UpdatedAt = lastestOfficials.时间;
            return View(weatherStationsLastestViewModel);
        }

        public ActionResult WeatherForecast()
        {
            var sevenDaysForecast = _weatherService.GetNextDays(7);
            var previousDays = _weatherService.GetPreviousDays(7).Union(sevenDaysForecast).ToList();
            previousDays.Add(_weatherService.GetToday());
            previousDays = previousDays.OrderBy(i => i.日期).ToList();
            var weaterStationViewModel = new WeatherStationForecastViewModel()
            {
                七日预测 = _weatherService.GetSevenDays(),
                今日预测= _weatherService.GetToday(),
                十日预测 = _weatherService.GetTenDays(),
                七日预测实际温度 = previousDays,
                原始文本 = _weatherService.GetText()
            };
            return View(weaterStationViewModel);
        }

        public ActionResult WeatherStation(int weatherStationId)
        {
            var weatherStationViewModel = new WeatherStationViewModel();
            weatherStationViewModel.Temperatures = _weatherService.GetRealtime(weatherStationId, DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
            weatherStationViewModel.WeatherStationId = weatherStationId;
            weatherStationViewModel.WeatherStationName = ((WeatherStations)weatherStationId).ToString();
            return View(weatherStationViewModel);
        }

        public ActionResult DownloadWeatherReport(string daterange)
        {
            ReportType reportType = ReportType.天气预报历史;
            
            var fromdate_todate = daterange.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            var fromDate = DateTime.Parse(fromdate_todate[0]);
            var toDate = DateTime.Parse(fromdate_todate[1]);
           
            var report = (WeatherReport)ReportFactory.CreateReport(reportType, fromDate);
            report.FromDate = fromDate;
            report.ToDate = toDate;

            var templateName = string.Format("{0}{1}.xlsx", ConfigurationManager.AppSettings["ReportTemplateDirectory"], report.TemplateName);
            FileInfo template = new FileInfo(templateName);
            var newFileName = string.Format("{0}{1:yyyy-MM-dd}-{2:yyyy-MM-dd}.xlsx", report.TemplateName, fromDate, toDate);
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

        public ActionResult UpdateTemperature(string date, string forecastTemperature, string actualTemperature)
        {
            try {
                var Date = Convert.ToDateTime(date);
                var ForecastTemperature = Convert.ToDecimal(forecastTemperature);
                var ActualTemperature = Convert.ToDecimal(actualTemperature);
                _weatherService.UpdateWeather(Date, ForecastTemperature, ActualTemperature);
                return Json(new { hasError = false }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex) {
                return Json(new { hasError = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
