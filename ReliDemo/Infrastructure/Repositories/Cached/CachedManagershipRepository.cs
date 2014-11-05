using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;

namespace ReliDemo.Infrastructure.Repositories.Cached
{
    public class CachedManagershipRepository : ManagershipRepository
    {
        private static readonly object CacheLockObject = new object();
        public override IEnumerable<Managership> GetAllManagerships()
        {
            string cacheKey = "Managerships";
            var result = HttpRuntime.Cache[cacheKey] as List<Managership>;
            if (result == null)
            {
                lock (CacheLockObject)
                {
                    result = HttpRuntime.Cache[cacheKey] as List<Managership>;
                    if (result == null)
                    {
                        result = base.GetAllManagerships().ToList();
                        HttpRuntime.Cache.Insert(cacheKey, result, null,
                            DateTime.Now.AddSeconds(60), TimeSpan.Zero);
                    }
                }
            }
            return result;
        }
    }
}