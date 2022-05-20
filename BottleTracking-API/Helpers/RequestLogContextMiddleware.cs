using System.Text;
using static Core.DTOs.User;

namespace BottleTracking_API.Helpers
{
    public class RequestLogContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLogContextMiddleware> _logger;

        public RequestLogContextMiddleware(RequestDelegate next, ILogger<RequestLogContextMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            DateTimeOffset requestTimestamp = DateTimeOffset.UtcNow;
            string endpoint = context.Request.Path;
            var ipAddress = context.Connection.RemoteIpAddress;

            string requestBody;
            string responseBody;

            using (var bodyReader = new StreamReader(context.Request.Body))
            {
                requestBody = await bodyReader.ReadToEndAsync();
                context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
            }


            using (var buffer = new MemoryStream())
            {
                var stream = context.Response.Body;
                context.Response.Body = buffer;

                await _next.Invoke(context);

                buffer.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(buffer);
                using (var bufferReader = new StreamReader(buffer))
                {
                    responseBody = await bufferReader.ReadToEndAsync();

                    buffer.Seek(0, SeekOrigin.Begin);

                    await buffer.CopyToAsync(stream);
                    context.Response.Body = stream;

                }
            }

            int responseCode = context.Response.StatusCode;
            var user = (UserInfo)context.Items["UserInfo"];

            _logger.LogInformation("{endpoint} {body} {response_code} {response_message} {ip_address} {user_id} {request_timestamp}", 
                endpoint, requestBody, responseCode, responseBody, ipAddress, user?.Id, requestTimestamp);
        }
    }
}
