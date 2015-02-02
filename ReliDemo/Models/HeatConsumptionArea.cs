using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public class HeatConsumptionArea
    {
        public decimal? 当日计划供热总面积 { get; set; }
        public decimal? 当日实际供热总面积 { get; set; }
        public decimal? 前日计划供热总面积 { get; set; }
        public decimal? 前日实际供热总面积 { get; set; }

        public decimal? 总供热面积 { get; set; }
        public decimal? 计算用监控面积 { get; set; }
        public decimal? 当日计划投入面积 { get; set; }
        public decimal? 当日实际投入面积 { get; set; }
        public decimal? 当日计划停热面积 { get; set; }
        public decimal? 当日实际停热面积 { get; set; }
        public decimal? 前日计划投入面积 { get; set; }
        public decimal? 前日实际投入面积 { get; set; }
        public decimal? 前日计划停热面积 { get; set; }
        public decimal? 前日实际停热面积 { get; set; }
        
        public bool 当日计划是否投 { get; set; }
        public bool 当日实际是否投 { get; set; }
        public bool 前日计划是否投 { get; set; }
        public bool 前日实际是否投 { get; set; }
    }
}