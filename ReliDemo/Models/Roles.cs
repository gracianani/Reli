using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliDemo.Models
{
    [DataContract]
    public enum Role
    {
        系统管理员 = 1,
        生产部调度 = 2,
        分公司调度 = 3,
        供热中心调度= 4,
        抄表填报 = 5,
        系统维护员=6
    }
}