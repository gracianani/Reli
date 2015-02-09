using System;
using System.Collections.Generic;
using System.Data;
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

        public CompanyStat(IDataReader reader)
        {
            公司名 = reader.GetString(reader.GetOrdinal("公司"));
            回温超标站个数 = reader.IsDBNull(reader.GetOrdinal("回温超标站个数")) ? 0 : reader.GetInt32(reader.GetOrdinal("回温超标站个数"));
            实际超核算供热量站个数 = reader.IsDBNull(reader.GetOrdinal("超核算供热量站个数")) ? 0 : reader.GetInt32(reader.GetOrdinal("超核算供热量站个数"));
            实际超核算供热量站面积 = reader.IsDBNull(reader.GetOrdinal("超核算供热量站面积")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("超核算供热量站面积"));
            实际超计划供热量站个数 = reader.IsDBNull(reader.GetOrdinal("超计划供热量站个数")) ? 0 : reader.GetInt32(reader.GetOrdinal("超计划供热量站个数"));
            实际超计划供热量站面积 = reader.IsDBNull(reader.GetOrdinal("超计划供热量站面积")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("超计划供热量站面积"));
            有效监控站数 = reader.IsDBNull(reader.GetOrdinal("有效监控站数")) ? 0 : reader.GetInt32(reader.GetOrdinal("有效监控站数"));
            核算执行到位率 = reader.IsDBNull(reader.GetOrdinal("核算执行到位率")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("核算执行到位率"));
            监测站供热面积 = reader.IsDBNull(reader.GetOrdinal("监测站供热面积")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("监测站供热面积"));
            计划执行到位率 = reader.IsDBNull(reader.GetOrdinal("计划执行到位率")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("计划执行到位率"));
        }

        public CompanyStat()
        {

        }
    }
}