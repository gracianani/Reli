using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class TemperatureDetails : List<TemperatureDetail>
    {
        public TemperatureDetails() {}
        public TemperatureDetails(List<TemperatureDetail> temperatureDetails) : base(temperatureDetails) { }
    }
}