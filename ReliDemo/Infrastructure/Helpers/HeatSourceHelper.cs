using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Infrastructure.Helpers
{
    public class SimplifiedHeatSource
    {
        public int 生产热源ID { get; set; }
        public string 生产热源 { get; set; }
        public string 东西部 { get; set; }
        public bool 是否加入大网 { get; set; }
    }

    public class HeatSourceHelper
    {
        private static List<SimplifiedHeatSource> _heatSources = new List<SimplifiedHeatSource>()
        {
            new SimplifiedHeatSource() { 生产热源ID = 1, 生产热源 = "华能", 东西部="东部", 是否加入大网 = true },
            new SimplifiedHeatSource() { 生产热源ID = 22, 生产热源 = "京能", 东西部="西部", 是否加入大网 = true },
            new SimplifiedHeatSource() { 生产热源ID = 24, 生产热源 = "宝能", 东西部="西部", 是否加入大网 = false },
            new SimplifiedHeatSource() { 生产热源ID = 24, 生产热源 = "宝能永泰", 东西部="西部", 是否加入大网 = false }, //?
            new SimplifiedHeatSource() { 生产热源ID = 25, 生产热源 = "北郊", 东西部="东部", 是否加入大网 = false }, //?
            new SimplifiedHeatSource() { 生产热源ID = 28, 生产热源 = "东郊供热厂", 东西部="东部", 是否加入大网 = false },
            new SimplifiedHeatSource() { 生产热源ID = 31, 生产热源 = "西马供热厂", 东西部="西部", 是否加入大网 = false },
            new SimplifiedHeatSource() { 生产热源ID = 38, 生产热源 = "高井", 东西部="西部", 是否加入大网 = false },
            new SimplifiedHeatSource() { 生产热源ID = 52, 生产热源 = "松榆里供热厂", 东西部="东部", 是否加入大网 = false },
            new SimplifiedHeatSource() { 生产热源ID = 193, 生产热源 = "西直门锅炉房", 东西部="西部", 是否加入大网 = false },
            new SimplifiedHeatSource() { 生产热源ID = 297, 生产热源 = "科利源", 东西部="东部", 是否加入大网 = false }, //?
            new SimplifiedHeatSource() { 生产热源ID = 298, 生产热源 = "北清锅炉房站", 东西部="西部", 是否加入大网 = false },
            new SimplifiedHeatSource() { 生产热源ID = 302, 生产热源 = "国华汽网", 东西部="东部", 是否加入大网 = false }, //?
            new SimplifiedHeatSource() { 生产热源ID = 344, 生产热源 = "区域锅炉房", 东西部="东部", 是否加入大网 = false } //?
        };

        public static List<SimplifiedHeatSource> HeatSources
        {
            get
            {
                return _heatSources.Where( i=> i.是否加入大网 == true ).ToList();
            }
        }
    }
}