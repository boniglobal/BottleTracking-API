using BottleTracking_API.Helpers;
using Business.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using static Core.Constants.DocumentTexts;
using static Core.DTOs.Bottle;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Controllers
{
    [SwaggerTag(Bottle.ControllerDesc)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    [ApiController]
    public class BottlesController : ControllerBase
    {
        private readonly IBottleService _bottleService;

        public BottlesController(IBottleService bottleService)
        {
            _bottleService = bottleService;
        }

        /// <param name="id" example="1"></param>
        [Authorize("Admin, Panel, Printer")]
        [HttpGet]
        [Route("{id:int}")]
        [SwaggerOperation(nameof(Get), Bottle.GetDesc)]
        [ProducesResponseType(typeof(BottleView), StatusCodes.Status200OK)]
        public dynamic Get(int id)
        {
            var data = _bottleService.GetById(id);
            return Messaging.GetResponse(true, null, null, data);
        }

        /// <param name="trackingId" example="815259166761"></param>
        [Authorize("Admin, Panel, Printer")]
        [HttpGet]
        [Route("{trackingId}")]
        [SwaggerOperation(nameof(Get), Bottle.GetByTrackingId)]
        [ProducesResponseType(typeof(BottleView), StatusCodes.Status200OK)]
        public dynamic Get(long trackingId)
        {
            var data = _bottleService.GetDetailByTrackingId(trackingId);
            return Messaging.GetResponse(true, null, null, data);
        }

        ///<param name="trackingId" example="815259166761"></param>
        [Authorize("Admin, Panel, Kiosk")]
        [HttpGet]
        [Route("check-status")]
        [SwaggerOperation(nameof(GetStatus), Bottle.CheckStatusDesc)]
        [ProducesResponseType(typeof(BottleStatusGetResponse), StatusCodes.Status200OK)]
        public dynamic GetStatus(long trackingId)
        {
            var data = _bottleService.GetBottleStatusByTrackingId(trackingId);
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Admin, Panel, Printer")]
        [HttpGet]
        [SwaggerOperation(nameof(GetAll), Bottle.GetAllDesc)]
        [ProducesResponseType(typeof(PagedData<BottleView>), StatusCodes.Status200OK)]
        public dynamic GetAll([FromQuery] RequestFilter filter)
        {
            var data = _bottleService.GetAll(filter);
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Admin, Panel, Printer")]
        [HttpGet]
        [Route("statistics")]
        [SwaggerOperation(nameof(GetStatistics), Bottle.GetStatisticsDesc)]
        [ProducesResponseType(typeof(BottleStatistics), StatusCodes.Status200OK)]
        public dynamic GetStatistics()
        {
            var data = _bottleService.GetStatistics();
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Admin, Panel, Printer")]
        [HttpPost]
        [SwaggerOperation(nameof(Add), Bottle.PostDesc)]
        public dynamic Add(BottleAdd bottleAdd)
        {
            _bottleService.Add(bottleAdd);
            return Messaging.GetResponse(true, null, null, null);
        }

        [Authorize("Admin, Panel, Printer")]
        [HttpPut]
        [SwaggerOperation(nameof(Update), Bottle.PutDesc)]
        public dynamic Update(BottleUpdate data)
        {
            _bottleService.Update(data);
            return Messaging.GetResponse(true, null, null, null);
        }

        /// <param name="id" example="[1,2,3]"></param>
        [Authorize("Admin, Panel, Printer")]
        [HttpDelete]
        [SwaggerOperation(nameof(Delete), Bottle.DeleteDesc)]
        public dynamic Delete([FromBody] List<int> id)
        {
            _bottleService.Delete(id);
            return Messaging.GetResponse(true, null, null, null);
        }
    }
}
