using System;
using System.IO;
using System.Threading.Tasks;

namespace Sample.Features.Files
{
    public interface IFileService
    {
        Task<Guid> Upload(Stream timeSeriesContent);
        Task WaitForFileProcessed(Guid fileId);
    }
}