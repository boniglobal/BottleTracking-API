using Core.Constants;
using Core.Utilities;
using Core.Utilities.JWT;
using Data.Abstract;
using Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Core.Models.AuthModels;

namespace Business.Services
{
    public interface ITokenService
    {
        AuthResponse Authenticate(AuthRequest authRequest, out TokenInfo refreshToken);
        AuthResponse RefreshToken(string oldToken, out TokenInfo newToken);
        void RevokeTokens(string userId, string sessionId);
    }
    public class TokenService : ITokenService
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly IPaneUserService _paneUserService;
        private readonly ITokenRepository _tokenRepository;

        public TokenService(
            ITokenHelper tokenHelper,
            IPaneUserService paneUserService, 
            ITokenRepository tokenRepository)
        {
            _tokenHelper = tokenHelper;
            _paneUserService = paneUserService;
            _tokenRepository = tokenRepository;
        }

        public AuthResponse Authenticate(AuthRequest authRequest, out TokenInfo refreshToken)
        {
            var user = _paneUserService.GetByEmail(authRequest.Email);
            var checkPassword = (user != null) && HashingHelper.VerifyPasswordHash(authRequest.Password, user.Password, user.PasswordSalt);
            if (checkPassword == false)
            {
                throw new UnauthorizedAccessException(Messages.InvalidUserInfo);
            }
            var sessionId = Guid.NewGuid().ToString();
            refreshToken = _tokenHelper.GenerateRefreshToken(sessionId, authRequest.RememberMe.GetValueOrDefault());
            RotateRefreshTokens(user.Email, false, refreshToken);

            return GetAuthResponse(user, sessionId);
        }

        public AuthResponse RefreshToken(string oldToken, out TokenInfo newToken)
        {
            PanelUser user = null;
            newToken = null;
            var token = _tokenRepository.Get(oldToken);
            if(token == null)
            {
                throw new UnauthorizedAccessException(Messages.InvalidToken);
            }

            var sessionId = token.SessionId;

            if (token.RevokedTime == null && token.ExpirationTime > DateTimeOffset.UtcNow)
            {
                newToken = _tokenHelper.GenerateRefreshToken(sessionId, token.HasLongTTL.GetValueOrDefault());
                user = _paneUserService.GetByEmail(token.UserId);
            }
            RotateRefreshTokens(token.UserId, true, newToken);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            return GetAuthResponse(user, sessionId);
        }

        public void RevokeTokens(string userId, string sessionId)
        {
            RotateRefreshTokens(userId, true, new TokenInfo { Token = null, SessionId = sessionId });
        }

        private AuthResponse GetAuthResponse(PanelUser user, string sessionId)
        {
            var fullName = user.Name + " " + user.Surname;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sid, sessionId),
            };
            var accessToken = _tokenHelper.GenerateToken(user, claims);

            return new AuthResponse
            {
                Name = fullName,
                Email = user.Email,
                UserType = ((UserConstants.Types) user.Type).ToString(),
                AccessToken = accessToken.Token,
                TokenExpiration = accessToken.Expiration
            };
        }

        private void RotateRefreshTokens(string userId, bool revoke, TokenInfo token = null)
        {
            if (revoke)
                _tokenRepository.RevokeTokensByUserId(userId, token?.SessionId);

            if (!string.IsNullOrEmpty(token?.Token))
            {
                var now = DateTime.UtcNow;
                var newToken = new RefreshToken
                {
                    Token = token.Token,
                    UserId = userId,
                    CreateDate = now,
                    ExpirationTime = token.Expires,
                    HasLongTTL = token.RememberMe,
                    RevokedTime = null,
                    SessionId = token.SessionId
                };
                _tokenRepository.Add(newToken);
            }

            _tokenRepository.Save();
        }
    }
}
