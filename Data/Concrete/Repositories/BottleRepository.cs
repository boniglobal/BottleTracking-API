using Core.Models;
using Data.Abstract;
using Data.Concrete.Contexts;
using static Core.Constants.BottleConstants;
using static Core.DTOs.Bottle;
using Core.Extensions;
using Entities;
using System.Globalization;
using Core.Utilities.Encryption;
using Microsoft.Extensions.Configuration;

namespace Data.Concrete.Repositories
{
    public class BottleRepository : IBottleRepository
    {
        private readonly BottleTrackingDbContext _dbContext;
        private readonly IAesEncryptor _aesEncryptor;
        private readonly IConfiguration _configuration;

        public BottleRepository(
            BottleTrackingDbContext dbContext,
            IAesEncryptor aesEncryptor, 
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _aesEncryptor = aesEncryptor;
            _configuration = configuration;
        }

        public BottleView GetById(int id)
        {
            return _dbContext.Bottles.Where(x => x.Id == id).Select(x => new BottleView
            {
                Id = x.Id,
                ProductionDate = x.ProductionDate,
                RefillCount = x.RefillCount,
                LastRefillDate = x.LastRefillDate,
                BottleType = (BottleTypes)x.BottleType,
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
                                         BottleType = (BottleTypes)x.BottleType,
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
                        bottle.LastUpdateDate = DateTimeOffset.UtcNow;
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

            bottle.BottleType = (int)data.BottleType;
            bottle.ProductionDate = productionDate;
            bottle.LastUpdateDate = DateTimeOffset.UtcNow;
            bottle.Status = BottleStatusHelper.CheckBottleStatus(bottle, _dbContext);

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
            DateTimeOffset productionDate = DateTimeOffset.ParseExact(bottleAdd.ProductionDate,
                ProductionDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var bottle = new Bottle
            {
                BottleType = (int)bottleAdd.BottleType,
                RefillCount = bottleAdd.RefillCount ?? 0,
                ProductionDate = productionDate
            };

            bottle.Status = BottleStatusHelper.CheckBottleStatus(bottle);
            _dbContext.Bottles.Add(bottle);
            _dbContext.SaveChanges();

            var trackingId = Convert.ToInt64($"{bottle.Id}{bottle.ProductionDate.Date:MMyy}");
            bottle.TrackingId = _aesEncryptor.EncryptData(trackingId);
            bottle.QrCode = $"{_configuration["QrUrl"]}{bottle.TrackingId}";
            _dbContext.SaveChanges();
        }

        public BottleView GetByQrCode(string qrCode)
        {
            return _dbContext.Bottles.Where(x => x.QrCode == qrCode).Select(x => new BottleView
            {
                Id = x.Id,
                QrCode = x.QrCode,
                BottleType = (BottleTypes)x.BottleType,
                CreateDate = x.CreateDate,
                LastRefillDate = x.LastRefillDate,
                ProductionDate = x.ProductionDate,
                QrPrintCount = x.QrPrintCount,
                RefillCount = x.RefillCount,
                Status = (UsageStatus)x.Status,
                TrackingId = x.TrackingId
            }).FirstOrDefault();
        }

        public BottleStatusGetResponse GetBottleStatusByTrackingId(long trackingId)
        {
            return _dbContext.Bottles.Where(x => x.TrackingId == trackingId)
                              .Select(x => new BottleStatusGetResponse
                              {
                                  TrackingId = x.TrackingId,
                                  Status = (UsageStatus)x.Status,
                                  LastRefillDate = x.LastRefillDate,
                                  ProductionDate = x.ProductionDate,
                                  RefillCount = x.RefillCount
                              }).FirstOrDefault();
        }
    }
}
