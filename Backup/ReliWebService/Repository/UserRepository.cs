using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using ReliDemo.Core.Interfaces;
using ReliDemo.Models;

namespace ReliWebService.Repository
{
    public class UserRepository : IUserRepository
    {
        private ReliMobileEntities _db = new ReliMobileEntities();
        private List<ReliMobileUser> _users;

        public List<ReliMobileUser> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        public UserRepository()
        {
            _users = new List<ReliMobileUser>();
            _users = _db.users.Select(i=> new ReliMobileUser() { DBUser = i }).ToList();
        }

        public IEnumerable<IUser> GetAllUsers()
        {
            return _users;
        }

        public IUser FindByUserName(string userName)
        {
            return _users.Find(i => string.Compare(i.userName, userName, true) == 0);
        }

        public IUser Find(int userId)
        {
            return _users.Find(i => i.userId == userId);
        }

        public bool Update(IUser user)
        {
            return false;
        }

        public IQueryable<IUser> SearchFor(System.Linq.Expressions.Expression<Func<IUser, bool>> predicate)
        {
            return _users.AsQueryable();
        }

    }
}