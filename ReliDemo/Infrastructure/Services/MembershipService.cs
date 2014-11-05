using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using ReliDemo.Infrastructure.Repositories;

namespace ReliDemo.Infrastructure.Services
{
    public class MembershipService
    {
        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var userName = HttpContext.Current.User.Identity.Name;
                    var userRepo = new UserRepository();
                    var user = userRepo.FindByUserName(userName);
                    return (User) user;
                }
                return null;
            }
        }
    }
}