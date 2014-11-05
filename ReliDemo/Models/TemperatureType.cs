using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public enum TemperatureType
    {
        预测最高温度 = 1,
        预测最低温度 = 2,
        预测平均温度 = 3,
        实时温度 = 4,
        实际平均温度=5
    }
}