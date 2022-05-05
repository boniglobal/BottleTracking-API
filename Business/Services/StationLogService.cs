using Core.Models;
using Data.Abstract;
using static Core.DTOs.StationLog;

namespace Business.Services
{
    public interface IStationLogService
    {
        PagedData<StationLogGetResponse> GetAll(RequestFilter filter);
        StationLogStatistics GetLogStatistics();
        void Add(long trackingId, int kioskId);
    }
    public class StationLogService : IStationLogService
    {
        private readonly IStationLogRepository _stationLogRepository;

        public StationLogService(IStationLogRepository stationLogRepository)
        {
            _stationLogRepository = stationLogRepository;
        }

        public void Add(long trackingId, int kioskId)
        {
            _stationLogRepository.Add(trackingId, kioskId);
        }

        public PagedData<StationLogGetResponse> GetAll(RequestFilter filter)
        {
            return _stationLogRepository.GetAll(filter);
        }

        public StationLogStatistics GetLogStatistics()
        {
            return _stationLogRepository.GetStatistics();
        }
    }
}