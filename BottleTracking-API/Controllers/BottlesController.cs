using BottleTracking_API.Helpers;
using Business.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using static Core.DTOs.Bottle;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BottlesController : ControllerBase
    {
        private readonly IBottleService _bottleService;

        public BottlesController(IBottleService bottleService)
        {
            _bottleService = bottleService;
        }

        [Authorize("Admin, Panel, Printer")]
        [HttpGet]
        [Route("{id:int}")]
        public dynamic Get(int id)
        {
            var data = _bottleService.GetById(id);
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Admin, Panel, Printer")]
        [HttpGet]
        [Route("{qrCode}")]
        public dynamic Get(string qrCode)
        {
            var data = _bottleService.GetByQrCode(qrCode);
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Admin, Panel, Kiosk")]
        [HttpGet]
        [Route("check-status")]
        public dynamic GetStatus(string trackingId)
        {
            var data = _bottleService.GetBottleStatusByTrackingId(trackingId);
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Admin, Panel, Printer")]
        [HttpGet]
        public dynamic GetAll([FromQuery] RequestFilter filter)
        {
            var data = _bottleService.GetAll(filter);
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Admin, Panel, Printer")]
        [HttpGet]
        [Route("statistics")]
        public dynamic GetStatistics()
        {
            var data = _bottleService.GetStatistics();
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Admin, Panel, Printer")]
        [HttpPost]
        public dynamic Add(BottleAdd bottleAdd)
        {
            _bottleService.Add(bottleAdd);
            return Messaging.GetResponse(true, null, null, null);
        }

        [Authorize("Admin, Panel, Printer")]
        [HttpPut]
        public dynamic Update(BottleUpdate data)
        {
            _bottleService.Update(data);
            return Messaging.GetResponse(true, null, null, null);
        }

        [Authorize("Admin, Panel, Printer")]
        [HttpDelete]
        public dynamic Delete([FromBody]List<int> id)
        {
            _bottleService.Delete(id);
            return Messaging.GetResponse(true, null, null, null);
        }
    }
}
