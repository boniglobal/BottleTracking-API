using BottleTracking_API.Helpers;
using Business.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using static Core.Constants.DocumentTexts;
using static Core.DTOs.Station;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Controllers
{
    [SwaggerTag(Station.ControllerDesc)]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize("Admin, Panel")]
    [Route("[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly IStationService _stationService;

        public StationsController(IStationService stationService)
        {
            _stationService = stationService;
        }

        [HttpGet]
        [SwaggerOperation(nameof(GetAll), Station.GetAllDesc)]
        [ProducesResponseType(typeof(PagedData<StationListView>), StatusCodes.Status200OK)]
        public dynamic GetAll([FromQuery] RequestFilter filter)
        {
            var data = _stationService.GetAll(filter);
            return Messaging.GetResponse(true, null, null, data);
        }

        [HttpGet]
        [SwaggerOperation(nameof(GetStatistics), Station.GetStatisticsDesc)]
        [ProducesResponseType(typeof(StationStatistics), StatusCodes.Status200OK)]
        [Route("statistics")]
        public dynamic GetStatistics()
        {
            var data = _stationService.GetStatistics();
            return Messaging.GetResponse(true, null, null, data);
        }

        [HttpPost]
        [SwaggerOperation(nameof(Add), Station.PostDesc)]
        public dynamic Add(StationAdd station)
        {
            _stationService.Add(station);
            return Messaging.GetResponse(true, null, null, null);
        }

        [HttpPut]
        [SwaggerOperation(nameof(Update), Station.PutDesc)]
        public dynamic Update(StationUpdate station)
        {
            _stationService.Update(station);
            return Messaging.GetResponse(true, null, null, null);
        }

        ///<param name="id" example="1"></param>
        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(nameof(Delete), Station.DeleteDesc)]
        public dynamic Delete(int id)
        {
            _stationService.Delete(id);
            return Messaging.GetResponse(true, null, null, null);
        }
    }
}
