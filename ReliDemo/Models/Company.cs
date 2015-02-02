using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Infrastructure.Services;

namespace ReliDemo.Models
{
    public partial class Company
    {
        public int 热力站个数 { get { return this.Stations.Count(); } }

        public int 超标站个数 { 
            get { 
                return 
                    this.Stations.Count(i => i.一次回温 > ConfigurationService.Instance.TemperatureExceed ); 
            } 
        }

        public int 温度超标站个数 { 
            get { 
                return 
                    this.Stations.Count(i => i.一次回温 > ConfigurationService.Instance.TemperatureExceed); 
            } 
        }
    }
}