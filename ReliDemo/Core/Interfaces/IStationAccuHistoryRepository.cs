using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReliDemo.Models;
using System.Linq.Expressions;

namespace ReliDemo.Core.Interfaces
{
    public interface IStationAccuHistoryRepository
    {
        IEnumerable<StationAccuHistory> GetTopNStationAccuHistories(int count);
        IQueryable<StationAccuHistory> SearchFor(Expression<Func<StationAccuHistory, bool>> predicate);
    }
}
