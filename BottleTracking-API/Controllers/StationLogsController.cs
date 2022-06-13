using BottleTracking_API.Helpers;
using Business.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using static Core.Constants.DocumentTexts;
using static Core.DTOs.StationLog;
using static Core.DTOs.User;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Controllers
{
    [SwaggerTag(StationLog.ControllerDesc)]
    [Produces(MediaTypeNames.Application.Json)]
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
        [SwaggerOperation(nameof(GetAll), StationLog.GetAllDesc)]
        [ProducesResponseType(typeof(PagedData<StationLogGetResponse>), StatusCodes.Status200OK)]
        public dynamic GetAll([FromQuery] RequestFilter filter)
        {
            var data = _stationLogService.GetAll(filter);
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Admin, Panel")]
        [HttpGet]
        [Route("statistics")]
        [SwaggerOperation(nameof(GetStatistics), StationLog.GetStatisticsDesc)]
        [ProducesResponseType(typeof(StationLogStatistics), StatusCodes.Status200OK)]
        public dynamic GetStatistics()
        {
            var data = _stationLogService.GetLogStatistics();
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Kiosk")]
        [HttpPost]
        [SwaggerOperation(nameof(Add), StationLog.PostDesc)]
        public dynamic Add(StationLogAdd log)
        {
            var user = (UserInfo)Request.HttpContext.Items["UserInfo"];
            _stationLogService.Add(log, user.StationId);
            return Messaging.GetResponse(true, null, null, null);
        }
    }
}
