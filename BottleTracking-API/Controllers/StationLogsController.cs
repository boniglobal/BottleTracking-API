using BottleTracking_API.Helpers;
using Business.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using static Core.DTOs.User;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StationLogsController : ControllerBase
    {
        private readonly IStationLogService _stationLogService;

        public StationLogsController(IStationLogService stationLogService)
        {
            _stationLogService = stationLogService;
        }

        [Authorize("Admin, Panel")]
        [HttpGet]
        public dynamic GetAll([FromQuery] RequestFilter filter)
        {
            var data = _stationLogService.GetAll(filter);
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Admin, Panel")]
        [HttpGet]
        [Route("statistics")]
        public dynamic GetStatistics()
        {
            var data = _stationLogService.GetLogStatistics();
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Kiosk")]
        [HttpPost]
        [Route("{trackingId}")]
        public dynamic Add(string trackingId)
        {
            var user = (UserInfo)Request.HttpContext.Items["UserInfo"];
            _stationLogService.Add(trackingId, user.StationId.GetValueOrDefault());
            return Messaging.GetResponse(true, null, null, null);
        }
    }
}
