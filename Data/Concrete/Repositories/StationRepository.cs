using Core.Models;
using Core.Extensions;
using Data.Abstract;
using Data.Concrete.Contexts;
using static Core.DTOs.Station;
using Entities;
using static Core.Constants.StationConstants;
using Microsoft.EntityFrameworkCore;

namespace Data.Concrete.Repositories
{
    public class StationRepository : IStationRepository
    {
        private readonly BottleTrackingDbContext _dbContext;

        public StationRepository(BottleTrackingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(StationAdd item)
        {
            var station = new Station
            {
                Location = (int)item.Location,
                PanelUserId = item.PanelUserId,
                ProductionLine = item.ProductionLine,
                Deleted = false
            };

            _dbContext.Stations.Add(station);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var station = _dbContext.Stations.FirstOrDefault(s => s.Id == id);
            if (station != null)
            {
                station.Deleted = true;
                _dbContext.SaveChanges();
            }
        }

        public PagedData<StationListView> GetAll(RequestFilter filter)
        {
            var query = from station in _dbContext.Stations.Where(x => !x.Deleted)
                        join user in _dbContext.PanelUsers.IgnoreQueryFilters()
                        on station.PanelUserId equals user.Id
                        select new StationListView
                        {
                            Id = station.Id,
                            PanelUserId = !user.Deleted ? station.PanelUserId : null,
                            CreateDate = station.CreateDate,
                            Location = (Locations)station.Location,
                            ProductionLine = station.ProductionLine,
                            Fullname = !user.Deleted ? user.Name + " " + user.Surname : null
                        };

            return query.Filter(ref filter)
                        .OrderBy(filter.Order.Field, filter.Order.IsDesc)
                        .Paginate(filter.PageNumber, filter.PageSize);
        }

        public Station GetByPanelUserId(int id)
        {
            return _dbContext.Stations.Where(x => x.PanelUserId == id).FirstOrDefault();
        }

        public StationStatistics GetStatistics()
        {
            var query = _dbContext.Stations;

            return new StationStatistics
            {
                TotalNumberOfLines = query.Select(x => x.ProductionLine).Distinct().Count(),
                TotalNumberOfStations = query.Count()
            };
        }

        public void Update(StationUpdate data)
        {
            var station = _dbContext.Stations.Where(x => x.Id == data.StationId).FirstOrDefault();
            if (station != null)
            {
                station.Location = (int)data.Location;
                station.ProductionLine = data.ProductionLine;
                station.PanelUserId = data.PanelUserId;

                _dbContext.SaveChanges();
            }
        }
    }
}
