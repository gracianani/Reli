using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [DataContract]
    public class BackwardAndSupply
    {
        [DataMember]
        public decimal temperatureSupply { get; set; }

        [DataMember]
        public decimal temperatureBackward { get; set; }

        [DataMember]
        public decimal pressureSupply { get; set; }

        [DataMember]
        public decimal pressureBackward { get; set; }

        [IgnoreDataMember]
        public DateTime date { get; set; }

        [DataMember]
        public string time
        {
            get
            {
                return date.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set { }
        }
    }
}