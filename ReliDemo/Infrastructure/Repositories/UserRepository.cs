using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;
using ReliDemo.Models;
using System.Linq.Expressions;

namespace ReliDemo.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private xz2013Entities db = new xz2013Entities();

        public virtual IEnumerable<IUser> GetAllUsers()
        {
            return db.Users;
        }

        public virtual IUser FindByUserName(string userName)
        {
            if (db.Users.Count(i => string.Compare(i.email, userName, true) == 0) > 0)
            {
                return db.Users.Single(i => i.email == userName);
            }
            else
            {
                return null;
            }
        }

        public virtual IUser Find(int userId)
        {
            return db.Users.Single(i => i.userId == userId);
        }

        public virtual IQueryable<IUser> SearchFor(Expression<Func<IUser, bool>> predicate)
        {
            return db.Users.Where(predicate);
        }

        public virtual bool Update(IUser user)
        {
            var dbUser = (User)user;
            db.Users.ApplyCurrentValues(dbUser);
            db.SaveChanges();
            return true;
        }
    }
}