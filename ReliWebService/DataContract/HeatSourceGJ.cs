using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [DataContract]
    public class HeatSourceGJ
    {
        [DataMember]
        public decimal dailyGJ { get; set; }

        [DataMember]
        public string date
        {
            get
            {
                return 日期.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set { }
        }

        [IgnoreDataMember]
        public DateTime 日期
        {
            get;
            set;
        }
    }
}