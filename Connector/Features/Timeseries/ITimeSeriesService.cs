using System;
using System.Threading.Tasks;

namespace Sample.Features.TimeSeries
{
    public interface ITimeSeriesService
    {
        Task Append(Guid fileId, string timeSeriesId);
    }
}