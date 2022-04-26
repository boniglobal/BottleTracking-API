using Entities;
using static Core.Constants.BottleConstants;
using Data.Concrete.Contexts;

namespace Data.Concrete
{
    public class BottleStatusHelper
    {
        public static int CheckBottleStatus(Bottle bottle, BottleTrackingDbContext _dbContext = null)
        {
            var bottleProductionDate = bottle.ProductionDate;
            var bottleExpirationDate = bottleProductionDate.AddDays(BottleShelfLife);
            var stationLog = _dbContext != null ? _dbContext.StationLogs
                                                            .Where(x => x.Bottle.TrackingId == bottle.TrackingId &&
                                                               x.CreateDate > bottleExpirationDate)
                                                            .FirstOrDefault() : null;

            int status = 0;
            var totalOfDaysBottleIsInUse = (DateTimeOffset.UtcNow.Date - bottleProductionDate.Date).TotalDays;

            if (totalOfDaysBottleIsInUse < BottleShelfLife)
            {
                status = (int)UsageStatus.Valid;
            }
            else
            {
                if (stationLog != null)
                {
                    status = (int)UsageStatus.Trash;
                }
                else
                {
                    status = (int)UsageStatus.Expired;
                }
            }

            return status;
        }
    }
}
