using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using ReliDemo.Models;
using ReliDemo.Core.Interfaces;

namespace ReliDemo.Infrastructure.Repositories
{
    public class HeatSourceRepository : IHeatSourceRepository
    {
        private xz2013Entities db = new xz2013Entities();

        public virtual IEnumerable<HeatSource> GetAllHeatSources()
        {
            return db.HeatSources.ToList();
        }

        public virtual IEnumerable<HeatSourceRecent> GetAllHeatSourceRecents()
        {
            return db.HeatSourceRecents.ToList();
        }

        public virtual void Update(HeatSource heatSource)
        {
            db.HeatSources.ApplyCurrentValues(heatSource);
            db.SaveChanges();
        }

    }
}