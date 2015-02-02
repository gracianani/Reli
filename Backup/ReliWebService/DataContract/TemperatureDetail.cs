using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using ReliDemo.Models;
using ReliDemo.Infrastructure.Services;

namespace ReliWebService
{
    [DataContract]
    public class TemperatureDetail : ITemperatureGraphItem
    {
        private decimal? _forecastHighest;
        private decimal? _forecastLowest;
        private decimal? _forecastAverage;
        private decimal? _actualHighest;
        private decimal? _actualLowest;
        private decimal? _actualAverage;
        private DateTime _day;
        private string _weatherDescription;
        private string _windSpeedAndDirection;

        [DataMember]
        public decimal? forecastHighest
        {
            get
            {
                return _forecastHighest;
            }
            set
            {
                _forecastHighest = value;
            }
        }

        [DataMember]
        public decimal? forecastLowest
        {
            get
            {
                return _forecastLowest;
            }
            set
            {
                _forecastLowest = value;
            }
        }

        [DataMember]
        public decimal? forecastAverage
        {
            get
            {
                if (!_forecastAverage.HasValue)
                {
                    return (forecastHighest + forecastLowest) / 2.0m;
                }
                return _forecastAverage;
            }
            set
            {
                _forecastAverage = value;
            }
        }

        [DataMember]
        public decimal? actualHighest
        {
            get
            {
                return _actualHighest;
            }
            set
            {
                _actualHighest = value;
            }
        }

        [DataMember]
        public decimal? actualLowest
        {
            get
            {
                return _actualLowest;
            }
            set
            {
                _actualLowest = value;
            }
        }

        [DataMember]
        public decimal? actualAverage
        {
            get
            {
                return _actualAverage;
            }
            set
            {
                _actualAverage = value;
            }
        }

        [DataMember]
        public string weatherDescription
        {
            get
            {
                return _weatherDescription;
            }
            set
            {
                _weatherDescription = value;
            }
        }

        [DataMember]
        public string day
        {
            get
            {
                return _day.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set { }
        }

        private WeatherTypes _weatherTypes;
        public WeatherTypes weatherType
        {
            get { return _weatherTypes; }
            set { _weatherTypes = value; }
        }

        [DataMember]
        public string windSpeedAndDirection
        {
            get
            {
                return _windSpeedAndDirection;
            }
            set
            {
                _windSpeedAndDirection = value;
            }
        }

        [DataMember]
        public int weatherIcon
        {
            get
            {
                if (!string.IsNullOrEmpty(_weatherDescription))
                {
                    return (int)new WeatherService().GetWeatherTypesByText(_weatherDescription);
                }
                return 0;
            }
            set
            {
            }
        }

        public DateTime 日期
        {
            get { return _day; }
            set { _day = value; }
        }

        public TemperatureDetail()
        {
        }



    }
}