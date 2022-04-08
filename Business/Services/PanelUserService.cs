using Core.Utilities;
﻿using Core.Models;
using Data.Abstract;
using Entities;
using static Core.DTOs.User;
using static Core.Constants.UserConstants;

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
        public PanelUserService(
            IPanelUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Add(PanelUserAddRequest data)
        {
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
                    return new UserInfo
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Type = user.Type,
                        StationId = user.Type == (int)Types.Kiosk ? _userRepository.GetUserStationIdByUserId(user.Id) : null 
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
            _userRepository.Update(data);
        }
    }
}
