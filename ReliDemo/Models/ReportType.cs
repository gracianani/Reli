﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public enum ReportType
    {
        热耗统计明细 = 1,
        热耗统计汇总 = 2,
        生产日报=3,
        一日一站一计划日报 = 21,
        天气预报历史 = 22,
        非重点站一站一日一计划 = 23,
        回温超标明细 = 24,
        实际超核算明细 = 25,
        总明细 = 26,
        故障明细 = 27,
        按时间段到位率对比 = 28,
        站执行到位率统计 = 29,
        各单位执行到位率统计_日 = 30,
        各单位执行到位率统计_汇总  = 32
    }
}