using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ReliDemo.Models
{
    public class StationTemperatureStats
    {
        public DateTime 测温日期 { get; set; }
        public string 热力站名称 { get; set; }
        public int 节能16以下 { get; set; }
        public int 节能16至18 { get; set; }
        public int 节能18至20 { get; set; }
        public int 节能20至22 { get; set; }
        public int 节能22至24 { get; set; }
        public int 节能24以上 { get; set; }
        public int 非节能16以下 { get; set; }
        public int 非节能16至18 { get; set; }
        public int 非节能18至20 { get; set; }
        public int 非节能20至22 { get; set; }
        public int 非节能22至24 { get; set; }
        public int 非节能24以上 { get; set; }
        public StationTemperatureStats() { }
        public StationTemperatureStats(IDataReader reader)
        {
            测温日期 = reader.GetDateTime(reader.GetOrdinal("测温日期"));
            热力站名称 = reader.GetString(reader.GetOrdinal("热力站名称"));
            节能16以下 = reader.GetInt32(reader.GetOrdinal("16°C以下 节能"));
            节能16至18 = reader.GetInt32(reader.GetOrdinal("16°C-18°C 节能"));
            节能18至20 = reader.GetInt32(reader.GetOrdinal("18°C-20°C 节能"));
            节能20至22 = reader.GetInt32(reader.GetOrdinal("20°C-22°C 节能"));
            节能22至24 = reader.GetInt32(reader.GetOrdinal("22°C-24°C 节能"));
            节能24以上 = reader.GetInt32(reader.GetOrdinal("24°C以上 节能"));
            非节能16以下 = reader.GetInt32(reader.GetOrdinal("16°C以下 非节能"));
            非节能16至18 = reader.GetInt32(reader.GetOrdinal("16°C-18°C 非节能"));
            非节能18至20 = reader.GetInt32(reader.GetOrdinal("18°C-20°C 非节能"));
            非节能20至22 = reader.GetInt32(reader.GetOrdinal("20°C-22°C 非节能"));
            非节能22至24 = reader.GetInt32(reader.GetOrdinal("22°C-24°C 非节能"));
            非节能24以上 = reader.GetInt32(reader.GetOrdinal("24°C以上 非节能"));
        }
    }
}