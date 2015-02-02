using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class HeatSourceGJs : List<HeatSourceGJ>
    {
        public HeatSourceGJs() { }
        public HeatSourceGJs(List<HeatSourceGJ> heatSourceGJs) : base(heatSourceGJs) { }
    }
}