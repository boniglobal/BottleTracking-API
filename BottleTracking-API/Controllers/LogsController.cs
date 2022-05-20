using Microsoft.AspNetCore.Mvc;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogger<LogsController> _logger;

        public LogsController(ILogger<LogsController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public dynamic Add()
        {
            return Messaging.GetResponse(true, null, null, null);
        }
    }
}
