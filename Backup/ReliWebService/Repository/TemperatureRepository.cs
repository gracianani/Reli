using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliWebService.DataContract;

namespace ReliWebService.Repository
{
    public class TemperatureRepository
    {
        private ReliMobileEntities db = new ReliMobileEntities();
        private TemperatureDetail _today;
        private TemperatureDetail _yesterday;

        private IEnumerable<TemperatureDetail> _histories;
        public IEnumerable<TemperatureDetail> Histories
        {
            get { return _histories; }
        }
        private List<TemperatureDetail> _forecasts;
        public List<TemperatureDetail> Forecasts
        {
            get { return _forecasts; }
        }

        public TemperatureDetail Today
        {
            get
            {
                return _today;
            }
        }
        public List<TemperatureDetail> TodayAndYesterday
        {
            get
            {
                return new List<TemperatureDetail>() {
                    _today, _yesterday };
            }
        }

        private List<WeatherStation> _weatherStations;
        public List<WeatherStation> WeatherStations
        {
            get
            {
                return _weatherStations;
            }
            set
            {
                _weatherStations = value;
            }
        }
        public TemperatureRepository()
        {
            _weatherStations = new List<WeatherStation>();
            var dateFrom = DateTime.Now.AddDays(-7);
            var dateTo = DateTime.Now.AddDays(7);
            _histories = db.Weather_4.OrderByDescending(i=>i.日期).Select(i => new TemperatureDetail() { 
                日期 = i.日期, actualHighest = i.实际最高, actualLowest = i.实际最低, 
                actualAverage = i.朝阳平均, forecastHighest = i.预报最高, forecastLowest = i.预报最低, 
            forecastAverage = i.预报平均});
            _forecasts = db.Weather_7.Select(i => new TemperatureDetail()
            {
                日期 = i.日期,
                forecastHighest = i.最高温,
                forecastLowest = i.最低温,
                windSpeedAndDirection = i.风力 + i.风向,
                weatherDescription = i.天气,
                forecastAverage = ( (i.最高温 ?? 0.0m) + (i.最低温 ?? 0.0m) ) / 2.0m
            }).ToList();

            var todayAndYesterday = db.weather_1.First();
            _today = new TemperatureDetail()
            {
                日期 = DateTime.Today,
                forecastHighest = todayAndYesterday.今日预报白天最高温,
                forecastAverage = todayAndYesterday.今日预报一天平均温,
                forecastLowest = todayAndYesterday.今日预报夜间最低温,
                weatherDescription = todayAndYesterday.白天天气,
                windSpeedAndDirection = todayAndYesterday.白天风力
            };
            _yesterday = new TemperatureDetail()
            {
                日期 = DateTime.Today.AddDays(-1),
                actualAverage = todayAndYesterday.昨日实况一天平均温,
                actualHighest = todayAndYesterday.昨日实况白天最高温,
                actualLowest = todayAndYesterday.昨日实况夜间最低温
            };

            var weatherStationTemperatures = db.Weather_0.OrderByDescending(i => i.时间).First();
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.朝阳, "朝阳", Convert.ToDecimal(weatherStationTemperatures.朝阳), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.天安门, "天安门", Convert.ToDecimal(weatherStationTemperatures.天安门), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.古观象台, "古观象台", Convert.ToDecimal(weatherStationTemperatures.古观象台), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.双榆树, "双榆树", Convert.ToDecimal(weatherStationTemperatures.双榆树), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.方庄, "方庄", Convert.ToDecimal(weatherStationTemperatures.方庄), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.左热, "左热", Convert.ToDecimal(weatherStationTemperatures.左热), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.宝能, "宝能", Convert.ToDecimal(weatherStationTemperatures.宝能), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.四惠桥, "四惠桥", Convert.ToDecimal(weatherStationTemperatures.四惠桥), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.官园, "官园", Convert.ToDecimal(weatherStationTemperatures.官园), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.大观园, "大观园", Convert.ToDecimal(weatherStationTemperatures.大观园), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.海淀, "海淀", Convert.ToDecimal(weatherStationTemperatures.海淀), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.石景山, "石景山", Convert.ToDecimal(weatherStationTemperatures.石景山), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.车道沟, "车道沟", Convert.ToDecimal(weatherStationTemperatures.车道沟), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.玉渊潭, "玉渊潭", Convert.ToDecimal(weatherStationTemperatures.玉渊潭), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.龙潭湖, "龙潭湖", Convert.ToDecimal(weatherStationTemperatures.龙潭湖), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.观象台, "观象台", Convert.ToDecimal(weatherStationTemperatures.观象台), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.和平西桥, "和平西桥", Convert.ToDecimal(weatherStationTemperatures.和平西桥), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.奥体中心, "奥体中心", Convert.ToDecimal(weatherStationTemperatures.奥体中心), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.雕塑园, "雕塑园", Convert.ToDecimal(weatherStationTemperatures.雕塑园), weatherStationTemperatures.时间));
            _weatherStations.Add(new WeatherStation((int)WeatherStationName.通州, "通州", Convert.ToDecimal(weatherStationTemperatures.通州), weatherStationTemperatures.时间));
        }
    }
}