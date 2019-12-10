using System;
using Microsoft.AspNetCore.Mvc.Filters;
using VesselTrackApi.Helpers;

namespace VesselTrackApi.Controllers
{
    public class DateActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("Timestamp", out object timestamp))
            {
                var t = (DateTime?) timestamp;
                context.ActionArguments["Timestamp"] = t.ConvertToUtcTime(t.Value.Kind);
            }

            if (context.ActionArguments.TryGetValue("from", out object from))
            {
                var t = (DateTime?) from;
                context.ActionArguments["from"] = t.ConvertToUtcTime(t.Value.Kind);
            }

            if (context.ActionArguments.TryGetValue("to", out object to))
            {
                var t = (DateTime?) to;
                context.ActionArguments["to"] = t.ConvertToUtcTime(t.Value.Kind);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}