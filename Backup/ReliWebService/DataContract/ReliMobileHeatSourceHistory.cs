using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Runtime.Serialization;
using System.Data;

namespace ReliWebService
{
    public class ReliMobileHeatSourceHistory
    {
        public DateTime 时间
        {
            get;
            set;
        }
        public decimal PressureOut
        {
            get;
            set;
        }
        public decimal PressureIn
        {
            get;
            set;
        }
        public decimal TemperatureOut
        {
            get;
            set;
        }
        public decimal TemperatureIn
        {
            get;
            set;
        }
        public ReliMobileHeatSourceHistory()
        {
        }
        internal ReliMobileHeatSourceHistory(IDataReader reader)
        {
            时间 = reader.GetDateTime(reader.GetOrdinal("时间"));
            PressureOut = reader.GetDecimal(reader.GetOrdinal("供压"));
            PressureIn = reader.GetDecimal(reader.GetOrdinal("回压"));
            TemperatureOut = reader.GetDecimal(reader.GetOrdinal("供温"));
            TemperatureIn = reader.GetDecimal(reader.GetOrdinal("回温"));
        }
    }
}