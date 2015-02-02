using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;
using ReliDemo.Models;

namespace ReliDemo.Infrastructure.Repositories
{
    public class Station1stHistoryRepository : IStation1stHistoryRepository
    {
        private xz2013Entities db = new xz2013Entities();
        public IEnumerable<Station1stHistory> GetTopNStation1stHistories(int count)
        {
            return db.Station1stHistory.ToList().Take(count);
        }

        public IQueryable<Station1stHistory> SearchFor(System.Linq.Expressions.Expression<Func<Station1stHistory, bool>> predicate)
        {
            return db.Station1stHistory.Where(predicate);
        }
    }
}