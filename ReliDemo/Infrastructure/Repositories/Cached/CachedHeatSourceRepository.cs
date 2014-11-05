using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Infrastructure.Repositories;
using System.Diagnostics;
using ReliDemo.Models;

namespace ReliDemo.Infrastructure.Repositories.Cached
{
    public class CachedHeatSourceRepository : HeatSourceRepository
    {
        private static readonly object CacheLockObject = new object();
        public override IEnumerable<HeatSource> GetAllHeatSources()
        {
            string cacheKey = "HeatSources";
            var result = HttpRuntime.Cache[cacheKey] as List<HeatSource>;
            if (result == null)
            {
                lock (CacheLockObject)
                {
                    result = HttpRuntime.Cache[cacheKey] as List<HeatSource>;
                    if (result == null)
                    {
                        result = base.GetAllHeatSources().ToList();
                        HttpRuntime.Cache.Insert(cacheKey, result, null,
                            DateTime.Now.AddSeconds(60), TimeSpan.Zero);
                    }
                }
            }
            return result;
        }
    }
}