using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;

namespace ReliDemo.Infrastructure.Services
{
    public class HeatSourcesService
    {
        private xz2013Entities db = new xz2013Entities();

        public IEnumerable<HeatSourceAccuHistory> GetHeatSourceHistory(int heatSourceId, DateTime from, DateTime to)
        {
            return db.HeatSourceAccuHistories.Where(i => i.生产热源ID == heatSourceId && i.日期 >= from && i.日期 <= to);
        }

        public IEnumerable<HeatSourceRecent> GetHeatSourcesRealTime(DateTime from, DateTime to)
        {
            return db.HeatSourceRecents.Where(i =>i.HeatSource.是否并网供热 == true && i.采集时间 > from && i.采集时间 < to);
        }

        public IEnumerable<HeatSourceRecent> GetHeatSourcesRealTimeByRegion(HeatSourceRegions region)
        {
            var result = new List<HeatSourceRecent>();
            switch (region)
            {
                case HeatSourceRegions.东部:
                    result = db.HeatSourceRecents.Where(i => i.HeatSource.是否并网供热 == true && i.HeatSource.东西部 == "东部").ToList();
                    break;
                case HeatSourceRegions.西部:
                    result = db.HeatSourceRecents.Where(i => i.HeatSource.是否并网供热 == true && i.HeatSource.东西部 == "西部").ToList();
                    break;
                case HeatSourceRegions.全网:
                    result = db.HeatSourceRecents.Where(i => i.HeatSource.是否并网供热 == true).ToList();
                    break;
                case HeatSourceRegions.独网:
                    result = db.HeatSourceRecents.Where(i => i.HeatSource.是否并网供热 == false).ToList();
                    break;
            }

            return result;
        }

        public IEnumerable<IdAndName> GetDisplyHeatSources()
        {
            return new List<IdAndName>() { new IdAndName() { Id = 1, Name = "华能" }, new IdAndName() { Id = 22, Name = "京能" } };
        }


    }
}