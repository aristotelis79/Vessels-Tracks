using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VesselTrackApi.Services
{
    public interface IImportService
    {
        Task<bool> ImportAsync(Stream stream, CancellationToken token = default(CancellationToken));
    }
}