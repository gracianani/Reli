using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public class GJHistoryItem
    {
        public int StationId { get; set; }
        public decimal? 计划GJ { get; set; }
        public decimal? 核算GJ { get; set; }
        public decimal? 采暖GJ { get; set; }
        public DateTime 日期 { get; set; }
        public decimal? 预报温度 { get; set; }
        public decimal? 实测温度 { get; set; }
        public decimal? 计划热指标 { get; set; }
        public decimal? 核算热指标 { get; set; }
        public decimal? 实际运行热指标 { get; set; }
    }
}