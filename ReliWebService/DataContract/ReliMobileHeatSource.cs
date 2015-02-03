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
    public class ReliMobileHeatSource
    {
        private HeatSource _dbHeatSource;
        public HeatSource DBHeatSource
        {
            get
            {
                if (_dbHeatSource == null)
                {
                    _dbHeatSource = new HeatSource();
                }
                return _dbHeatSource;
            }
            set
            {
                _dbHeatSource = value;
            }
        }

        [DataMember]
        public int heatSourceId
        {
            get
            {
                return Convert.ToInt32(DBHeatSource.ItemID);
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
                return DBHeatSource.热源名称;
            }
            set
            {
            }
        }

        [DataMember]
        public string lastUpdatedAt
        {
            get
            {
                return DBHeatSource.采集时间.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
            }
        }

        [DataMember]
        public string heatSourceType
        {
            get
            {
                return DBHeatSource.机组类型;
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
                return DBHeatSource.东西部;
            }
            set
            {
            }
        }

        [DataMember]
        public string innerOrOuter
        {
            get
            {
                return DBHeatSource.内外部;
            }
            set
            {
            }
        }

        [DataMember]
        public string waterLine
        {
            get
            {
                return DBHeatSource.水线名称;
            }
            set
            {
            }
        }

        [DataMember]
        public string gasLine
        {
            get
            {
                return DBHeatSource.蒸汽线名称;
            }
            set
            {
            }
        }

        [DataMember]
        public int peakCoalCount
        {
            get
            {
                return Convert.ToInt32(DBHeatSource.燃煤尖峰炉数);
            }
            set
            {
            }
        }

        [DataMember]
        public int peakGasCount
        {
            get
            {
                return Convert.ToInt32(DBHeatSource.燃气尖峰炉数);
            }
            set
            {
            }
        }

        [DataMember]
        public bool isInSystem
        {
            get
            {
                return Convert.ToBoolean(DBHeatSource.是否并网供热);
            }
            set
            {
            }
        }

        [DataMember]
        public int sequence
        {
            get
            {
                return Convert.ToInt32(DBHeatSource.sequence); ;
            }
            set
            {
            }
        }
    }
}