﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Infrastructure.Services;
using ReliDemo.Models;

namespace ReliDemo.ViewModels
{
    public class ConfigurationViewModel
    {
        public decimal 回温标准线
        {
            get
            {
                return ConfigurationService.Instance.TemperatureExceed;
            }
        }

        public DateTime 报表默认开始时间
        {
            get
            {
                return ConfigurationService.Instance.报表默认开始时间;
            }
        }
        public DateTime 报表默认结束时间
        {
            get
            {
                return ConfigurationService.Instance.报表默认结束时间;
            }
        }
        private IEnumerable<TemperatureAudit> _操作记录;
        public IEnumerable<TemperatureAudit> 操作记录
        {
            get
            {
                if (_操作记录 == null)
                {
                    _操作记录 = new List<TemperatureAudit>();
                }
                return _操作记录;
            }
            set
            {
                _操作记录 = value;
            }
        }
    }
}