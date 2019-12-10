using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VesselTrackApi.Models;
using VesselTrackApi.Services;

namespace VesselTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : BaseController
    {
        private readonly ILogger<ImportController> _logger;
        private readonly IImportService _importService;

 
        public ImportController(ILogger<ImportController> logger,
            IImportService importService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _importService = importService ?? throw new ArgumentNullException(nameof(importService));
        }

        /// <summary>
        /// Import sample data (ship_positions.json) via post request
        /// </summary>
        /// <param name="file">import demo json format data to import in database</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Import([FromForm] IFormFile file)
        {
            if (file == null)
            {
                _logger.Log(LogLevel.Warning, "empty file");
                throw new ArgumentNullException(nameof(file));
            }

            try
            {
                return Ok(await _importService.ImportAsync(file.OpenReadStream())
                                            .ConfigureAwait(false));
            }
            catch (Exception e)
            {
                _logger.LogError("Can't import vessels", e);
                return JsonErrorApi.Error500;
            }
        }
    }
}