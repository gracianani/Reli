using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReliDemo.Models
{
    public interface ITemperatureGraphItem
    {
        decimal? forecastHighest { get;}
        decimal? forecastLowest { get;  }
        decimal? forecastAverage { get; }
        decimal? actualAverage { get; }
        decimal? actualHighest { get; }
        decimal? actualLowest { get; }
        WeatherTypes weatherType { get;  }
        DateTime 日期 { get; }
        string windSpeedAndDirection { get; }
    }
}
