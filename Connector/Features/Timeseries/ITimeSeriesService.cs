using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Features.TimeSeries
{
    public interface ITimeSeriesService
    {
        /// <summary>
        /// Add data from <paramref name="fileId"/> to <paramref name="timeSeriesId"/>.
        /// </summary>
        Task AddData(Guid fileId, string timeSeriesId);

        /// <summary>
        /// Get data from <paramref name="timeSeriesId"/> within <paramref name="start"/> and <paramref name="end"/>.
        /// </summary>
        IAsyncEnumerable<Sample> GetData(string timeSeriesId, DateTimeOffset start, DateTimeOffset end);

        /// <summary>
        /// Get data from <paramref name="timeSeriesId"/> within <paramref name="start"/> and <paramref name="end"/>.
        /// Start and end is given in nanoseconds since EPOCH.
        /// All samples are ensured to be in sequence and any duplicate data are removed.
        /// </summary>
        IAsyncEnumerable<Sample> GetData(string timeSeriesId, long start, long end);

        /// <summary>
        /// Get raw data from <paramref name="timeSeriesId"/> within <paramref name="start"/> and <paramref name="end"/>.
        /// Start and end is given in nanoseconds since EPOCH.
        /// </summary>
        IAsyncEnumerable<string> GetRawData(string timeSeriesId, long start, long end);
    }
}