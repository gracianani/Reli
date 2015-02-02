using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;

namespace ReliDemo.Infrastructure.Repositories.Cached
{
    public class CachedStationRepository : StationRepository
    {
        private static readonly object CacheLockObject = new object();
        public override IEnumerable<Station> GetAllStations()
        {
            string cacheKey = "Stations";
            var result = HttpRuntime.Cache[cacheKey] as List<Station>;
            if (result == null)
            {
                lock (CacheLockObject)
                {
                    result = HttpRuntime.Cache[cacheKey] as List<Station>;
                    if (result == null)
                    {
                        result = base.GetAllStations().ToList();
                        HttpRuntime.Cache.Insert(cacheKey, result, null,
                            DateTime.Now.AddSeconds(60), TimeSpan.Zero);
                    }
                }
            }
            return result;
        }
    }
}