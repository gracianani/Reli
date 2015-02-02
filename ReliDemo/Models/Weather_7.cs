using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Infrastructure.Services;

namespace ReliDemo.Models
{
    public partial class Weather_7 : ITemperatureGraphItem
    {
        private xz2013Entities db = new xz2013Entities();

        public decimal? forecastHighest
        {
            get
            {
                return this._最高温;
            }
        }

        public decimal? forecastLowest
        {
            get
            {
                return this._最低温;
            }
        }

        public decimal? forecastAverage
        {
            get
            {
                return (forecastHighest + forecastLowest)/2.0m;
            }
        }

        public decimal? actualAverage
        {
            get
            {
                return null;
            }
        }

        public WeatherTypes weatherType
        {
            get
            {
                return (new WeatherService()).GetWeatherTypesByText(_天气);
            }
        }


        public decimal? actualHighest
        {
            get { return null;  }
        }

        public decimal? actualLowest
        {
            get { return null; }
        }

        public string windSpeedAndDirection
        {
            get { return _风力+_风向; }
        }
    }
}