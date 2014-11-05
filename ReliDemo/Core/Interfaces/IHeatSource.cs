using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReliDemo.Core.Interfaces
{
    public interface IHeatSource
    {
        int HeatSourceId { get; set; }
        string HeatSourceName { get; set; }
        decimal? TemperatureIn { get; set; }
        decimal? TemperatureOut { get; set; }
        decimal? PressureIn { get; set; }
        decimal? PressureOut { get; set; }
        DateTime? LastUpdatedAt { get; set; }
    }

}
