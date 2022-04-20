using Core.Models;
using Data.Abstract;
using Data.Concrete.Contexts;
using Entities;
using Microsoft.EntityFrameworkCore;
using static Core.Constants.BottleConstants;
using static Core.Constants.StationConstants;
using static Core.DTOs.StationLog;
using static Core.Extensions.Extensions;

namespace Data.Concrete.Repositories
{
    public class StationLogRepository : IStationLogRepository
    {
        private readonly BottleTrackingDbContext _dbContext;

        public StationLogRepository(BottleTrackingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(string trackingId, int kioskId)
        {
            var station = _dbContext.Stations.Where(x => x.Id == kioskId)
                                             .FirstOrDefault();
            var bottle = _dbContext.Bottles.Where(x => x.TrackingId == trackingId)
                                           .FirstOrDefault();

            var log = new StationLog
            {
                BottleId = bottle.Id,
                StationId = station.Id,
                Status = CheckBottleStatus(bottle)
            };

            _dbContext.StationLogs.Add(log);
            _dbContext.SaveChanges();
        }

        public PagedData<StationLogGetResponse> GetAll(RequestFilter filter)
        {
            var query = from logs in _dbContext.StationLogs
                        join stations in _dbContext.Stations.IgnoreQueryFilters()
                        on logs.StationId equals stations.Id
                        join bottles in _dbContext.Bottles.IgnoreQueryFilters()
                        on logs.BottleId equals bottles.Id
                        select new StationLogGetResponse
                        {
                            Id = logs.Id,
                            BottleProductionDate = bottles.ProductionDate,
                            BottleStatus = (UsageStatus)logs.Status,
                            Location = (Locations)stations.Location,
                            CreatedDate = logs.CreateDate,
                            ProductionLine = stations.ProductionLine,
                            TrackingId = bottles.TrackingId
                        };


            return query.Filter(ref filter)
                        .OrderBy(filter.Order.Field, filter.Order.IsDesc)
                        .Paginate(filter.PageNumber, filter.PageSize);

        }

        public StationLogStatistics GetStatistics()
        {
            var query = _dbContext.StationLogs;

            var totalQuery = query.Count();

            return new StationLogStatistics
            {
                TotalQuery = totalQuery,
                AverageOfDailyQuery = (totalQuery / (int)(DateTimeOffset.UtcNow.Date - query.Min(x => x.CreateDate.Date)).TotalDays),
                NumberOfTodayQuery = query.Where(x => x.CreateDate.Date == DateTimeOffset.UtcNow.Date)
                                            .Count(),
                NumberOfTodayBottle = query.Where(x => x.CreateDate.Date == DateTimeOffset.UtcNow.Date)
                                            .Select(x => x.BottleId)
                                            .Distinct().Count()
            };
        }

        private int CheckBottleStatus(Bottle bottle)
        {
            var bottleProductionDate = bottle.ProductionDate;
            var stationLog = _dbContext.StationLogs.Where(x => x.Bottle.TrackingId == bottle.TrackingId &&
                                                               x.CreateDate > bottleProductionDate)
                                                   .FirstOrDefault();
            int status = 0;
            var totalOfDaysBottleIsInUse = (DateTimeOffset.UtcNow.Date - bottleProductionDate.Date).TotalDays;

            if (totalOfDaysBottleIsInUse < BottleShelfLife)
            {
                status = (int)UsageStatus.Valid;
            }
            else if (stationLog != null)
            {
                status = (int)UsageStatus.Trash;
            }
            else if (stationLog == null || totalOfDaysBottleIsInUse == BottleShelfLife)
            {
                status = (int)UsageStatus.Expired;
            }

            return status;
        }
    }
}
