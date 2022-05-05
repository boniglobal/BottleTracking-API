using Business.Utilities;
using Core.Constants;
using Core.Models;
using Data.Abstract;
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

        public StationLogService(IStationLogRepository stationLogRepository)
        {
            _stationLogRepository = stationLogRepository;
        }

        public void Add(long trackingId, int? kioskId)
        {
            CheckWhetherTheUserIsAssignedToAKiosk(kioskId);
            _stationLogRepository.Add(trackingId, kioskId.GetValueOrDefault());
        }

        public PagedData<StationLogGetResponse> GetAll(RequestFilter filter)
        {
            return _stationLogRepository.GetAll(filter);
        }

        public StationLogStatistics GetLogStatistics()
        {
            return _stationLogRepository.GetStatistics();
        }

        private static void CheckWhetherTheUserIsAssignedToAKiosk(int? kioskId)
        {
            if (kioskId == null)
            {
                throw new CustomException(Messages.UserNotAssignedToKiosk, HttpStatusCode.BadRequest);
            }
        }
    }
}