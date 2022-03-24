using Business.Services;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Core.DTOs.User;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Controllers
{
    [Authorize(Roles = "Admin")] //TO DO: Custom Authorize attribute will be used.
    [Route("[controller]")]
    [ApiController]
    public class PanelUsersController : ControllerBase
    {
        private readonly IPaneUserService _panelUserService;

        public PanelUsersController(IPaneUserService panelUserService)
        {
            _panelUserService = panelUserService;
        }

        [HttpGet]
        public dynamic GetAll([FromQuery] RequestFilter filter)
        {
            var data = _panelUserService.GetAll(filter);
            return Messaging.GetResponse(true, null, null, data);
        }

        [HttpPost]
        public dynamic Add(PanelUserAddRequest data)
        {
            _panelUserService.Add(data);
            return Messaging.GetResponse(true, null, null, null);
        }

        [HttpPut]
        public dynamic Update(PanelUserUpdateRequest data)
        {
            _panelUserService.Update(data);
            return Messaging.GetResponse(true, null, null, null);
        }

        [HttpDelete]
        [Route("{id}")]
        public dynamic Delete(int id)
        {
            _panelUserService.Delete(id);
            return Messaging.GetResponse(true, null, null, null);
        }
    }
}
