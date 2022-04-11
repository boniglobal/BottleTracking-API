using Business.Services;
using Core.Utilities.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using static Core.Constants.DocumentTexts;
using static Core.DTOs.User;
using static Core.Models.AuthModels;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Controllers
{
    [SwaggerTag(Token.ControllerDesc)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly TokenSettings _tokenSettings;
        public TokenController(
            ITokenService tokenService,
            IOptions<TokenSettings> tokenSettings)
        {
            _tokenService = tokenService;
            _tokenSettings = tokenSettings.Value;
        }

        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        [SwaggerOperation(nameof(Authenticate), Token.AuthenticateDesc)]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        public dynamic Authenticate(AuthRequest authRequest)
        {
            var data = _tokenService.Authenticate(authRequest, out TokenInfo refreshToken);
            AppendRefreshToken(refreshToken);

            return Messaging.GetResponse(true, null, null, data);
        }

        [AllowAnonymous]
        [Route("Refresh")]
        [HttpPost]
        public dynamic Refresh()
        {
            var oldToken = Request.Cookies["refresh_token"];
            var userData = _tokenService.RefreshToken(oldToken, out TokenInfo newToken);

            AppendRefreshToken(newToken);

            return Messaging.GetResponse(true, null, null, userData);
        }

        [Authorize]
        [Route("Revoke")]
        [HttpPost]
        public dynamic Revoke()
        {
            var user = (UserInfo)Request.HttpContext.Items["UserInfo"];
            _tokenService.RevokeTokens(user.Email, user.SessionId);

            AppendRefreshToken(null);

            return Messaging.GetResponse(true, null, null);
        }


        private void AppendRefreshToken(TokenInfo token)
        {
            var cookieOptions = new CookieOptions
            {
                Domain = _tokenSettings.Issuer,
                HttpOnly = true,
                Expires = token?.Expires ?? DateTimeOffset.UtcNow,
                SameSite = SameSiteMode.None,
                Secure = true,
            };
            var tokenStr = token?.Token ?? string.Empty;
            Response.Cookies.Append("refresh_token", tokenStr, cookieOptions);
        }
    }
}
