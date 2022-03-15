using System.Text;

namespace Core.Utilities
{
    public class HashingHelper
    {
        public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using System.Security.Cryptography.HMACSHA256 hmac = new();
            passwordSalt = Convert.ToBase64String(hmac.Key);
            passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        public static bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
        {
            var bytePasswordHash = Convert.FromBase64String(passwordHash);
            var bytePasswordSalt = Convert.FromBase64String(passwordSalt);

            using System.Security.Cryptography.HMACSHA256 hmac = new(bytePasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != bytePasswordHash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
