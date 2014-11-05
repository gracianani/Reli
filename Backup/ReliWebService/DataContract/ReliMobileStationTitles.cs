using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class ReliMobileStationTitles : List<ReliMobileStationTitle>
    {
        public ReliMobileStationTitles() { }
        public ReliMobileStationTitles(List<ReliMobileStationTitle> stations) : base(stations) { }
    }
}