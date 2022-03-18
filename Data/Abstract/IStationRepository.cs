using Core.Models;
using Entities;
using static Core.DTOs.Station;

namespace Data.Abstract
{
    public interface IStationRepository
    {
        PagedData<StationListView> GetAll(RequestFilter filter);
        Station GetByPanelUserId(int id);
        StationStatistics GetStatistics();
        void Add(StationAdd data);
        void Update(StationUpdate data);
        void Delete(int id);
    }
}
