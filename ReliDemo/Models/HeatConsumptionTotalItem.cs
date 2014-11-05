using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public class HeatConsumptionTotalItem
    {
        public string Title { get; set; }
        public decimal? 全网 { get; set; }
        public decimal? 东部 { get; set; }
        public decimal? 西部 { get; set; }
        public bool IsToday { get; set; }
        public bool IsPeriod { get; set; }
        public decimal? 总 { get; set; }
        public decimal? 监测站智能卡 { get; set; }
        public decimal? 其它 { get; set; }
        public decimal? 今日 { get; set; }
        public decimal? 昨日 { get; set; }
        public decimal? 人工抄表 { get; set; }
    }
}