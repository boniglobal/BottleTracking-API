using Entities;

namespace Data.Abstract
{
    public interface ITokenRepository
    {
        RefreshToken Get(string token);
        void RevokeTokensByUserId(string userId, string sessionId);
        void Add(RefreshToken token);
        void Save();
    }
}
