using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class ReliMobileStationSummaries: List<ReliMobileStationSummary>
    {
        public ReliMobileStationSummaries() { }
        public ReliMobileStationSummaries(List<ReliMobileStationSummary> stations) : base(stations) { }
    }
}