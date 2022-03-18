using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using static Core.Models.ResponseModels;

namespace BottleTracking_API.Helpers
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Where(x => x.Value.Errors.Count > 0)
                                               .Select(x => new KeyValuePair<string, object>(x.Key, x.Value.Errors.Select(e => e.ErrorMessage)))
                                               .ToDictionary(x=>x.Key, x=>x.Value);

                var responseObj = new Response
                {
                    Success = false,
                    Status = new Status
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "One or more validation errors occurred."
                    },
                    Data = errors
                };

                context.Result = new JsonResult(responseObj);
            }
        }
    }
}
