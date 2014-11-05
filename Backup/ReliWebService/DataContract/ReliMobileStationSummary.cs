using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    public class ReliMobileStationSummary
    {
        [DataMember]
        public int stationId
        {
            get;
            set;
        }

        [DataMember]
        public string stationName
        {
            get;
            set;
        }


        [DataMember]
        public bool isChaoBiao
        {
            get;
            set;
        }

        [DataMember]
        public decimal? pressureIn
        {
            get;
            set;
        }

        [DataMember]
        public decimal? pressureOut
        {
            get;
            set;
        }

        [DataMember]
        public decimal? temperatureIn
        {
            get;
            set;
        }

        [DataMember]
        public decimal? temperatureOut
        {
            get;
            set;
        }

        [DataMember]
        /// 数据来源 : 抄表, 智能卡, 监控
        public string type 
        {
            get;
            set;
        }

        [DataMember]
        /// 东西部
        public string eastOrWest
        {
            get;
            set;
        }
    }
}