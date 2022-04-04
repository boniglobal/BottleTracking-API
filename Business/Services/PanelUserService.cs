using Core.Utilities;
using Core.Models;
using Data.Abstract;
using Entities;
using static Core.DTOs.User;
using Core.Constants;
using System.Net;
using Business.Utilities;

namespace Business.Services
{
    public interface IPaneUserService
    {
        PanelUser GetByEmail(string email);
        PanelUser GetById(int Id);
        UserInfo GetUserInfo(string email, string password);
        PagedData<PanelUserGetResponse> GetAll(RequestFilter filter);
        void Add(PanelUserAddRequest data);
        void ResetPassword(ResetPassword data);
        void Update(PanelUserUpdateRequest data);
        void Delete(int id);
    }

    public class PanelUserService : IPaneUserService
    {
        private readonly IPanelUserRepository _userRepository;
        private readonly IStationService _stationService;
        public PanelUserService(
            IPanelUserRepository userRepository, 
            IStationService stationService)
        {
            _userRepository = userRepository;
            _stationService = stationService;
        }

        public void Add(PanelUserAddRequest data)
        {
            var existingUser = _userRepository.GetByEmail(data.Email);
            if(existingUser != null)
            {
                throw new CustomException(Messages.NonUniqueEmail, HttpStatusCode.BadRequest);
            }
            _userRepository.Add(data);
        }

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        public PagedData<PanelUserGetResponse> GetAll(RequestFilter filter)
        {
            return _userRepository.GetAll(filter);
        }

        public PanelUser GetByEmail(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        public PanelUser GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public UserInfo GetUserInfo(string email, string password)
        {
            var user = _userRepository.GetByEmail(email);
            if (user != null)
            {
                if (HashingHelper.VerifyPasswordHash(password, user?.Password, user?.PasswordSalt))
                {
                    var station = _stationService.GetByPanelUserId(user.Id);
                    return new UserInfo
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Type = user.Type,
                        StationId = station.Id
                    };
                }
            }

            return null;
        }

        public void ResetPassword(ResetPassword data)
        {
            _userRepository.ResetPassword(data);
        }

        public void Update(PanelUserUpdateRequest data)
        {
            var existingUser = _userRepository.GetByEmail(data.Email);
            if(existingUser != null && existingUser.Id != data.Id)
            {
                throw new CustomException(Messages.NonUniqueEmail, HttpStatusCode.BadRequest);
            }
            _userRepository.Update(data, existingUser);
        }
    }
}
