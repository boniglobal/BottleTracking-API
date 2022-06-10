using Business.Utilities;
using Core.Constants;
using Core.Models;
using Data.Abstract;
using Entities;
using System.Net;
using static Core.Constants.UserConstants;
using static Core.DTOs.Station;

namespace Business.Services
{
    public interface IStationService
    {
        PagedData<StationListView> GetAll(RequestFilter filter);
        Station GetById(int id);
        Station GetByPanelUserId(int id);
        StationStatistics GetStatistics();
        void Add(StationAdd station);
        void Update(StationUpdate station);
        void Delete(int id);
    }
    public class StationService : IStationService
    {
        private readonly IStationRepository _stationRepository;
        private readonly IPaneUserService _paneUserService;
        public StationService(
            IStationRepository stationRepository, 
            IPaneUserService paneUserService)
        {
            _stationRepository = stationRepository;
            _paneUserService = paneUserService;
        }

        public void Add(StationAdd station)
        {
            CheckIfUserIsAssignedToAnotherStation(station.PanelUserId);
            CheckIfUserExistsOrItsTypeIsKiosk(station.PanelUserId);

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

        public Station GetById(int id)
        {
            return _stationRepository.GetById(id);
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
            CheckIfUserIsAssignedToAnotherStation(station.PanelUserId, station.StationId);
            CheckIfUserExistsOrItsTypeIsKiosk(station.PanelUserId);

            _stationRepository.Update(station);
        }

        private void CheckIfUserIsAssignedToAnotherStation(int userId, int? stationId = null)
        {
            var existingStation = _stationRepository.GetByPanelUserId(userId);
            if (stationId == null && existingStation != null)
            {
                throw new CustomException(Messages.AssignUserMoreThanOneStation, HttpStatusCode.BadRequest);
            }else if(stationId != null && existingStation != null && existingStation.Id != stationId)
            {
                throw new CustomException(Messages.AssignUserMoreThanOneStation, HttpStatusCode.BadRequest);
            }
        }

        private void CheckIfUserExistsOrItsTypeIsKiosk(int userId)
        {
            var user = _paneUserService.GetById(userId);
            if (user == null)
            {
                throw new CustomException(Messages.UserNotFound, HttpStatusCode.BadRequest);
            }
            else if (user.Type != (int)Types.Kiosk)
            {
                throw new CustomException(Messages.AssignDifferentUserType, HttpStatusCode.BadRequest);
            }
        }
    }
}
