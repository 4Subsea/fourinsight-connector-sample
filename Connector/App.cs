using System;
using System.Threading.Tasks;
using Sample.Features.Files;
using Sample.Features.TimeSeries;
using Sample.Utils;

namespace Sample
{
    public class App
    {
        private readonly IFileService _file;
        private readonly ITimeSeriesService _timeSeries;

        public App(
            IFileService file,
            ITimeSeriesService timeSeries)
        {
            _file = file;
            _timeSeries = timeSeries;
        }

        public async Task Run()
        {
            var timeSeriesId = "<timeseries id>";

            // await AddDataToTimeSeries(timeSeriesId);
            // await DownloadTimeSeriesData(timeSeriesId);
            // await DownloadTimeSeriesRawData(timeSeriesId);
        }

        /// <summary>
        /// Shows how to add data to an existing series.
        /// Data is structured as CSV where each line represent one sample.
        /// Each sample have a timestamp and a value, where timestamp is in nanoseconds since EPOCH.
        /// Value can be number or text data.
        /// </summary>
        private async Task AddDataToTimeSeries(string timeSeriesId)
        {
            Guid fileId;
            await using (var timeSeriesContent = ContentGenerator.GenerateRandomSeries(DateTimeOffset.UtcNow, 1, 12))
            {
                fileId = await _file.Upload(timeSeriesContent);
            }

            Console.WriteLine($"{fileId} uploaded. Waiting for processing ready...");

            await _file.WaitForFileProcessed(fileId);

            Console.WriteLine($"Appending {fileId} to {timeSeriesId}...");
            await _timeSeries.AddData(fileId, timeSeriesId);
        }

        /// <summary>
        /// Shows how to download data from a series.
        /// All data chunks will be downloaded and merged together to ensure de-duplication
        /// </summary>
        private async Task DownloadTimeSeriesData(string timeSeriesId)
        {
            var start = new DateTimeOffset(2010, 9, 22, 0, 0, 0, TimeSpan.Zero);
            var end = new DateTimeOffset(2010, 9, 26, 0, 0, 0, TimeSpan.Zero);
            var samples = _timeSeries.GetData(timeSeriesId, start, end);
            await foreach (var sample in samples)
            {
                Console.WriteLine($"{sample.Value}: {sample.Value}");
            }
        }

        /// <summary>
        /// Shows how to download raw data from a series.
        /// </summary>
        private async Task DownloadTimeSeriesRawData(string timeSeriesId)
        {
            var start = new DateTimeOffset(2010, 9, 22, 0, 0, 0, TimeSpan.Zero);
            var end = new DateTimeOffset(2010, 9, 26, 0, 0, 0, TimeSpan.Zero);
            var samples = _timeSeries.GetRawData(timeSeriesId, start.ToNanoSecondsSinceEpoch(), end.ToNanoSecondsSinceEpoch());
            await foreach (var sample in samples)
            {
                Console.WriteLine(sample);
            }
        }
    }
}
