using System;
using System.Threading.Tasks;
using Sample.Services.Rest;

namespace Sample.Features.TimeSeries
{
    public class TimeSeriesService : ITimeSeriesService
    {
        private readonly IRestService _rest;

        public TimeSeriesService(IRestService rest)
        {
            _rest = rest;
        }

        public async Task Append(Guid fileId, string timeSeriesId)
        {
            await _rest.Post("/api/timeseries/add", new
            {
                FileId = fileId,
                TimeSeriesId = timeSeriesId
            });
        }

    }
}