using Business.Services;
using Core.Utilities.JWT;
using System.Net.Http.Headers;
using static Core.DTOs.User;

namespace BottleTracking_API.Helpers
{
    public class AuthHandler
    {
        public const string UserInfoKey = "UserInfo";
        private readonly RequestDelegate _next;

        public AuthHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITokenHelper tokenService, IPaneUserService adminService)
        {
            var authorization = context.Request.Headers["Authorization"].FirstOrDefault();
            _ = AuthenticationHeaderValue.TryParse(authorization, out var header);

            if (header != null)
            {
                if (header.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                {
                    var userId = tokenService.ValidateAccessToken(header?.Parameter, out var sessionId);
                    if (userId != null)
                    {
                        var user = adminService.GetByEmail(userId);
                        var userInfo = new UserInfo
                        {
                            Id = user.Id,
                            Email = user.Email,
                            SessionId = sessionId,
                            Type = user.Type
                        };
                        context.Items[UserInfoKey] = userInfo;
                    }
                }
            }

            await _next(context);
        }
    }
}
