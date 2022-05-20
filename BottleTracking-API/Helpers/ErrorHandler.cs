using Business.Utilities;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using static Core.DTOs.User;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Helpers
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandler> _logger;
        public ErrorHandler(
            RequestDelegate next,
            ILogger<ErrorHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = MediaTypeNames.Application.Json;
                var code = error switch
                {
                    CustomException e => (int)e.StatusCode,
                    InvalidOperationException => (int)HttpStatusCode.BadRequest,
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                var ipAddress = context.Connection.RemoteIpAddress;
                var user = (UserInfo)context.Items["UserInfo"];
                var endpoint = context.Request.Path;

                response.StatusCode = code;
                var result = JsonSerializer.Serialize(Messaging.GetResponse(false, code, error.Message, null));
                await response.WriteAsync(result);

                _logger.LogError("{endpoint} {user_id} {ip_address} {response_code} {response_message} {exception}",
                    endpoint, user?.Id, ipAddress, code, result, error);
            }
        }
    }
}
