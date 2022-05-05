using Core.Models;
using Entities;
using static Core.DTOs.StationLog;

namespace Data.Abstract
{
    public interface IStationLogRepository
    {
        PagedData<StationLogGetResponse> GetAll(RequestFilter filter);
        StationLogStatistics GetStatistics();
        void Add(Bottle bottle, int kioskId);
    }
}
