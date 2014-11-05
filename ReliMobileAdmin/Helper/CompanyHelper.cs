using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliMobileAdmin.Helper
{
    public class CompanyHelper
    {
        public static string GetDepartmentId(int? companyId, bool? 是否为集团员工)
        {
            if (是否为集团员工.HasValue && 是否为集团员工.Value == true)
            {
                return
                    "集团员工";
            }
            switch (companyId ) {
                case 1 :
                return "创合供热公司";
                case 2:
                    return "科利源供热公司";
                case 4:
                    return "特力昆公司";
                case 5:
                    return "天禹供热公司";
                case 6 :
                    return "销售分公司";
            }
            return "";
        }

    }
}