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

        public void Add(StationLogAdd log, Bottle bottle, int stationId)
        {
            _dbContext.StationLogs.Add(new StationLog
            {
                BottleId = bottle.Id,
                StationId = stationId,
                Status = BottleStatusHelper.CheckBottleStatus(bottle, _dbContext),
                DistributorId = log.DistributorId,
                DistributionRegion = log.DistributionRegion
            });
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
                            CreatedDate = logs.CreateDate.AddHours(3),
                            ProductionLine = stations.ProductionLine,
                            TrackingId = bottles.TrackingId,
                            DistributorId = logs.DistributorId,
                            DistributionRegion = logs.DistributionRegion
                        };

            return query.Filter(ref filter)
                        .OrderBy(filter.Order.Field, filter.Order.IsDesc)
                        .Paginate(filter.PageNumber, filter.PageSize);

        }

        public StationLogStatistics GetStatistics()
        {
            var query = _dbContext.StationLogs;

            var totalQuery = query.Count();

            var numberOfDays = (int)(DateTimeOffset.UtcNow.Date - query.Min(x => x.CreateDate.Date)).TotalDays;
            numberOfDays = numberOfDays == 0 ? 1 : numberOfDays;

            return new StationLogStatistics
            {
                TotalQuery = totalQuery,
                AverageOfDailyQuery = (int)Math.Ceiling(decimal.Divide(totalQuery, numberOfDays)),
                NumberOfTodayQuery = query.Where(x => x.CreateDate.Date == DateTimeOffset.UtcNow.Date)
                                            .Count(),
                NumberOfTodayBottle = query.Where(x => x.CreateDate.Date == DateTimeOffset.UtcNow.Date)
                                            .Select(x => x.BottleId)
                                            .Distinct().Count()
            };
        }
    }
}
