using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Linq.Expressions;

namespace ReliDemo.Core.Interfaces
{
    public interface IStationRepository
    {
        IEnumerable<Station> GetAllStations();
        void Insert(Station station);
        void Delete(Station station);
        void Update(Station station);
        IQueryable<Station> SearchFor(Expression<Func<Station, bool>> predicate);
        IQueryable<StationManualInput> GetManualInput();
        IQueryable<RoomTemperature> GetRoomTemperature();
    }
}