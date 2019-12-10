using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VesselTrackApi.Data.Entities;
using VesselTrackApi.Helpers;
using VesselTrackApi.Models;
using VesselTrackApi.Services;
using VesselTrackApi.Validation;

namespace VesselTrackApi.Controllers
{
    /// <summary>
    /// Search Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VesselController : BaseController
    {
        private readonly ILogger<VesselController> _logger;
        private readonly ITrackService _trackService;

        public VesselController(ILogger<VesselController> logger, ITrackService trackService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _trackService = trackService ?? throw new ArgumentNullException(nameof(trackService));
        }

        /// <summary>
        /// Search via get request. Support Responses in application/json, application/vnd.api+json, application/xml, text/csv
        /// Every action logged in database table
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="429">Too Many Requests</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="mmsi">unique vessel identifier</param>
        /// <param name="minLat">search from latitude(between -90, 90)</param>
        /// <param name="maxLat">search to latitude(between -90, 90)</param>
        /// <param name="minLon">search from longitude(between -180, 180)</param>
        /// <param name="maxLon">search to longitude(between -180, 180)</param>
        /// <param name="from">search from timestamp</param>
        /// <param name="to">search to timestamp (Time convert it to utc)</param>
        /// <returns>List of vessels</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<VesselPosition>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.TooManyRequests)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(JsonErrorApi), (int)HttpStatusCode.InternalServerError)]
        [Produces(ContentType.APPLICATION_JSON,
                            ContentType.APPLICATION_VND_API_JSON,
                            ContentType.APPLICATION_XML,
                            ContentType.TEXT_CSV)]
        public async Task<IActionResult> Search([FromQuery] long[] mmsi, decimal? minLat, decimal? maxLat,
                                            decimal? minLon, decimal? maxLon, DateTime? from, DateTime? to)
        {
            try
            {
                var vps = new VesselPositionSearch(mmsi, minLat, maxLat, minLon, maxLon, from, to);

                var validation = new VesselPositionSearchValidation().Validate(vps);
                if (!validation.IsValid)
                    return JsonErrorApi.Error400(validation.Errors);

                var exp = ExpressionBuilder.BetweenDate<VesselPositionEntity>(vps.Timestamp)
                    .And(ExpressionBuilder.BetweenLat<VesselPositionEntity>(vps.Lat))
                    .And(ExpressionBuilder.BetweenLon<VesselPositionEntity>(vps.Lon))
                    .And(ExpressionBuilder.In<VesselPositionEntity>(vps.Mmsi));

                var results = (await _trackService.SearchAsync(exp).ConfigureAwait(false))
                                    .ToList();

                return results.Any() ? Ok(results.ToVesselPositions())
                                     : JsonErrorApi.Error404;
            }
            catch (Exception e)
            {
                _logger.LogError("Can't get vessels", e);
                return JsonErrorApi.Error500;
            }
        }
    }
}
