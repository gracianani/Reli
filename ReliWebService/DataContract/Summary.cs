using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [DataContract]
    public class Summary
    {
        private decimal? _forecastHighest;
        private decimal? _forecastLowest;
        private decimal? _forecastAverage;
        private string _windSpeedAndDirection;
        private string _weatherDescription;
        private int _weatherIcon;
        private int _countMessages;
        private int _countWarnings;
        private int _countPhotos;
        
        [DataMember]
        public decimal? forecastHighest {
            get
            {
                return _forecastHighest;
            }
            set
            {
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
            }
        }
        [DataMember]
        public decimal? forecastAverage{
            get {
                return _forecastAverage;
            }
            set {}
        }

        [DataMember]
        public string windSpeedAndDirection
        {
            get
            {
                return _windSpeedAndDirection;
            }
            set { }
        }

        [DataMember]
        public string weatherDescription
        {
            get
            {
                return _weatherDescription;
            }
            set { }
        }

        [DataMember]
        public int weatherIcon
        {
            get
            {
                return _weatherIcon;
            }
            set { }
        }

        [DataMember]
        public int countWarnings
        {
            get
            {
                return _countWarnings;
            }
            set { }
        }

        [DataMember]
        public int countPhotos
        {
            get
            {
                return _countPhotos;
            }
            set { }
        }

        [DataMember]
        public int countMessages
        {
            get
            {
                return _countMessages;
            }
            set { }
        }


        public Summary(decimal? forecastHighest, decimal? forecastLowest, decimal? forecastAverage,
            string windSpeedAndDirection, int weatherIcon, string weatherDescription,
            int countMessages, int countPhotos, int countWarnings)
        {
            _forecastHighest = forecastHighest;
            _forecastLowest = forecastLowest;
            _forecastAverage = forecastAverage;
            _windSpeedAndDirection = windSpeedAndDirection;
            _weatherIcon = weatherIcon;
            _weatherDescription = weatherDescription;
            _countMessages = countMessages;
            _countPhotos = countPhotos;
            _countWarnings = countWarnings;
        }

	
    }
}