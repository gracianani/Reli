using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public class HeatConsumptionSummaryItem
    {
        public decimal? 计划供热量GJ { get; set; }
        public decimal? 实际供热量GJ { get; set; }
        public int 热力站个数 { get; set; }
        public int? 总面积 { get; set; }
        public int? 实际投入面积 { get; set; }
        public decimal? 万平米循环水量 { get; set; }
        public decimal? 总瞬时流量 { get; set; }
        public decimal? 总瞬时热量 { get; set; }
        public decimal? 平均热负荷 { get; set; }
        public decimal? 当前小时GJ { get; set; }
        public decimal? 上一小时GJ { get; set; }
        public decimal? 核算供热量GJ { get; set; }
        public decimal? 计划热指标 { get; set; }
        public decimal? 参考热指标 { get; set; }
        public decimal? 瞬时单耗 { get; set; }
        public decimal? 供暖季累计单耗 { get; set; }
        public decimal? 当日累计单耗 { get; set; }
        public bool IsToday { get; set; }
    }
}