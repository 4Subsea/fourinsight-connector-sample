using System;

namespace Sample.Utils
{
    public static class DateTimeExtensions
    {
        public static readonly DateTimeOffset UnixEpochDto = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

        public static readonly long NanosecondsPerDay = UnixEpochDto.AddDays(1).ToNanoSecondsSinceEpoch();
        public static readonly long NanosecondsPerMinute = NanosecondsPerDay / (24L * 60L);
        public static readonly long NanosecondsPerSecond = NanosecondsPerDay / (24L * 60L * 60L);

        /// <summary>
        /// Return the difference, in nanoseconds, between <paramref name="dto"/> and EPOCH (1970-01-01 00:00:00)
        /// </summary>
        public static long ToNanoSecondsSinceEpoch(this DateTimeOffset dto)
        {
            return (dto.Ticks - UnixEpochDto.Ticks) * 100L;
        }
    }
}