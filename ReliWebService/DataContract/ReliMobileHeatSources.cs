using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class ReliMobileHeatSources : List<ReliMobileHeatSource>
    {
        public ReliMobileHeatSources() { }
        public ReliMobileHeatSources(List<ReliMobileHeatSource> heatSources) : base(heatSources) { }
    }
}