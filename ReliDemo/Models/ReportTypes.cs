﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public enum ReportTypes
    {
        热源启停时间=1,
        供热天数比较表 = 2,
        供暖季室外温度比较=3,
        供暖季室外温度段统计=4,
        各月最低室温外温度比较=5,
        各月预报与实际室外温度统计=6,
        计算供热面积汇总表=7,
        热源供热量对比=8,
        热源供热量按成本划分对比=9,
        提前供热量汇总=10,
        延长供热量汇总=11,
        平均供热量统计=12,
        耗天然气量对比=13,
        燃气单耗对比=14,
        供气量对比=15,
        热源平均循环流量对比=16,
        热源平均回水温度与上一供暖季对比=17,
        热源实际供回水压力对比=18,
        流量供压及供水温度最大值汇总=19,
        各温度段房间数=20
    }
}