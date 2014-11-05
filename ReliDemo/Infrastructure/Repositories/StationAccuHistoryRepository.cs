using ReliDemo.Core.Interfaces;
using ReliDemo.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReliDemo.Infrastructure.Repositories
{
    public class StationAccuHistoryRepository : IStationAccuHistoryRepository
    {
        private xz2013Entities db = new xz2013Entities();

        public IEnumerable<StationAccuHistory> GetTopNStationAccuHistories(int count)
        {
            return db.StationAccuHistories.ToList().Take(count);
        }

        public IQueryable<StationAccuHistory> SearchFor(System.Linq.Expressions.Expression<System.Func<StationAccuHistory, bool>> predicate)
        {
            return db.StationAccuHistories.Where(predicate);
        }
    }
}