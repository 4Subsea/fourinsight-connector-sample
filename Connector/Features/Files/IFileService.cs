using System;
using System.IO;
using System.Threading.Tasks;

namespace Sample.Features.Files
{
    public interface IFileService
    {
        /// <summary>
        /// Upload data to a file and return the file id for later adding to a time series.
        /// </summary>
        Task<Guid> Upload(Stream timeSeriesContent);

        /// <summary>
        /// Check processing state of a file and return when the state is either Ready or Failed.
        /// </summary>
        Task WaitForFileProcessed(Guid fileId);
    }
}