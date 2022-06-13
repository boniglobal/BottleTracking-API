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
        void Add(StationLogAdd log, int? kioskId);
    }
    public class StationLogService : IStationLogService
    {
        private readonly IStationLogRepository _stationLogRepository;
        private readonly IBottleService _bottleService;
        private readonly IStationService _stationService;

        public StationLogService(
            IStationLogRepository stationLogRepository, 
            IBottleService bottleService,
            IStationService stationService)
        {
            _stationLogRepository = stationLogRepository;
            _stationService = stationService;
            _bottleService = bottleService;
        }

        public void Add(StationLogAdd log, int? kioskId)
        {
            var station = GetStationById(kioskId);
            CheckStationLocationForDistributorInfo(ref log, station.Location);
            var bottle = GetBottleByTrackingId(log.TrackingId);
            _stationLogRepository.Add(log, bottle, station.Id);
        }

        public PagedData<StationLogGetResponse> GetAll(RequestFilter filter)
        {
            return _stationLogRepository.GetAll(filter);
        }

        public StationLogStatistics GetLogStatistics()
        {
            return _stationLogRepository.GetStatistics();
        }

        private Station GetStationById(int? id)
        {
            if (id == null)
            {
                throw new CustomException(Messages.UserNotAssignedToKiosk, HttpStatusCode.BadRequest);
            }

            var station = _stationService.GetById(id.GetValueOrDefault());
            if (id == null)
            {
                throw new CustomException(Messages.StationNotFound, HttpStatusCode.BadRequest);
            }

            return station;
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

        private static void CheckStationLocationForDistributorInfo(ref StationLogAdd log, int stationLocation)
        {
            //clear distributor info if station is located at the start of the line
            if (stationLocation == (int)StationConstants.Locations.Pre)
            {
                log.DistributorId = null;
                log.DistributionRegion = null;
            }
        }
    }
}