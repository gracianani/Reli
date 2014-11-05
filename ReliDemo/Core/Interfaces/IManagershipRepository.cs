using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Linq.Expressions;

namespace ReliDemo.Core.Interfaces
{
    public interface IManagershipRepository
    {
        IEnumerable<Managership> GetAllManagerships();
        IQueryable<Managership> SearchFor(Expression<Func<Managership, bool>> predicate);
        Managership Find(int managershipId);
    }
}