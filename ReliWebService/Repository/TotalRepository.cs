using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace ReliWebService.Repository
{
    public class TotalRepository
    {
        private xz2013Entities db = new xz2013Entities();
        private Overview _overview;
        public Overview Overview
        {
            get
            {
                return _overview;
            }
            set
            {
                _overview = value;
            }
        }

        public TotalRepository()
        {
            var totalNetRecents = db.TotalNetRecents.ToList();
            var 瞬时热量 = totalNetRecents.Select(i => (float)i.总瞬时热量).ToArray();
            var 面积 = totalNetRecents.Select(i => (float)i.总面积).ToArray();
            var 单耗 = totalNetRecents.Select(i => (float)i.当日累计单耗).ToArray();
            var 总流量 = totalNetRecents.Select(i => (float)i.总瞬时流量).ToArray();
            var 万平米流量 = totalNetRecents.Select(i => (float)i.万平方米流量).ToArray();
            var 今日累计 = totalNetRecents.Select(i => (float)i.今日累计GJ).ToArray();
            var 昨日累计 = totalNetRecents.Select(i => (float)i.昨日累计GJ).ToArray();
            float[] 今日预计 = totalNetRecents.Select(i=>(float)i.预计全天GJ).ToArray();
            var overview = new Overview(瞬时热量, 面积, 单耗, 总流量, 万平米流量, 今日累计, 昨日累计, 今日预计);
        }
    }
}