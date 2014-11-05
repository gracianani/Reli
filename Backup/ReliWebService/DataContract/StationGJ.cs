using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [DataContract]
    public class StationGJ
    {
        [DataMember]
        public decimal planGJ { get; set; }

        [DataMember]
        public decimal calculateGJ { get; set; }

        [DataMember]
        public decimal actualGJ { get; set; }

        [DataMember]
        public decimal actualOverCalculateGJ { get; set; }

        [DataMember]
        public decimal forecastTemperature { get; set; }

        [DataMember]
        public decimal actualTemperature { get; set; }

        [DataMember]
        public string date { 
            get {
                return 日期.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set{}
        }

        [IgnoreDataMember]
        public DateTime 日期
        {
            get;
            set;
        }
    }
}