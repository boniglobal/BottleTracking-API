﻿using Core.Models;
using Data.Abstract;
using Data.Concrete.Contexts;
using Entities;
using static Core.Constants.BottleConstants;
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
                        join stations in _dbContext.Stations
                        on logs.StationId equals stations.Id
                        join bottles in _dbContext.Bottles
                        on logs.BottleId equals bottles.Id
                        select new StationLogGetResponse
                        {
                            Id = logs.Id,
                            BottleProductionDate = bottles.ProductionDate,
                            BottleStatus = (UsageStatus)logs.Status,
                            Location = stations.Location,
                            CreatedDate = logs.CreateDate,
                            ProductionLine = stations.ProductionLine,
                            TrackingId = bottles.TrackingId
                        };


            return query.OrderBy(filter.Order.Field, filter.Order.IsDesc)
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
                                                               x.CreateDate.Date < DateTimeOffset.UtcNow.Date)
                                                   .FirstOrDefault();
            int status = 0;

            if ((DateTimeOffset.UtcNow.Date - bottleProductionDate).TotalDays < BottleShelfLife)
            {
                status = (int)UsageStatus.Valid;
            }
            else if ((DateTimeOffset.UtcNow.Date - bottleProductionDate).TotalDays > BottleShelfLife && stationLog != null)
            {
                status = (int)UsageStatus.Trash;
            }
            else if ((DateTimeOffset.UtcNow.Date - bottleProductionDate).TotalDays > BottleShelfLife && stationLog == null)
            {
                status = (int)UsageStatus.Expired;
            }

            return status;
        }
    }
}
