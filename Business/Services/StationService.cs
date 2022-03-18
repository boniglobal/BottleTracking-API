using Core.Models;
using Data.Abstract;
using Entities;
using static Core.DTOs.Station;

namespace Business.Services
{
    public interface IStationService
    {
        PagedData<StationListView> GetAll(RequestFilter filter);
        Station GetByPanelUserId(int id);
        StationStatistics GetStatistics();
        void Add(StationAdd station);
        void Update(StationUpdate station);
        void Delete(int id);
    }
    public class StationService : IStationService
    {
        private readonly IStationRepository _stationRepository;

        public StationService(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public void Add(StationAdd station)
        {
            _stationRepository.Add(station);
        }

        public void Delete(int id)
        {
            _stationRepository.Delete(id);
        }

        public PagedData<StationListView> GetAll(RequestFilter filter)
        {
            return _stationRepository.GetAll(filter);
        }

        public Station GetByPanelUserId(int id)
        {
            return _stationRepository.GetByPanelUserId(id);
        }

        public StationStatistics GetStatistics()
        {
            return _stationRepository.GetStatistics();
        }

        public void Update(StationUpdate station)
        {
            _stationRepository.Update(station);
        }
    }
}
