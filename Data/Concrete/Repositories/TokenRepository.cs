using Data.Abstract;
using Data.Concrete.Contexts;
using Entities;

namespace Data.Concrete.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly BottleTrackingDbContext _dbContext;

        public TokenRepository(BottleTrackingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(RefreshToken token)
        {
            _dbContext.RefreshTokens.Add(token);
        }

        public RefreshToken Get(string token)
        {
            return _dbContext.RefreshTokens.Where(x => x.Token == token).FirstOrDefault();
        }

        public void RevokeTokensByUserId(string userId, string sessionId)
        {
            var now = DateTimeOffset.UtcNow;
            var query = _dbContext.RefreshTokens.Where(t => t.UserId == userId && t.RevokedTime == null);
            if (!string.IsNullOrEmpty(sessionId))
            {
                query = query.Where(t => t.SessionId == sessionId);
            }

            var tokens = query.ToList();
            foreach (var t in tokens)
            {
                t.RevokedTime = now;
            }
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
