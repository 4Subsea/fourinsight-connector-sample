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
            Guid fileId;
            await using (var timeSeriesContent = ContentGenerator.GenerateRandomSeries(DateTimeOffset.UtcNow, 1, 12))
            {
                fileId = await _file.Upload(timeSeriesContent);
            }

            Console.WriteLine($"{fileId} uploaded. Waiting for processing read");

            await _file.WaitForFileProcessed(fileId);

            Console.ReadLine();
        }


    }
}