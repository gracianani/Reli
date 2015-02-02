using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [DataContract]
    public class ReliMobileHeatSourceSummary
    {
        [DataMember]
        public string strCountHeatSources
        {
            get
            {
                return "5";
            }
            set
            {
            }
        }

        [DataMember]
        public string strEastArea
        {
            get
            {
                return "103305069";
            }
            set
            {
            }
        }

        [DataMember]
        public string strWestArea
        {
            get
            {
                return "64248594";
            }
            set
            {
            }
        }


        [DataMember]
        public string strHeatLoad
        {
            get
            {
                return "8956558";
            }
            set
            {
            }
        }

        public ReliMobileHeatSourceSummary()
        {
        }
    }
}