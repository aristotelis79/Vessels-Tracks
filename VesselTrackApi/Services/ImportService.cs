using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VesselTrackApi.Data;
using VesselTrackApi.Data.Entities;
using VesselTrackApi.Repositories;

namespace VesselTrackApi.Services
{ 
    public class ImportService : IImportService
    {
        private ILogger<ImportService> _logger;

        private readonly IRepository<VesselPositionEntity, long> _repository;

        public ImportService(ILogger<ImportService> logger,
                                IRepository<VesselPositionEntity,long> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> ImportAsync(Stream stream, CancellationToken token = default(CancellationToken))
        {
            CheckForNull(stream);

            using (var r = new StreamReader(stream))
            {
                var data = await r.ReadToEndAsync().ConfigureAwait(false);

                var vesselsPos = JsonConvert.DeserializeObject<List<VesselPositionEntity>>(data);
                
                await _repository.InsertAsync(vesselsPos,token).ConfigureAwait(false);
                
                return true;
            }
        }


        private void CheckForNull(Stream stream)
        {
            if (stream != null) return;

            _logger.Log(LogLevel.Warning, "empty stream");

            throw new ArgumentNullException(nameof(stream));
        }
    }
}