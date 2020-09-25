using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Sample.Utils
{
    public static class ContentGenerator
    {
        /// <summary>
        /// Generate a CSV containing samples for a series with a given frequency and number of hours.
        /// </summary>
        public static Stream GenerateRandomSeries(DateTimeOffset? startDate, long hertz, long hours)
        {
            var rnd = new Random();
            var ts = startDate?.ToNanoSecondsSinceEpoch() ?? 0L;
            var samples = hours * 60L * 60L * hertz;
            var interval = DateTimeExtensions.NanosecondsPerSecond / hertz;
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 4096, true))
            {
                for (var s = 0; s < samples; s++)
                {
                    writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0},{1}", ts, rnd.NextDouble()));
                    ts += interval;
                }
            }

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}