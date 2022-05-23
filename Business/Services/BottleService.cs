using Core.Models;
using Data.Abstract;
using Entities;
using static Core.DTOs.Bottle;

namespace Business.Services
{
    public interface IBottleService
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
    public class BottleService : IBottleService
    {
        private readonly IBottleRepository _bottleRepository;

        public BottleService(IBottleRepository bottleRepository)
        {
            _bottleRepository = bottleRepository;
        }
        public BottleView GetById(int id)
        {
            return _bottleRepository.GetById(id);
        }

        public Bottle GetByTrackingId(long trackingId)
        {
            return _bottleRepository.GetByTrackingId(trackingId);
        }

        public BottleView GetDetailByTrackingId(long trackingId)
        {
            return _bottleRepository.GetDetailByTrackingId(trackingId);
        }

        public PagedData<BottleView> GetAll(RequestFilter filter)
        {
            return _bottleRepository.GetAll(filter);
        }

        public BottleStatistics GetStatistics()
        {
            return _bottleRepository.GetStatistics();
        }

        public void Add(BottleAdd bottleAdd)
        {
            _bottleRepository.Add(bottleAdd);
        }

        public void Update(BottleUpdate data)
        {
            _bottleRepository.Update(data);
        }

        public void Delete(List<int> ids)
        {
            _bottleRepository.Delete(ids);
        }

        public BottleStatusGetResponse GetBottleStatusByTrackingId(long trackingId)
        {
            return _bottleRepository.GetBottleStatusByTrackingId(trackingId);
        }
    }
}
