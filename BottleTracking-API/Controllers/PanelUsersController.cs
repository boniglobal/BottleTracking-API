using BottleTracking_API.Helpers;
using Business.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using static Core.Constants.DocumentTexts;
using static Core.DTOs.User;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Controllers
{
    [SwaggerTag(PanelUser.ControllerDesc)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    [ApiController]
    public class PanelUsersController : ControllerBase
    {
        private readonly IPaneUserService _panelUserService;

        public PanelUsersController(IPaneUserService panelUserService)
        {
            _panelUserService = panelUserService;
        }

        [Authorize("Admin")]
        [HttpGet]
        [SwaggerOperation(nameof(GetAll), PanelUser.GetAllDesc)]
        [ProducesResponseType(typeof(PagedData<PanelUserGetResponse>), StatusCodes.Status200OK)]
        public dynamic GetAll([FromQuery] RequestFilter filter)
        {
            var data = _panelUserService.GetAll(filter);
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Admin, Panel")]
        [HttpGet]
        [Route("unassigned-kiosk-users")]
        [SwaggerOperation(nameof(GetUnassignedKioskUsers), PanelUser.GetUnassignedKioskUsers)]
        [ProducesResponseType(typeof(List<KioskUserGetResponse>), StatusCodes.Status200OK)]
        public dynamic GetUnassignedKioskUsers()
        {
            var data = _panelUserService.GetUnassignedKioskUsers();
            return Messaging.GetResponse(true, null, null, data);
        }

        [Authorize("Admin")]
        [HttpPost]
        [SwaggerOperation(nameof(Add), PanelUser.PostDesc)]
        public dynamic Add(PanelUserAddRequest data)
        {
            _panelUserService.Add(data);
            return Messaging.GetResponse(true, null, null, null);
        }

        [Authorize("Admin")]
        [HttpPut]
        [SwaggerOperation(nameof(Update), PanelUser.PutDesc)]
        public dynamic Update(PanelUserUpdateRequest data)
        {
            _panelUserService.Update(data);
            return Messaging.GetResponse(true, null, null, null);
        }

        [Authorize("Admin")]
        [HttpPut]
        [Route("reset-password")]
        [SwaggerOperation(nameof(ResetPassword), PanelUser.ResetPasswordDesc)]
        public dynamic ResetPassword(ResetPassword data)
        {
            _panelUserService.ResetPassword(data);
            return Messaging.GetResponse(true, null, null, null);
        }

        ///<param name="id" example="1"></param>
        [Authorize("Admin")]
        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(nameof(Delete), PanelUser.DeleteDesc)]
        public dynamic Delete(int id)
        {
            _panelUserService.Delete(id);
            return Messaging.GetResponse(true, null, null, null);
        }
    }
}
