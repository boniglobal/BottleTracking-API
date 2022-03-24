using Core.Models;
using Data.Abstract;
using Entities;
using static Core.DTOs.User;

namespace Business.Services
{
    public interface IPaneUserService
    {
        PanelUser GetByEmail(string email);
        PanelUser GetById(int Id);
        PagedData<PanelUserGetResponse> GetAll(RequestFilter filter);
        void Add(PanelUserAddRequest data);
        void Update(PanelUserUpdateRequest data);
        void Delete(int id);
    }
    public class PanelUserService : IPaneUserService
    {
        private readonly IPanelUserRepository _userRepository;

        public PanelUserService(IPanelUserRepository userRepository)
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

        public void Update(PanelUserUpdateRequest data)
        {
            _userRepository.Update(data);
        }
    }
}
