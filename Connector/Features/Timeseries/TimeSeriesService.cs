using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Services.Rest;
using Sample.Services.Storage;
using Sample.Utils;

namespace Sample.Features.TimeSeries
{
    public class TimeSeriesService : ITimeSeriesService
    {
        private readonly IRestService _rest;
        private readonly IBlobService _blob;

        public TimeSeriesService(
            IRestService rest,
            IBlobService blob)
        {
            _rest = rest;
            _blob = blob;
        }

        public async Task AddData(Guid fileId, string timeSeriesId)
        {
            await _rest.Post("/api/timeseries/add", new
            {
                FileId = fileId,
                TimeSeriesId = timeSeriesId
            });
        }

        public IAsyncEnumerable<Sample> GetData(string timeSeriesId, DateTimeOffset start, DateTimeOffset end)
        {
            return GetData(timeSeriesId, start.ToNanoSecondsSinceEpoch(), end.ToNanoSecondsSinceEpoch());
        }

        public async IAsyncEnumerable<Sample> GetData(string timeSeriesId, long start, long end)
        {
            var dictionary = new Dictionary<long, Sample>();
            var dl = await GetTimeSeriesDataDays(timeSeriesId, start, end);

            foreach (var file in dl.Files.OrderBy(f => f.Index))
            {
                foreach (var chunk in file.Chunks)
                {
                    await using var data = await _blob.Download(chunk.EndpointUri); 
                    var samples = SampleReader.Read(data);
                    await foreach (var sample in samples)
                    {
                        dictionary[sample.Timestamp] = sample;
                    }
                }
            }

            foreach (var sample in dictionary.Values)
            {
                yield return sample;
            }
        }

        public async IAsyncEnumerable<string> GetRawData(string timeSeriesId, long start, long end)
        {
            var dl = await GetTimeSeriesDataDays(timeSeriesId, start, end);

            foreach (var file in dl.Files.OrderBy(f => f.Index))
            {
                foreach (var chunk in file.Chunks)
                {
                    await using var data = await _blob.Download(chunk.EndpointUri); 
                    using var reader = new StreamReader(data, Encoding.UTF8);
                    while (!reader.EndOfStream)
                        yield return await reader.ReadLineAsync();
                }
            }
        }

        private async Task<TimeSeriesDownloadResponse> GetTimeSeriesDataDays(string timeSeriesId, long start, long end)
        {
            return await _rest.Get<TimeSeriesDownloadResponse>($"/api/timeseries/{timeSeriesId}/data/days?start={start}&end={end}");
        }

        public class TimeSeriesDownloadResponse
        {
            public File[] Files { get; set; }
        }

        /// <summary>
        /// Blob reference with access path and key.
        /// </summary>
        public class Chunk
        {
            /// <summary>The combined endpoint, containing account, path and sas key</summary>
            public string Endpoint { get; set; }
            public Uri EndpointUri => new Uri(Endpoint);
        }

        public class File
        {
            public int Index { get; set; }
            public Chunk[] Chunks { get; set; }
        }
    }
}