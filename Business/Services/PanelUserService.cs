using Data.Abstract;
using Entities;

namespace Business.Services
{
    public interface IPaneUserService
    {
        PanelUser GetByEmail(string email);
        PanelUser GetById(int Id);
    }
    public class PanelUserService : IPaneUserService
    {
        private readonly IPanelUserRepository _userRepository;

        public PanelUserService(IPanelUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public PanelUser GetByEmail(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        public PanelUser GetById(int id)
        {
            return _userRepository.GetById(id);
        }
    }
}
