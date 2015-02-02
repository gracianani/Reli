using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;
using ReliDemo.Models;
using System.Data;
using System.Linq.Expressions;

namespace ReliDemo.Infrastructure.Repositories
{
    public class StationRepository : IStationRepository
    {
        private xz2013Entities db = new xz2013Entities();

        public virtual IEnumerable<Station> GetAllStations()
        {
            return db.Stations.ToList();
        }

        public virtual void Insert(Station station)
        {
            db.Stations.AddObject(station);
            db.SaveChanges();
        }

        public virtual void Delete(Station station)
        {
            db.Stations.DeleteObject(station);
            db.SaveChanges();
        }

        public virtual void Update(Station station)
        {
            db.Stations.ApplyCurrentValues(station);
         //   db.Stations.Attach(station);
         //   db.ObjectStateManager.ChangeObjectState(station, EntityState.Modified);
            db.SaveChanges();
        }

        public virtual IQueryable<Station> SearchFor(Expression<Func<Station, bool>> predicate)
        {
            return db.Stations.Where(predicate);
        }

        public virtual IQueryable<StationManualInput> GetManualInput()
        {
            return db.StationManualInputs;
        }

        public virtual IQueryable<RoomTemperature> GetRoomTemperature()
        {
            return db.RoomTemperatures;
        }
    }
}