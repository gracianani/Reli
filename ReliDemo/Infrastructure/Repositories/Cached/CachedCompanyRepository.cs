using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;

namespace ReliDemo.Infrastructure.Repositories.Cached
{
    public class CachedCompanyRepository : CompanyRepository
    {
        private static readonly object CacheLockObject = new object();
        public override IEnumerable<Company> GetAllCompanies()
        {
            string cacheKey = "Companies";
            var result = HttpRuntime.Cache[cacheKey] as List<Company>;
            if (result == null)
            {
                lock (CacheLockObject)
                {
                    result = HttpRuntime.Cache[cacheKey] as List<Company>;
                    if (result == null)
                    {
                        result = base.GetAllCompanies().ToList();
                        HttpRuntime.Cache.Insert(cacheKey, result, null,
                            DateTime.Now.AddSeconds(60), TimeSpan.Zero);
                    }
                }
            }
            return result;
        }
    }
}