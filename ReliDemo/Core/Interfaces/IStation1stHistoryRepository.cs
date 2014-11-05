using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using ReliDemo.Models;

namespace ReliDemo.Core.Interfaces
{
    public interface IStation1stHistoryRepository
    {
        IEnumerable<Station1stHistory> GetTopNStation1stHistories(int count);
        IQueryable<Station1stHistory> SearchFor(Expression<Func<Station1stHistory, bool>> predicate);
    }
}
