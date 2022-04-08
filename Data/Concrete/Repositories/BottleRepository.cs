using Core.Models;
using Data.Abstract;
using Data.Concrete.Contexts;
using static Core.Constants.BottleConstants;
using static Core.DTOs.Bottle;
using Core.Extensions;
using Entities;
using System.Globalization;

namespace Data.Concrete.Repositories
{
    public class BottleRepository : IBottleRepository
    {
        private readonly BottleTrackingDbContext _dbContext;

        public BottleRepository(BottleTrackingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BottleView GetById(int id)
        {
            return _dbContext.Bottles.Where(x => x.Id == id).Select(x => new BottleView
            {
                Id = x.Id,
                ProductionDate = x.ProductionDate,
                RefillCount = x.RefillCount,
                LastRefillDate = x.LastRefillDate,
                BottleType = x.BottleType,
                QrCode = x.QrCode,
                QrPrintCount = x.QrPrintCount,
                Status = (UsageStatus)x.Status,
                CreateDate = x.CreateDate,
                TrackingId = x.TrackingId
            }).FirstOrDefault();
        }

        public PagedData<BottleView> GetAll(RequestFilter filter)
        {
            return _dbContext.Bottles.Filter(ref filter)
                                     .OrderBy(filter.Order.Field, filter.Order.IsDesc)
                                     .Select(x => new BottleView
                                     {
                                         Id = x.Id,
                                         ProductionDate = x.ProductionDate,
                                         RefillCount = x.RefillCount,
                                         LastRefillDate = x.LastRefillDate,
                                         BottleType = x.BottleType,
                                         QrCode = x.QrCode,
                                         QrPrintCount = x.QrPrintCount,
                                         Status = (UsageStatus)x.Status,
                                         CreateDate = x.CreateDate,
                                         TrackingId = x.TrackingId
                                     }).Paginate(filter.PageNumber, filter.PageSize);
        }

        public void Delete(List<int> ids)
        {
            if (ids.Count > 0)
            {
                var bottles = _dbContext.Bottles.Where(x => ids.Contains(x.Id)).ToList();
                if (bottles.Count > 0)
                {
                    foreach (var bottle in bottles)
                    {
                        bottle.Deleted = true;
                        bottle.LastUpdateDate = DateTime.UtcNow;
                    }
                    _dbContext.SaveChanges();
                }
            }
        }

        public void Update(BottleUpdate data)
        {
            var bottle = _dbContext.Bottles.Where(x => x.Id == data.Id).FirstOrDefault();

            DateTimeOffset productionDate = DateTimeOffset.ParseExact(data.ProductionDate,
               ProductionDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            bottle.BottleType = data.BottleType;
            bottle.ProductionDate = productionDate;

            _dbContext.SaveChanges();
        }

        public BottleStatistics GetStatistics()
        {
            var bottles = _dbContext.Bottles;

            return new BottleStatistics
            {
                TotalNumberOfBottles = bottles.Count(),
                TotalNumberOfBottlesInUse = bottles.Where(x => x.Status == (int)UsageStatus.Valid).Count(),
                TotalNumberOfBottlesExpiredAndInUse = bottles.Where(x => x.Status == (int)UsageStatus.Expired).Count(),
                TotalNumberOfBottlesOutOfUse = bottles.Where(x => x.Status == (int)UsageStatus.Trash).Count()
            };
        }

        public void Add(BottleAdd bottleAdd)
        {
            var trackingId = Guid.NewGuid().ToString();
            DateTimeOffset productionDate = DateTimeOffset.ParseExact(bottleAdd.ProductionDate,
                ProductionDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var bottle = new Bottle
            {
                BottleType = (int)BottleTypes.L12,
                RefillCount = bottleAdd.RefillCount ?? 0,
                TrackingId = trackingId,
                QrCode = QrGenerator(trackingId, productionDate),
                ProductionDate = productionDate,
                Status = StatusCheck(productionDate)
            };

            _dbContext.Bottles.Add(bottle);
            _dbContext.SaveChanges();
        }


        private static string QrGenerator(string trackingId, DateTimeOffset productionDate)
        {
            return $"{trackingId}{productionDate.Year}{productionDate.Month}";
        }

        private static int StatusCheck(DateTimeOffset productionDate)
        {
            var current = DateTimeOffset.UtcNow;

            var result = current.Subtract(productionDate).TotalDays;

            if (result < BottleShelfLife)
            {
                return (int)UsageStatus.Valid;
            }
            else
            {
                return (int)UsageStatus.Expired;
            }
        }

        public BottleView GetByQrCode(string qrCode)
        {
            return _dbContext.Bottles.Where(x => x.QrCode == qrCode).Select(x => new BottleView
            {
                Id = x.Id,
                QrCode = x.QrCode,
                BottleType = x.BottleType,
                CreateDate = x.CreateDate,
                LastRefillDate = x.LastRefillDate,
                ProductionDate = x.ProductionDate,
                QrPrintCount = x.QrPrintCount,
                RefillCount = x.RefillCount,
                Status = (UsageStatus)x.Status,
                TrackingId = x.TrackingId
            }).FirstOrDefault();
        }

        public BottleStatusGetResponse GetBottleStatusByTrackingId(string trackingId)
        {
            return _dbContext.Bottles.Where(x => x.TrackingId == trackingId)
                              .Select(x => new BottleStatusGetResponse
                              {
                                  TrackingId = x.TrackingId,
                                  Status = (BottleTypes)x.Status,
                                  LastRefillDate = x.LastRefillDate,
                                  ProductionDate = x.ProductionDate,
                                  RefillCount = x.RefillCount
                              }).FirstOrDefault();
        }
    }
}
