using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;
using System.Runtime.Serialization;
using ReliDemo.Models;
using System.Data.Objects.DataClasses;

namespace ReliWebService
{
    [KnownType(typeof(IUser))]
    [KnownType(typeof(Role))]
    [KnownType(typeof(List<Role>))]
    [DataContract]
    public class ReliMobileUser : IUser
    {
        private user _dbUser;
        public user DBUser
        {
            get
            {
                if (_dbUser == null)
                {
                    _dbUser = new user();
                }
                return _dbUser;
            }
            set
            {
                _dbUser = value;
            }
        }

        [DataMember]
        public string userName {
            get { return _dbUser.email; }
            set { }
        }

        public string FullName
        {
            get
            {
                return _dbUser.姓名;
            }
            set
            {
            }
        }

        public List<Role> Roles
        {
            get
            {
                return _dbUser.roles.Select(i=>(Role)i.RoleId).ToList();
            }
        }

        public int UserId
        {
            get { return userId; }
        }

        public string UserName
        {
            get
            {
                return userName;
            }
        }
        [DataMember]
        public string roles
        {
            get
            {
                return string.Join(",", Roles.ToList());
            }
            set
            {
            }
        }

        [DataMember]
        public int userId
        {
            get
            {
                return _dbUser.userId;
            }
            set
            {
                
            }
        }

        private string _menu;
        [DataMember]
        public string menu
        {
            get
            {
                return string.Join("", VisibleHomeScreenBlocks);
            }
            set
            {
                _menu = value;
            }
        }
        public List<int> VisibleHomeScreenBlocks {
            get
            {
                if (Roles.Contains(Role.生产部调度))
                {
                    return new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
                }
                else if (Roles.Contains(Role.分公司调度))
                {
                    return new List<int> { 1, 4, 5, 6 };
                }
                else if (Roles.Contains(Role.供热中心调度))
                {
                    return new List<int> { 1, 4, 5, 6 };
                }
                else
                {
                    return new List<int> { 1, 2, 3, 4, 5, 6, 7, 8};
                }
            }
        }
        public ReliMobileUser(user dbUser)
        {
            _dbUser = dbUser;
        }
        public ReliMobileUser()
        {
        }
    }
}