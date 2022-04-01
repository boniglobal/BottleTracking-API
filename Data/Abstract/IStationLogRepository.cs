using Core.Models;
using static Core.DTOs.StationLog;

namespace Data.Abstract
{
    public interface IStationLogRepository
    {
        PagedData<StationLogGetResponse> GetAll(RequestFilter filter);
        StationLogStatistics GetStatistics();
        void Add(string trackingId, int kioskId);
    }
}
