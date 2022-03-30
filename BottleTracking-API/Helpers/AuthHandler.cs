using Business.Services;
using Core.Constants;
using Core.Utilities.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http.Headers;
using System.Text;
using static Core.Constants.UserTypes;
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
                else if (header.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
                {
                    var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(header.Parameter)).Split(":");
                    var userInfo = adminService.GetUserInfo(credentials[0], credentials[1]);
                    if (userInfo == null || userInfo.Type != (int)Types.Kiosk)
                    {
                        throw new UnauthorizedAccessException(Messages.Unauthorized);
                    }
                    context.Items[UserInfoKey] = userInfo;
                }
            }

            await _next(context);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        readonly string _roles;

        public AuthorizeAttribute(string roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            var authRoles = _roles.Split(',').Select(x => x.Trim());

            var userInfo = (UserInfo)context.HttpContext.Items[AuthHandler.UserInfoKey];
            string userRole = string.Empty;
            if (userInfo != null)
            {
                userRole = Enum.GetName(typeof(Types), userInfo.Type);
            }
            if (!authRoles.Contains(userRole))
            {
                throw new UnauthorizedAccessException(Messages.Unauthorized);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute, IAllowAnonymous
    {

    }
}
