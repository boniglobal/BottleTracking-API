using BottleTracking_API.Helpers;
using Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using static Core.DTOs.User;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogger<LogsController> _logger;

        public LogsController(ILogger<LogsController> logger)
        {
            _logger = logger;
        }

        [Authorize("Admin, Panel, Printer, Kiosk")]
        [HttpPost]
        public dynamic Add(Logging log)
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress;
            var user = (UserInfo)Request.HttpContext.Items["UserInfo"];
            _logger.Log(logLevel: LogLevel.None, "{endpoint} {user_id} {ip_address} {response_code} {response_message} {request_timestamp}", 
                log.Endpoint, user?.Id, ipAddress, log.ResponseCode, log.ResponseMessage, log.RequestTimestamp);
            return Messaging.GetResponse(true, null, null, null);
        }
    }
}
