using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;
using ReliDemo.Models;
using System.Linq.Expressions;

namespace ReliDemo.Infrastructure.Repositories
{
    public class ManagershipRepository : IManagershipRepository
    {
        private xz2013Entities db = new xz2013Entities();

        public virtual IEnumerable<Managership> GetAllManagerships()
        {
            return db.Managerships.ToList();
        }

        public virtual IQueryable<Managership> SearchFor(Expression<Func<Managership, bool>> predicate)
        {
            return db.Managerships.Where(predicate);
        }

        public virtual Managership Find(int managershipId)
        {
            return db.Managerships.Single(i => i.ItemID == managershipId);
        }
    }
}