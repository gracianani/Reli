using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public partial class Weather_4 : ITemperatureGraphItem
    {
        private xz2013Entities db = new xz2013Entities();

        public decimal? forecastHighest
        {
            get {
                return _预报最高;
            }
        }

        public decimal? forecastLowest
        {
            get
            {
                return _预报最低;
            }
        }

        public decimal? actualHighest
        {
            get
            {
                return _实际最高;
            }
        }

        public decimal? actualLowest
        {
            get
            {
                return _实际最低;
            }
        }

        public decimal? forecastAverage
        {
            get
            {
                return _预报平均;
            }
        }

        public decimal? actualAverage
        {
            get {
                return _朝阳平均;
            }
        }

        public WeatherTypes weatherType
        {
            get {
                return WeatherTypes.Unknow;
            }
        }

        public string windSpeedAndDirection
        {
            get { return null; }
        }
    }
}