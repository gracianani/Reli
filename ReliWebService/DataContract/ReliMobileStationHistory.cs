using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ReliWebService
{
    public class ReliMobileStationHistory
    {
        public ReliMobileStationHistory()
        {
        }

        public ReliMobileStationHistory(IDataReader reader)
        {
            时间 = reader.GetDateTime(reader.GetOrdinal("时间"));
            PressureOut1st = reader.GetDecimal(reader.GetOrdinal("一次供压"));
            PressureIn1st = reader.GetDecimal(reader.GetOrdinal("一次回压"));
            TemperatureOut1st = reader.GetDecimal(reader.GetOrdinal("一次供温"));
            TemperatureIn1st = reader.GetDecimal(reader.GetOrdinal("一次回温"));
        }
        public DateTime 时间
        {
            get;
            set;
        }
        public decimal PressureOut1st
        {
            get;
            set;
        }
        public decimal PressureIn1st
        {
            get;
            set;
        }
        public decimal TemperatureOut1st
        {
            get;
            set;
        }
        public decimal TemperatureIn1st
        {
            get;
            set;
        }
        public decimal plannedGJ
        {
            get;
            set;
        }
        public decimal actualGJ
        {
            get;
            set;
        }
        public decimal calculatedGJ
        {
            get;
            set;
        }
        public decimal forecastTemperature
        {
            get;
            set;
        }
        public decimal actualTemperature
        {
            get;
            set;
        }
        public decimal exceed
        {
            get;
            set;
        }
    }
}