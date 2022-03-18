using Core.Models;
using Core.Extensions;
using Data.Abstract;
using Data.Concrete.Contexts;
using static Core.DTOs.Station;
using Entities;

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
                Location = item.Location,
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
            var query = from station in _dbContext.Stations
                        join user in _dbContext.PanelUsers
                        on station.PanelUserId equals user.Id
                        select new StationListView
                        {
                            Id = station.Id,
                            PanelUserId = station.PanelUserId,
                            CreateDate = station.CreateDate,
                            Location = station.Location,
                            ProductionLine = station.ProductionLine,
                            Fullname = user.Name + " " + user.Surname
                        };

            return query.OrderBy(filter.Order.Field, filter.Order.IsDesc)
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
                station.Location = data.Location;
                station.ProductionLine = data.ProductionLine;
                station.PanelUserId = data.PanelUserId;

                _dbContext.SaveChanges();
            }
        }
    }
}
