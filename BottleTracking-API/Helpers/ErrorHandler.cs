using System.Net;
using System.Net.Mime;
using System.Text.Json;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Helpers
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;
        public ErrorHandler(RequestDelegate next)
        {
            _next = next;
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
                response.StatusCode = code;
                var result = JsonSerializer.Serialize(Messaging.GetResponse(false, code, error.Message, null));
                await response.WriteAsync(result);
            }
        }

        public class CustomException : Exception
        {
            public HttpStatusCode StatusCode { get; set; }
            public CustomException()
            {
            }

            public CustomException(string message, HttpStatusCode code)
                : base(message)
            {
                StatusCode = code;
            }
        }
    }
}
