using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Infrastructure.Services;

namespace ReliDemo.Models
{
    public partial class weather_1 : ITemperatureGraphItem
    {
        public decimal? forecastHighest
        {
            get {
                return this.今日预报白天最高温;
            }
        }

        public decimal? forecastLowest
        {
            get {
                return this.今日预报夜间最低温;
            }
        }

        public decimal? forecastAverage
        {
            get {
                return this.今日预报一天平均温;
            }
        }

        public decimal? actualAverage
        {
            get {
                return null;
            }
        }

        public WeatherTypes weatherType
        {
            get {
                return (new WeatherService()).GetWeatherTypesByText(_白天天气);  
            }
        }

        public DateTime 日期
        {
            get { 
                return DateTime.Today;
            }
        }


        public decimal? actualHighest
        {
            get { return null; }
        }

        public decimal? actualLowest
        {
            get { return null; }
        }

        public string windSpeedAndDirection
        {
            get { return _白天风力; }
        }
    }
}