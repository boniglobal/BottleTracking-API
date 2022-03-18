using Core.Models;
using static Core.DTOs.Bottle;

namespace Data.Abstract
{
    public interface IBottleRepository
    {
        BottleView GetById(int id);
        PagedData<BottleView> GetAll(RequestFilter filter);
        BottleStatistics GetStatistics();
        void Add(BottleAdd bottleAdd);
        void Update(BottleUpdate data);
        void Delete(List<int> ids);
    }
}
