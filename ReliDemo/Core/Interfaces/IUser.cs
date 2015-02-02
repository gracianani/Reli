using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Runtime.Serialization;

namespace ReliDemo.Core.Interfaces
{
    public interface IUser
    {
        int UserId { get;}
        string UserName { get;  }
        string FullName { get; }
        List<Role> Roles { get;}
        //IList<string> 权限 { get; }
        //string 权限们 { get; }
        //List<IdAndName> VisibleManagerships;
        //List<IdAndName> VisibleCompanies;
        //bool 是否有分公司权限 { get; }
        //bool 是否有供热中心权限 { get; }
        //bool 是否有集团权限 { get; }
        //bool 是否可以设置权限 { get; }
        //string LandingPage { get; }
    }
}