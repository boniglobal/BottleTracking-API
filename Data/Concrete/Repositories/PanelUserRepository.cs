using Data.Abstract;
using Data.Concrete.Contexts;
using Entities;

namespace Data.Concrete.Repositories
{
    public class PanelUserRepository : IPanelUserRepository
    {
        private readonly BottleTrackingDbContext _dbContext;

        public PanelUserRepository(BottleTrackingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PanelUser GetByEmail(string email)
        {
            return _dbContext.PanelUsers.Where(x=>x.Email == email).FirstOrDefault();    
        }
    }
}
