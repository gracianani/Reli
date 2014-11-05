using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.ComponentModel.DataAnnotations;

namespace ReliDemo.ViewModels
{
    public class WeatherStationForecastViewModel
    {
        public int ItemId { get; set; }
        public weather_1 今日预测 { get; set; }
        public IEnumerable<Weather_7> 七日预测 { get; set; }
        public Weather_10 十日预测 { get; set; }
        public IEnumerable<ITemperatureGraphItem> 七日预测实际温度 { get; set; }
        public Weather_2 原始文本 { get; set; }

        [Required(ErrorMessage="请注明更新日期")]
        public DateTime ModifyDate { get; set; }

    }

    public class WeatherStationViewModel
    {
        public int WeatherStationId { get; set; }
        public string WeatherStationName { get; set; }
        public List<TimeTemperature> Temperatures { get; set; }
    }

    public class WeatherStationsLastestViewModel
    {
        public List<WeatherStationViewModel> WeatherStations { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}