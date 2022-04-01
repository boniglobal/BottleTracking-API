using Core.Utilities;
using Data.Abstract;
using Entities;
using static Core.DTOs.User;

namespace Business.Services
{
    public interface IPaneUserService
    {
        PanelUser GetByEmail(string email);
        PanelUser GetById(int Id);
        UserInfo GetUserInfo(string email, string password);
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
            if(user != null)
            {
                if(HashingHelper.VerifyPasswordHash(password, user?.Password, user?.PasswordSalt))
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
    }
}
