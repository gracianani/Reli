using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public partial class StationAccuHistory
    {
        public decimal? 实际比核算多耗
        {
            get {
                return 采暖GJ - 核算GJ;
            }
        }

        public decimal? 实际比计划多耗
        {
            get
            {
                return  采暖GJ - 计划GJ;
            }
        }

        public decimal? 实际比核算多耗percent
        {
            get
            {
                return (核算GJ.HasValue &&核算GJ.Value >0　 ? 实际比核算多耗/核算GJ : 0.0m ) * 100.0m;
            }
        }

        public decimal? 实际比计划多耗percent
        {
            get
            {
                return (计划GJ.HasValue && 计划GJ.Value > 0 ? 实际比计划多耗 / 计划GJ : 0.0m) * 100.0m;
            }
        }

        public string 公司 {get;set;}
        public string 管理单位 {get;set;}
        public string 热源 {get;set;}
        public string 热力站名称 { get; set; }
        public string 收费性质 { get; set; }
        public int 是否重点站 { get; set; }
        public string 数据来源 { get; set; }
        public decimal 参考热指标 { get; set; }
        public decimal 计划的多耗 { get; set; }
        public decimal 核算的多耗 { get; set; }
        public StationAccuHistory(IDataReader reader)
        {
            this._热力站ID = reader.GetInt32(reader.GetOrdinal("热力站ID"));
            this._日期 = reader.GetDateTime(reader.GetOrdinal("日期"));
            this._计划GJ = reader.IsDBNull(reader.GetOrdinal("计划GJ")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("计划GJ"));
            this._核算GJ = reader.IsDBNull(reader.GetOrdinal("核算GJ")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("核算GJ"));
            this._采暖GJ = reader.IsDBNull(reader.GetOrdinal("采暖GJ")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("采暖GJ"));
            this._热水GJ = reader.IsDBNull(reader.GetOrdinal("热水GJ")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("热水GJ"));
            this._计划热指标 = reader.IsDBNull(reader.GetOrdinal("计划热指标")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("计划热指标"));
            this._核算热指标 = reader.IsDBNull(reader.GetOrdinal("核算热指标")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("核算热指标"));
            this._实际热指标 = reader.IsDBNull(reader.GetOrdinal("实际热指标")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("实际热指标"));
            this._投入面积 = reader.IsDBNull(reader.GetOrdinal("投入面积")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("投入面积"));
            this._实际面积 = reader.IsDBNull(reader.GetOrdinal("实际面积")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("实际面积"));
            this._预报温度 = reader.IsDBNull(reader.GetOrdinal("预报温度")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("预报温度"));
            this._实际温度 = reader.IsDBNull(reader.GetOrdinal("实际温度")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("实际温度"));
            this._今日计划Area = reader.IsDBNull(reader.GetOrdinal("今日计划Area")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("今日计划Area"));
            this._今日投入Area = reader.IsDBNull(reader.GetOrdinal("今日投入Area")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("今日投入Area"));
            this.公司 = reader.GetString(reader.GetOrdinal("公司"));
            this.管理单位 = reader.GetString(reader.GetOrdinal("分公司"));
            this.热源 = reader.GetString(reader.GetOrdinal("生产热源"));
            this.热力站名称 = reader.GetString(reader.GetOrdinal("热力站名称"));
            this.收费性质 = reader.GetString(reader.GetOrdinal("收费性质"));
            this.数据来源 = reader.IsDBNull(reader.GetOrdinal("数据来源")) ? "" : reader.GetString(reader.GetOrdinal("数据来源"));
            this.是否重点站 = Convert.ToInt32( reader.GetBoolean(reader.GetOrdinal("是否重点站")) );
            this.参考热指标 = reader.GetDecimal(reader.GetOrdinal("参考热指标"));
            this.计划的多耗 = reader.IsDBNull(reader.GetOrdinal("计划多耗")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("计划多耗"));
            this.核算的多耗 = reader.IsDBNull(reader.GetOrdinal("核算多耗")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("核算多耗"));
        }

        public StationAccuHistory()
        { }
    }
}