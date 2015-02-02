using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public enum HeatConsumptionGraphType
    {
        Total = 1,
        Company = 2,
        Managership = 3,
        Station = 4
    }

    public enum HeatConsumptionGraphSpan
    {
        Dayly = 1,
        Weekly = 2,
        Monthly = 3,
        Seaonal = 4
    }
}