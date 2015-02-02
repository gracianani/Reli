using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class WeatherStations : List<WeatherStation>
    {
        public WeatherStations() { }
        public WeatherStations(List<WeatherStation> weatherStations)
            : base(weatherStations) { }
    }
}