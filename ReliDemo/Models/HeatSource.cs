using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;

namespace ReliDemo.Models
{
    public partial class HeatSource : IHeatSource 

    {
        public int HeatSourceId
        {
            get
            {
                return this.ItemID;
            }
            set
            {
            }
        }

        public string HeatSourceName
        {
            get
            {
                return this.热源名称;
            }
            set
            {
            }
        }

        public decimal? TemperatureIn
        {
            get
            {
                return this.HeatSourceRecents.OrderByDescending(i => i.采集时间).First().回温;
            }
            set
            {
            }
        }

        public decimal? TemperatureOut
        {
            get
            {
                return this.HeatSourceRecents.OrderByDescending(i => i.采集时间).First().供温;
            }
            set
            {
            }
        }

        public decimal? PressureIn
        {
            get
            {
                return this.HeatSourceRecents.OrderByDescending(i => i.采集时间).First().回压;
            }
            set
            {
            }
        }

        public decimal? PressureOut
        {
            get
            {
                return this.HeatSourceRecents.OrderByDescending(i => i.采集时间).First().供压;
            }
            set
            {
            }
        }

        public DateTime? LastUpdatedAt
        {
            get
            {
                return this.HeatSourceRecents.OrderByDescending(i => i.采集时间).First().采集时间;
            }
            set
            {
            }
        }
    }
}