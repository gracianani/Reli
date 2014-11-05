using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class ReliMobileStations : List<ReliMobileStation>
    {
        public ReliMobileStations() { }
        public ReliMobileStations(List<ReliMobileStation> stations) : base(stations) { }
    }
}