using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [DataContract]
    public class WeatherStation
    {
        private int _weatherStationId;
        private string _weatherStationName;
        private decimal _currentTemperature;
        private DateTime _updatedAt;

        [DataMember]
        public string strListId
        {
            get {
                return _weatherStationId.ToString();
            }
            set { }
        }

        [DataMember]
        public string strListName
        {
            get
            {
                return _weatherStationName;
            }
            set
            {
            }
        }

        [DataMember]
        public decimal temperature
        {
            get
            {
                return _currentTemperature;
            }
            set { }
        }

        [DataMember]
        public string strListOther
        {
            get
            {
                return string.Format("{0:HH:mm}", _updatedAt);
            }
            set { }
        }

        public WeatherStation(int weatherStationId, string weatherStationName, decimal currentTemperatue, DateTime updatedAt)
        {
            _currentTemperature = currentTemperatue;
            _updatedAt = updatedAt;
            _weatherStationId = weatherStationId;
            _weatherStationName = weatherStationName;
        }
       

    }
}