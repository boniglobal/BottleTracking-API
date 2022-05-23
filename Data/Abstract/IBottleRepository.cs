using Core.Models;
using Entities;
using static Core.DTOs.Bottle;

namespace Data.Abstract
{
    public interface IBottleRepository
    {
        BottleView GetById(int id);
        Bottle GetByTrackingId(long trackingId);
        BottleStatusGetResponse GetBottleStatusByTrackingId(long trackingId);
        BottleView GetDetailByTrackingId(long trackingId);
        PagedData<BottleView> GetAll(RequestFilter filter);
        BottleStatistics GetStatistics();
        void Add(BottleAdd bottleAdd);
        void Update(BottleUpdate data);
        void Delete(List<int> ids);
    }
}
