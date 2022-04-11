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
    [Authorize("Admin")]
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
        [SwaggerOperation(nameof(GetAll), PanelUser.GetAllDesc)]
        [ProducesResponseType(typeof(PagedData<PanelUserGetResponse>), StatusCodes.Status200OK)]
        public dynamic GetAll([FromQuery] RequestFilter filter)
        {
            var data = _panelUserService.GetAll(filter);
            return Messaging.GetResponse(true, null, null, data);
        }

        [HttpPost]
        [SwaggerOperation(nameof(Add), PanelUser.PostDesc)]
        public dynamic Add(PanelUserAddRequest data)
        {
            _panelUserService.Add(data);
            return Messaging.GetResponse(true, null, null, null);
        }

        [HttpPut]
        [SwaggerOperation(nameof(Update), PanelUser.PostDesc)]
        public dynamic Update(PanelUserUpdateRequest data)
        {
            _panelUserService.Update(data);
            return Messaging.GetResponse(true, null, null, null);
        }

        [HttpPut]
        [Route("reset-password")]
        [SwaggerOperation(nameof(ResetPassword), PanelUser.ResetPasswordDesc)]
        public dynamic ResetPassword(ResetPassword data)
        {
            _panelUserService.ResetPassword(data);
            return Messaging.GetResponse(true, null, null, null);
        }

        ///<param name="id" example="1"></param>
        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(nameof(Delegate), PanelUser.DeleteDesc)]
        public dynamic Delete(int id)
        {
            _panelUserService.Delete(id);
            return Messaging.GetResponse(true, null, null, null);
        }
    }
}
