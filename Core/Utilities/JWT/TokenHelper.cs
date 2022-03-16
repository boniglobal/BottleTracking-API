using Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Core.Constants.UserTypes;

namespace Core.Utilities.JWT
{
    public interface ITokenHelper
    {
        AccessToken GenerateToken(PanelUser user, List<Claim> additionClaims = null);
        TokenInfo GenerateRefreshToken(string sessionId, bool rememberMe = false);
        public string ValidateAccessToken(string token, out string sessionId);
    }
    public class TokenHelper : ITokenHelper
    {
        private readonly TokenSettings _tokenSettings;
        public TokenHelper(IOptions<TokenSettings> options)
        {
            _tokenSettings = options.Value;
        }
        public AccessToken GenerateToken(PanelUser user, List<Claim> additionClaims = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = SetClaims(user);

            if(additionClaims.Count > 0)
            {
                claims.AddRange(additionClaims);
            }

            var jwt = new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Audience,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_tokenSettings.AccessTokenTTL)),
                claims: claims,
                signingCredentials: credentials
                );
            JwtSecurityTokenHandler jwtSecurityToken = new();

            return new AccessToken
            {
                Token = jwtSecurityToken.WriteToken(jwt),
                Expiration = jwt.ValidTo
            };
        }

        private static List<Claim> SetClaims(PanelUser user)
        {
            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, ((Types)user.Type).ToString())
            };

            return claims;
        }

        public TokenInfo GenerateRefreshToken(string sessionId, bool rememberMe = false)
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[64];
            rng.GetBytes(bytes);

            var ttlMinutes = rememberMe ? _tokenSettings.RefreshTokenLongTTL : _tokenSettings.RefreshTokenShortTTL;
            var ttl = TimeSpan.FromMinutes(ttlMinutes);

            return new TokenInfo
            {
                Token = Convert.ToBase64String(bytes),
                SessionId = sessionId,
                Expires = DateTimeOffset.UtcNow.Add(ttl),
                RememberMe = rememberMe
            };
        }

        public string ValidateAccessToken(string token, out string sessionId)
        {
            sessionId = null;
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret)),
                    ValidateIssuer = true,
                    ValidIssuer = _tokenSettings.Issuer,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validToken);

                var jwtToken = (JwtSecurityToken)validToken;
                sessionId = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid).Value;
                return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
