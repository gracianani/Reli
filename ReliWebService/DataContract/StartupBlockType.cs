using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliWebService
{
    public enum StartupBlockType
    {
        DailyReport = 1,
        Warning = 2,
        Weather = 3,
        Message = 4,
        HeatSource = 5,
        Station = 6
    }
}