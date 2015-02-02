using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace ReliDemo.Infrastructure.Services
{
    public class WeatherService
    {
        private xz2013Entities db = new xz2013Entities();

        private string recalculateDataSql = "Insert into Recalc_DaysHeat(开始日期, 结束日期, 请求人, 折算温度) values(@fromDate, @toDate, @user, @referingTemperature)";

        private void Recalculate(DateTime fromDate, DateTime toDate, decimal referingTemperature, string user)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = recalculateDataSql;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("fromDate", fromDate.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("toDate", toDate.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter("user", user));
                    cmd.Parameters.Add(new SqlParameter("referingTemperature", referingTemperature.ToString()));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public bool UpdateWeather(DateTime date, decimal forecastTemperature, decimal actualTemperature)
        {
            if(db.Weather_4.Count(i=>i.日期 == date) == 1) 
            {
                var weather = db.Weather_4.Single(i=>i.日期 == date);
                weather.预报平均 = forecastTemperature;
                weather.朝阳平均 = actualTemperature;
                db.SaveChanges();
                Recalculate(date, date.AddDays(1), -9.0m, MembershipService.CurrentUser.FullName);
                return true;
            }
            else if (db.Weather_4.Count(i => i.日期 == date) == 0)
            {
                Weather_4 weather = new Weather_4();
                weather.日期 = date;
                weather.预报平均 = forecastTemperature;
                weather.朝阳平均 = actualTemperature;
                db.Weather_4.AddObject(weather);
                db.SaveChanges();
                Recalculate(date, date.AddDays(1), -9.0m, MembershipService.CurrentUser.FullName);
                return true;
            }
            
            // insert into Recalc_DaysHeat(开始日期,结束日期,请求人,折算温度) 
            //   values('20131115','20131120','刘力',-9)

            return false;
        }
        public WeatherTypes GetWeatherTypesByText(string weatherText)
        {
            var weatherTexts = weatherText.Split(new string[] { "转", "有", "," }, StringSplitOptions.RemoveEmptyEntries);
            WeatherTypes weatherType;
            if(Enum.TryParse<WeatherTypes>( weatherTexts[0].Replace("天", ""), out weatherType)) {
                return weatherType;
            }
            else return WeatherTypes.晴;
        }

        public DateTime GetLatestPublishedAt()
        {
            var lastestText = db.Weather_2.OrderByDescending(i => i.日期).First();
            if (!string.IsNullOrEmpty(lastestText.十七时))
            {
                return lastestText.日期.AddHours(17);
            }
            else
            {
                return lastestText.日期.AddHours(6);
            }
        }

        public Weather_2 GetText()
        {
            return db.Weather_2.OrderByDescending(i => i.日期).First();
        }

        public Weather_0 GetLatestAllOfficial()
        {
            return db.Weather_0.OrderByDescending(i=>i.时间).First();
        }

        public IEnumerable<Weather_7> GetSevenDays()
        {
            return db.Weather_7.Take(7);
        }

        public IEnumerable<ITemperatureGraphItem> GetPreviousDays(int days)
        {
            return db.Weather_4.OrderByDescending(i => i.日期).Where(i=>i.朝阳平均.HasValue && i.日期 < DateTime.Today).Take(days);
        }

        public IEnumerable<ITemperatureGraphItem> GetHistory(DateTime fromDate, DateTime toDate)
        {
            return db.Weather_4.OrderBy(i => i.日期).Where(i => i.日期 >= fromDate && i.日期 <= toDate);
        }

        public IEnumerable<ITemperatureGraphItem> GetNextDays(int days)
        {
            return db.Weather_7.Where(i=>i.日期 != null &&　i.日期 > DateTime.Today).OrderBy(i=>i.日期).Take(days);
        }

        public weather_1 GetToday()
        {
            return db.weather_1.First();
        }
        public weather_1 GetYeasterday()
        {
            return db.weather_1.First();
        }
        public Weather_10 GetTenDays()
        {
            return db.Weather_10.First();
        }

        public TimeTemperature GetActual(DateTime date)
        {
            var forecast = db.Weather_4.FirstOrDefault(i => i.日期 == date);
            return new TimeTemperature
            {
                Temperature = forecast != null ? forecast.朝阳平均 : null,
                时间 = date,
                更新频率 = "day",
                气象站ID = 0,
                TemperatureType = TemperatureType.预测平均温度
            };
        }

        public IEnumerable<TimeTemperature> GetActual(DateTime fromDate, DateTime toDate)
        {
            fromDate = fromDate.Date;
            toDate = toDate.Date;
            var forecasts = db.Weather_4.Where(i => i.日期 >= fromDate && i.日期 <= toDate);

            var dates = new List<DateTime>();
            var span = toDate - fromDate;
            for (int i = 0; i < span.Days; i++)
            {
                dates.Add(fromDate.AddDays(i));
            }
            
            var forecastWithDate = (from date in dates
                                   from forecast in forecasts.Where(i => i.日期 == date).DefaultIfEmpty()
                                   select new TimeTemperature
                                   {
                                       时间 = date,
                                       更新频率 = "day",
                                       TemperatureType = TemperatureType.预测平均温度,
                                       气象站ID = 0,
                                       Temperature = forecast != null ? forecast.朝阳平均  : null
                                   }).ToList();
            if (DateTime.Today >= fromDate && DateTime.Today <= toDate)
            {
                forecastWithDate.Add(new TimeTemperature()
                {
                    时间 = DateTime.Today,
                    更新频率 = "day",
                    TemperatureType = TemperatureType.预测平均温度,
                    气象站ID = 0,
                    Temperature = db.weather_1.First().今日预报一天平均温
                });
            }
            return forecastWithDate;

        }

        public IEnumerable<TimeTemperature> GetForecast(DateTime fromDate, DateTime toDate)
        {
            fromDate = fromDate.Date;
            toDate = toDate.Date;
            var forecasts = db.Weather_4.Where(i => i.日期 >= fromDate && i.日期 <= toDate);
            var dates = new List<DateTime>();
            var span = toDate-fromDate;
            for(int i=0;i<span.Days; i++) {
                dates.Add(fromDate.AddDays(i));
            }
            var forecastWithDate = from date in dates
                                   from forecast in forecasts.Where(i=>i.日期 == date).DefaultIfEmpty()
                                   select new TimeTemperature
                                   {
                                       时间 = date,
                                       更新频率 = "day",
                                       TemperatureType = TemperatureType.预测平均温度,
                                       气象站ID = 0,
                                       Temperature = forecast != null ? forecast.预报平均 : null
                                   };
            return forecastWithDate;
        }

        public List<TimeTemperature> GetRealtime(int stationId, DateTime fromDate, DateTime toDate)
        {
            var temperatures = new List<TimeTemperature>();
            var stationEnum = (WeatherStations)stationId;
            switch( stationEnum) {
                case WeatherStations.天安门:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                        .Select(i => new TimeTemperature() {  气象站ID = stationId, 时间 = i.时间, Temperature = i.天安门.Value }).ToList();
                    break;
                }
                case WeatherStations.奥体中心 : 
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                        .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.奥体中心.Value }).ToList();
                    break;
                }
                case WeatherStations.大观园:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.大观园.Value }).ToList();
                    break;
                }
                case WeatherStations.四惠桥:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.四惠桥.Value }).ToList();
                    break;
                }
                case WeatherStations.和平西桥:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.和平西桥.Value }).ToList();
                    break;
                }
                case WeatherStations.古观象台:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.古观象台.Value}).ToList();
                    break;
                }
                case WeatherStations.双榆树:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.双榆树.Value}).ToList();
                    break;
                }
                case WeatherStations.官园:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.官园.Value }).ToList();
                    break;
                }
                case WeatherStations.宝能:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.宝能.Value }).ToList();
                    break;
                }
                case WeatherStations.左热:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.左热.Value }).ToList();
                    break;
                }
                case WeatherStations.方庄:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.方庄.Value}).ToList();
                    break;
                }
                case WeatherStations.朝阳:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.朝阳.Value }).ToList();
                    break;
                }
                case WeatherStations.海淀:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.海淀.Value}).ToList();
                    break;
                }
                case WeatherStations.玉渊潭:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.玉渊潭.Value }).ToList();
                    break;
                }
                case WeatherStations.石景山:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.石景山.Value }).ToList();
                    break;
                }
                case WeatherStations.观象台:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.观象台.Value }).ToList();
                    break;
                }
                case WeatherStations.车道沟:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.车道沟.Value }).ToList();
                    break;
                }
                case WeatherStations.雕塑园:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.雕塑园.Value}).ToList();
                    break;
                }
                case WeatherStations.龙潭湖:
                {
                    temperatures = db.Weather_0.Where(i => i.时间 >= fromDate && i.时间 < toDate)
                    .Select(i => new TimeTemperature() { 气象站ID = stationId, 时间 = i.时间, Temperature = i.龙潭湖.Value}).ToList();
                    break;
                }
            }
            foreach (var temperature in temperatures)
            {
                temperature.TemperatureType = TemperatureType.实时温度;
            }
            return temperatures;
        }
    }
}