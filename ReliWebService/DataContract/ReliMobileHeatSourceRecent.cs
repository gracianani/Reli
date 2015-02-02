using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using ReliDemo.Models;
using ReliDemo.Infrastructure.Helpers;

namespace ReliWebService
{
    [DataContract]
    public class ReliMobileHeatSourceRecent
    {
        private HeatSourceRecent _dbHeatSourceRecent;
        public HeatSourceRecent DBHeatSourceRecent
        {
            get
            {
                if (_dbHeatSourceRecent == null)
                {
                    _dbHeatSourceRecent = new HeatSourceRecent();
                }
                return _dbHeatSourceRecent;
            }
            set
            {
                _dbHeatSourceRecent = value;
            }
        }

        [DataMember]
        public string lastUpdatedAt
        {
            get
            {
                return DBHeatSourceRecent.采集时间.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
            }
        }
        [DataMember]
        public int heatSourceId
        {
            get
            {
                return Convert.ToInt32(DBHeatSourceRecent.生产热源ID);
            }
            set
            {
            }
        }

        [DataMember]
        public int heatSourceRecentId
        {
            get
            {
                return Convert.ToInt32(DBHeatSourceRecent.机组号);
            }
            set
            {
            }
        }
        [DataMember]
        public string name
        {
            get
            {
                return DBHeatSourceRecent.机组名;
            }
            set
            {
            }
        }

        [DataMember]
        public string pressureIn
        {
            get
            {
                return Convert.ToDecimal(DBHeatSourceRecent.回压).ToString("0.0");
            }
            set
            {
            }
        }

        [DataMember]
        public string pressureOut
        {
            get
            {
                return Convert.ToDecimal(DBHeatSourceRecent.供压).ToString("0.0");
            }
            set
            {
            }
        }

        [DataMember]
        public string temperatureIn
        {
            get
            {
                return Convert.ToDecimal(DBHeatSourceRecent.回温).ToString("0.0");
            }
            set
            {
            }
        }

        [DataMember]
        public string temperatureOut
        {
            get
            {
                return Convert.ToDecimal(DBHeatSourceRecent.供温).ToString("0.0");
            }
            set
            {
            }
        }

        [DataMember]
        public string instWater
        {
            get
            {
                return Convert.ToDecimal(DBHeatSourceRecent.瞬时供水流量).ToString("0.0");
            }
            set
            {
            }
        }

        [DataMember]
        public string instHeat
        {
            get
            {
                return Convert.ToDecimal(DBHeatSourceRecent.瞬时热量).ToString("0.0");
            }
            set
            {
            }
        }

        [DataMember]
        public string accuWater
        {
            get
            {
                return Convert.ToDecimal(DBHeatSourceRecent.累计供水流量).ToString("0.0");
            }
            set
            {
            }
        }

        [DataMember]
        public string accuHeat
        {
            get
            {
                return Convert.ToDecimal(DBHeatSourceRecent.累计热量).ToString("0.0");
            }
            set
            {
            }
        }

        [DataMember]
        public string waterSupply
        {
            get
            {
                return Convert.ToDecimal(DBHeatSourceRecent.瞬时补水量).ToString("0.0");
            }
            set
            {
            }
        }
    }
}