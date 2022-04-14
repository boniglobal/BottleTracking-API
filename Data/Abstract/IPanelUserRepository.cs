using Core.Models;
using Entities;
using static Core.DTOs.User;

namespace Data.Abstract
{
    public interface IPanelUserRepository
    {
        PanelUser GetByEmail(string email);
        PanelUser GetById(int id);
        int GetUserStationIdByUserId(int userId);
        PagedData<PanelUserGetResponse> GetAll(RequestFilter filter);
        List<KioskUserGetResponse> GetUnassignedKioskUsers();
        void Add(PanelUserAddRequest data);
        void ResetPassword(ResetPassword data);
        void Update(PanelUserUpdateRequest data, PanelUser user);
        void Delete(int id);
    }
}
