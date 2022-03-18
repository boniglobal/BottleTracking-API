using Business.Services;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Core.DTOs.Bottle;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Controllers
{
    [Authorize(Roles = "Admin, Panel, Printer")]
    [Route("[controller]")]
    [ApiController]
    public class BottlesController : ControllerBase
    {
        private readonly IBottleService _bottleService;

        public BottlesController(IBottleService bottleService)
        {
            _bottleService = bottleService;
        }

        [HttpGet]
        [Route("{id}")]
        public dynamic Get(int id)
        {
            var data = _bottleService.GetById(id);
            return Messaging.GetResponse(true, null, null, data);
        }

        [HttpGet]
        public dynamic GetAll([FromQuery] RequestFilter filter)
        {
            var data = _bottleService.GetAll(filter);
            return Messaging.GetResponse(true, null, null, data);
        }

        [HttpGet]
        [Route("statistics")]
        public dynamic GetStatistics()
        {
            var data = _bottleService.GetStatistics();
            return Messaging.GetResponse(true, null, null, data);
        }

        [HttpPost]
        public dynamic Add(BottleAdd bottleAdd)
        {
            _bottleService.Add(bottleAdd);
            return Messaging.GetResponse(true, null, null, null);
        }

        [HttpPut]
        public dynamic Update(BottleUpdate data)
        {
            _bottleService.Update(data);
            return Messaging.GetResponse(true, null, null, null);
        }

        [HttpDelete]
        public dynamic Delete([FromBody]List<int> id)
        {
            _bottleService.Delete(id);
            return Messaging.GetResponse(true, null, null, null);
        }
    }
}
