using ReliDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Infrastructure.Helpers
{
    public class ChargeHelper
    {
        public static List<string> AllCharge
        {
            get
            {
                return new List<string>() {
                    "热水智能卡",
                    "户用热计量",
                    "混合计量智能卡",
                    "采暖",
                    "计量",
                    "热水",
                    "面积智能卡",
                    "蒸汽",
                    "计量智能卡",
                    "采暖趸售"
                };
            }
        }

        public static IList<IdAndName> 是否重点站
        {
            get
            {
                return new List<IdAndName>() {
                    new IdAndName() { Id=2, Name="全部站（重点站及非重点站）" },
                    new IdAndName() { Id=1, Name="是" },
                    new IdAndName() { Id=0, Name="否" }
                };
            }
        }

        public static List<KeyAndValue> 含所有收费性质
        {
            get
            {
                return new List<KeyAndValue>() {
                    new KeyAndValue() { Key="ALL", Value="全部收费性质" },
                    new KeyAndValue() { Key="热水智能卡", Value="热水智能卡" },
                    new KeyAndValue() { Key="户用热计量", Value="户用热计量" },
                    new KeyAndValue() { Key="混合计量智能卡", Value="混合计量智能卡" },
                    new KeyAndValue() { Key="采暖", Value="采暖" },
                    new KeyAndValue() { Key="计量", Value="计量" },
                    new KeyAndValue() { Key="热水", Value="热水" },
                    new KeyAndValue() { Key="面积智能卡", Value="面积智能卡" },
                    new KeyAndValue() { Key="蒸汽", Value="蒸汽" },
                    new KeyAndValue() { Key="计量智能卡", Value="计量智能卡" },
                    new KeyAndValue() { Key="采暖趸售", Value="采暖趸售" }
                };
            }
        }

        public static List<KeyAndValue> 含所有热源
        {
            get
            {
                return new List<KeyAndValue>() {
                    new KeyAndValue() { Key="ALL", Value="全部热源" },
                    new KeyAndValue() { Key="华能", Value="华能" },
                    new KeyAndValue() { Key="京能", Value="京能" },
                    new KeyAndValue() { Key="西马供热厂", Value="西马供热厂" },
                    new KeyAndValue() { Key="宝能", Value="宝能" },
                    new KeyAndValue() { Key="宝能永泰", Value="宝能永泰" },
                    new KeyAndValue() { Key="高井", Value="高井" },
                    new KeyAndValue() { Key="国华汽网", Value="国华汽网" },
                    new KeyAndValue() { Key="松榆里供热厂", Value="松榆里供热厂" },
                    new KeyAndValue() { Key="科利源", Value="科利源" },
                    new KeyAndValue() { Key="区域锅炉房", Value="区域锅炉房" },
                    new KeyAndValue() { Key="东郊供热厂", Value="东郊供热厂" },
                    new KeyAndValue() { Key="北郊", Value="北郊" },
                    new KeyAndValue() { Key="北清锅炉房站", Value="北清锅炉房站" },
                    new KeyAndValue() { Key="西直门锅炉房", Value="西直门锅炉房" }
                };
            }
        }

        public static List<KeyAndValue> 含所有公司
        {
            get
            {
                return new List<KeyAndValue>() {
                    new KeyAndValue() { Key="ALL", Value="全部公司" },
                    new KeyAndValue() { Key="销售", Value="销售" },
                    new KeyAndValue() { Key="创合", Value="创合" },
                    new KeyAndValue() { Key="特力昆", Value="特力昆" },
                    new KeyAndValue() { Key="天禹", Value="天禹" }
                };
            }
        }

        public static List<KeyAndValue> 含所有数据来源
        {
            get
            {
                return new List<KeyAndValue>() {
                    new KeyAndValue() { Key="ALL", Value="全部数据来源" },
                    new KeyAndValue() { Key="监控", Value="监控" },
                    new KeyAndValue() { Key="智能卡", Value="智能卡" },
                    new KeyAndValue() { Key=null, Value="人工抄表" }
                };
            }
        }
        
    }
}