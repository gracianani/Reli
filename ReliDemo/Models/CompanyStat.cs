using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{

    public class CompanyStat
    {
        public string 公司名 { get; set; }
        public int 有效监控站数 { get; set; }
        public decimal 监测站供热面积 { get; set; }
        public int 回温超标站个数 { get; set; }
        public int 实际超核算供热量站个数 { get; set; }

        public decimal 实际超核算供热量站面积 { get; set; }
        public decimal 核算执行到位率 { get; set; }
        public int 实际超计划供热量站个数 { get; set; }
        public decimal 实际超计划供热量站面积 { get; set; }
        public decimal 计划执行到位率 { get; set; }

        public string GetDisplay(decimal? v, string append = "", string blank = "--")
        {
            if (v.HasValue)
            {
                return v.Value + append;
            }
            return blank;
        }
    }
}