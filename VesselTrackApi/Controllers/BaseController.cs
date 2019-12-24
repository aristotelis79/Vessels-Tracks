using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VesselTrackApi.Models;

namespace VesselTrackApi.Controllers
{
    public class BaseController : ControllerBase
    {
        [NonAction]
        public override OkObjectResult Ok(object value)
        {
            var accept = HttpContext?.Request?.Headers["Accept"].FirstOrDefault();

            if (accept == null || !accept.Equals(ContentType.APPLICATION_VND_API_JSON))
                return new OkObjectResult(value);

            if (IsIEnumerable(value))
            {
                var jsonApis = (from object v in (IEnumerable) value
                                select new JsonApi<Guid>((IJsonApi<Guid>) v)).ToList();

                return new OkObjectResult(jsonApis);
            }

            var jsonApi = new JsonApi<Guid>((IJsonApi<Guid>) value);

            return new OkObjectResult(jsonApi);
        }

        private bool IsIEnumerable(object o)
        {
            return (o.GetType().GetInterfaces().Any(
                                            i => i.IsGenericType &&
                                                 i.GetGenericTypeDefinition() == typeof(IEnumerable<>)));
        }
    }
}