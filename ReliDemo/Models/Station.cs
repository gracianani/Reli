
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ReliDemo.Infrastructure.Helpers;
using ReliDemo.Infrastructure.Services;
using System.Data;

namespace ReliDemo.Models
{
    public partial class Station
    {
        public Station()
        {

        }
        public Station(IDataReader reader) {

        }
        public int StationId { get { return ItemID; }  }
        public string StationName { get; set; }
        public string StationCode { get; set; } // 热力站编号
        public string StationAbbr  { get; set; } // 热力站编码
        public string StationAddress { get; set; }
 
        public string 东西部
        {
            get
            {
                try
                {
                    return HeatSourceHelper.HeatSources.Find(i => i.生产热源ID == 生产热源ID).东西部;
                }
                catch
                {
                    return "";
                }
            }
        }

        public int? 数据来源ID
        {
            get
            {
                return StationsService.Get数据来源ID(_数据来源);
            }
        }

        public bool 是否为大网
        {
            get
            {
                return 生产热源ID == 1 || 生产热源ID == 22;
            }
        }
    }
}