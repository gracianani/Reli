using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ReliDemo.Models
{
    public class StationDetailReport
    {
        public string 热力站名称;
        public string 管理单位;
        public string 公司;
        public decimal 参考热指标;
        public string 数据来源;
        public bool 是否重点站;
        public string 收费性质;
        public string 生产热源;
        public int ItemID;
        public DateTime 日期;
        public int 热力站ID;
        public decimal 总热量GJ;
        public decimal 热水GJ;
        public decimal 计划GJ;
        public decimal 日单耗;
        public decimal 实际热指标;
        public decimal 核算热指标;
        public decimal 计划热指标;
        public decimal 投入面积;
        public decimal 实际面积;
        public decimal 预报温度;
        public decimal 实际温度;
        public decimal upHour;
        public decimal 核算GJ;
        public decimal 今日计划Area;
        public decimal 今日投入Area;
        public string 面积计划类别;
        public string 面积操作类型;
        public decimal 供温avg;
        public decimal 回温avg;
        public decimal 供压avg;
        public decimal 回压avg;
        public decimal 瞬热avg;
        public decimal 瞬流avg;
        public decimal 万流avg;

        internal StationDetailReport(IDataReader reader)
        {
            热力站名称 = reader.GetString(reader.GetOrdinal("热力站名称"));
            管理单位 = reader.GetString(reader.GetOrdinal("管理单位"));
            公司 = reader.GetString(reader.GetOrdinal("公司"));
            参考热指标 = reader.IsDBNull(reader.GetOrdinal("参考热指标")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("参考热指标"));
            数据来源 = reader.IsDBNull(reader.GetOrdinal("数据来源")) ? "" : reader.GetString(reader.GetOrdinal("数据来源"));
            是否重点站 = reader.IsDBNull(reader.GetOrdinal("是否重点站")) ? false : reader.GetBoolean(reader.GetOrdinal("是否重点站"));
            收费性质 = reader.IsDBNull(reader.GetOrdinal("收费性质")) ? "" : reader.GetString(reader.GetOrdinal("收费性质"));
            生产热源 = reader.GetString(reader.GetOrdinal("生产热源"));
            ItemID = reader.IsDBNull(reader.GetOrdinal("ItemID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ItemID"));
            日期 = reader.GetDateTime(reader.GetOrdinal("日期"));
            热力站ID = reader.GetInt32(reader.GetOrdinal("热力站ID"));
            总热量GJ = reader.IsDBNull(reader.GetOrdinal("采暖GJ")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("采暖GJ"));
            热水GJ = reader.IsDBNull(reader.GetOrdinal("热水GJ")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("热水GJ"));
            计划GJ = reader.IsDBNull(reader.GetOrdinal("计划GJ")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("计划GJ"));
            日单耗 = reader.IsDBNull(reader.GetOrdinal("日单耗")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("日单耗"));
            实际热指标 = reader.IsDBNull(reader.GetOrdinal("实际热指标")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("实际热指标"));
            核算热指标 = reader.IsDBNull(reader.GetOrdinal("核算热指标")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("核算热指标"));
            计划热指标 = reader.IsDBNull(reader.GetOrdinal("计划热指标")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("计划热指标"));
            投入面积 = reader.IsDBNull(reader.GetOrdinal("投入面积")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("投入面积"));
            实际面积 = reader.IsDBNull(reader.GetOrdinal("实际面积")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("实际面积"));
            预报温度 = reader.IsDBNull(reader.GetOrdinal("预报温度")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("预报温度"));
            实际温度 = reader.IsDBNull(reader.GetOrdinal("实际温度")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("实际温度"));
            upHour = reader.GetInt16(reader.GetOrdinal("upHour"));
            核算GJ = reader.IsDBNull(reader.GetOrdinal("核算GJ")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("核算GJ"));
            今日计划Area = reader.IsDBNull(reader.GetOrdinal("今日计划Area")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("今日计划Area"));
            今日投入Area = reader.IsDBNull(reader.GetOrdinal("今日投入Area")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("今日投入Area"));
            面积计划类别 = reader.IsDBNull(reader.GetOrdinal("面积计划类别")) ? "" : reader.GetString(reader.GetOrdinal("面积计划类别"));
            面积操作类型 = reader.IsDBNull(reader.GetOrdinal("面积操作类型")) ? "" : reader.GetString(reader.GetOrdinal("面积操作类型"));
            供温avg = reader.IsDBNull(reader.GetOrdinal("供温avg")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("供温avg"));
            回温avg = reader.IsDBNull(reader.GetOrdinal("回温avg")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("回温avg"));
            供压avg = reader.IsDBNull(reader.GetOrdinal("供压avg")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("供压avg"));
            回压avg = reader.IsDBNull(reader.GetOrdinal("回压avg")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("回压avg"));
            瞬热avg = reader.IsDBNull(reader.GetOrdinal("瞬热avg")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("瞬热avg"));
            瞬流avg = reader.IsDBNull(reader.GetOrdinal("瞬流avg")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("瞬流avg"));
            万流avg = reader.IsDBNull(reader.GetOrdinal("万流avg")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("万流avg"));
        }
    }
}