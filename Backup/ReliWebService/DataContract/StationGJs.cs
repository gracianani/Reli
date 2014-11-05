using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class StationGJs : List<StationGJ>
    {
        public StationGJs() { }
        public StationGJs(List<StationGJ> stationGJs)
            : base(stationGJs) { }
    }
}