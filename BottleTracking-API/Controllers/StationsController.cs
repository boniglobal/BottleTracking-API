using BottleTracking_API.Helpers;
using Business.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using static Core.DTOs.Station;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Controllers
{
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
        public dynamic GetAll([FromQuery] RequestFilter filter)
        {
            var data = _stationService.GetAll(filter);
            return Messaging.GetResponse(true, null, null, data);
        }

        [HttpGet]
        [Route("statistics")]
        public dynamic GetStatistics()
        {
            var data = _stationService.GetStatistics();
            return Messaging.GetResponse(true, null, null, data);
        }

        [HttpPost]
        public dynamic Add(StationAdd station)
        {
            _stationService.Add(station);
            return Messaging.GetResponse(true, null, null, null);
        }

        [HttpPut]
        public dynamic Update(StationUpdate station)
        {
            _stationService.Update(station);
            return Messaging.GetResponse(true, null, null, null);
        }

        [HttpDelete]
        [Route("{id}")]
        public dynamic Delete(int id)
        {
            _stationService.Delete(id);
            return Messaging.GetResponse(true, null, null, null);
        }
    }
}
