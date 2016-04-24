using System;

namespace Blogger2Ghost
{
    public static class DateTimeExtensions
    {
        public static long ToEpochTimeInMilliseconds(this DateTime dateTime)
        {
            TimeSpan t = dateTime - new DateTime(1970, 1, 1);
            return (long)t.TotalMilliseconds;
        }
    }
}
