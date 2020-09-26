using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sample.Features.TimeSeries
{
    public struct Sample
    {
        public long Timestamp;
        public string Value;
    }

    public class SampleReader
    {
        public static async IAsyncEnumerable<Sample> Read(Stream file)
        {
            using var reader = new StreamReader(file, Encoding.UTF8, leaveOpen: true);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                var timestamp = Parse(line);
                if (!timestamp.HasValue)
                    break;

                yield return new Sample { Timestamp = timestamp.Value, Value = line };
            }
        }

        private static long? Parse(string line)
        {
            var first = line?.Split(',').FirstOrDefault();
            if (first == null)
                return null;

            if (long.TryParse(first, out var ts))
                return ts;

            return null;
        }
    }
}