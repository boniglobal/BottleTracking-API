using Business.Utilities;
using Core.Constants;
using Core.Models;
using Data.Abstract;
using Entities;
using System.Net;
using static Core.DTOs.StationLog;

namespace Business.Services
{
    public interface IStationLogService
    {
        PagedData<StationLogGetResponse> GetAll(RequestFilter filter);
        StationLogStatistics GetLogStatistics();
        void Add(long trackingId, int? kioskId);
    }
    public class StationLogService : IStationLogService
    {
        private readonly IStationLogRepository _stationLogRepository;
        private readonly IBottleService _bottleService;

        public StationLogService(
            IStationLogRepository stationLogRepository, 
            IBottleService bottleService)
        {
            _stationLogRepository = stationLogRepository;
            _bottleService = bottleService;
        }

        public void Add(long trackingId, int? kioskId)
        {
            CheckIfUserIsAssignedToKiosk(kioskId);
            var bottle = GetBottleByTrackingId(trackingId);
            _stationLogRepository.Add(bottle, kioskId.GetValueOrDefault());
        }

        public PagedData<StationLogGetResponse> GetAll(RequestFilter filter)
        {
            return _stationLogRepository.GetAll(filter);
        }

        public StationLogStatistics GetLogStatistics()
        {
            return _stationLogRepository.GetStatistics();
        }

        private static void CheckIfUserIsAssignedToKiosk(int? kioskId)
        {
            if (kioskId == null)
            {
                throw new CustomException(Messages.UserNotAssignedToKiosk, HttpStatusCode.BadRequest);
            }
        }

        private Bottle GetBottleByTrackingId(long trackingId)
        {
            var bottle = _bottleService.GetByTrackingId(trackingId);
            if (bottle == null)
            {
                throw new CustomException(Messages.BottleNotFound, HttpStatusCode.NotFound);
            }
            return bottle;
        }
    }
}