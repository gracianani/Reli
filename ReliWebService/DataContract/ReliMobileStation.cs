using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [DataContract]
    public class ReliMobileStation
    {
        private Station _dbStation;
        public Station DBStation 
        {
            get
            {
                if (_dbStation == null)
                {
                    _dbStation = new Station();
                }
                return _dbStation;
            }
            set
            {
                _dbStation = value;
            }
        }

        [DataMember]
        public int stationId
        {
            get
            {
                return _dbStation.ItemID;
            }
            set
            {
            }
        }

        [DataMember]
        public string stationName
        {
            get
            {
                return _dbStation.热力站名称;
            }
            set
            {
            }
        }


        [DataMember]
        public bool isChaoBiao
        {
            get
            {
                return string.IsNullOrEmpty(_dbStation.数据来源);
            }
            set { }
        }

        [DataMember]
        public decimal plannedGJToday
        {
            get
            {
                return Convert.ToDecimal(_dbStation.今日计划GJ);
            }
            set
            {
            }
        }

        [DataMember]
        public decimal actualGJToday
        {
            get
            {
                return Convert.ToDecimal(_dbStation.今日GJ);
            }
            set
            {
            }
        }

        [DataMember]
        public decimal plannedGJYesterday
        {
            get
            {
                return Convert.ToDecimal(_dbStation.昨日计划GJ);
            }
            set
            {
            }
        }

        [DataMember]
        public decimal actualGJYesterday
        {
            get
            {
                return Convert.ToDecimal(_dbStation.昨日GJ);
            }
            set
            {
            }
        }

        [DataMember]
        public decimal calculatedGJYesterday
        {
            get
            {
                return Convert.ToDecimal(_dbStation.昨日核算GJ);
            }
            set
            {
            }
        }

        [DataMember]
        public decimal instantaneousHeat
        {
            get
            {
                return Convert.ToDecimal( _dbStation.总瞬时热量 );
            }
            set
            {
            }
        }

        [DataMember]
        public decimal instantaneousWater
        {
            get
            {
                return Convert.ToDecimal(_dbStation.总瞬时流量);
            }
            set
            {
            }
        }

        [DataMember]
        public decimal accumulatedHeat
        {
            get
            {
                return Convert.ToDecimal(_dbStation.总累计热量);
            }
            set
            {
            }
        }

        [DataMember]
        public decimal accumulatedWater
        {
            get
            {
                return Convert.ToDecimal(_dbStation.总累计流量);
            }
            set
            {
            }
        }

        [DataMember]
        public string heatSourceName
        {
            get
            {
                return _dbStation.生产热源;
            }
            set
            {
            }
        }

        [DataMember]
        public decimal pressureIn
        {
            get
            {
                return Convert.ToDecimal(_dbStation.一次回压);
            }
            set
            {
            }
        }

        [DataMember]
        public decimal pressureOut
        {
            get
            {
                return Convert.ToDecimal(_dbStation.一次供压);
            }
            set
            {
            }
        }

        [DataMember]
        public decimal temperatureIn
        {
            get
            {
                return Convert.ToDecimal(_dbStation.一次回温);
            }
            set
            {
            }
        }

        [DataMember]
        public decimal temperatureOut
        {
            get
            {
                return Convert.ToDecimal(_dbStation.一次供温);
            }
            set
            {
            }
        }

        [DataMember]
        public string eastOrWest
        {
            get
            {
                switch (_dbStation.生产热源ID)
                {
                    case 1: return "东部";
                    case 22: return "西部";
                    default: return "";
                }
            }
            set
            {
            }
        }

        public ReliMobileStation()
        {
        }
    }
}