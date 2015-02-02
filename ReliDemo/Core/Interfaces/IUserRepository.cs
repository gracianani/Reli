using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Linq.Expressions;

namespace ReliDemo.Core.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<IUser> GetAllUsers();
        IUser FindByUserName(string userName);
        IUser Find(int userId);
        bool Update(IUser user);
        IQueryable<IUser> SearchFor(Expression<Func<IUser, bool>> predicate);
    }
}