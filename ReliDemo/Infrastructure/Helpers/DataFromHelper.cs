using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Infrastructure.Helpers
{
    public class DataFromHelper
    {
        public static List<string> 数据来源 {
            get
            {
                return new List<string>() {
                    "智能卡", "监控", "人工抄表"
                };
            }
        }
    }
}