using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public class TimeTemperature
    {
        public DateTime 时间 { get; set; }
        public decimal? Temperature { get; set; }
        public int 气象站ID { get; set; }
        public string 更新频率 { get; set; }
        public TemperatureType TemperatureType { get; set; }
    }
}