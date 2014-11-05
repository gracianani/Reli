using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;

namespace ReliDemo.Core.Interfaces
{
    public interface IHeatSourceRepository
    {
        IEnumerable<HeatSource> GetAllHeatSources();
        IEnumerable<HeatSourceRecent> GetAllHeatSourceRecents();
        void Update(HeatSource heatSource);
    }
}