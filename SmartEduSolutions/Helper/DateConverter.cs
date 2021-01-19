using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Helper
{
    public static class DateConverter
    {
        #region Unix Date time converter
        public static double GetUtcTimestamp(DateTime date)
        {
            return TimeSpan.FromTicks(date.Ticks).TotalMilliseconds -
                   TimeSpan.FromTicks(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks).TotalMilliseconds;
        }

        #endregion

        #region Unix Date time converter in second
        public static double GetUtcTimestampSecond(DateTime date)
        {
            return TimeSpan.FromTicks(date.Ticks).TotalSeconds -
                   TimeSpan.FromTicks(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks).TotalSeconds;
        }

        #endregion

        #region Convert Multiple DateTime (start date and end date) String to Date
        public static void StringToDateMultipleDate(string start, string end, string format, out DateTime startDate, out DateTime endDate)
        {
            startDate = DateTime.ParseExact(start, format, CultureInfo.InvariantCulture);
            endDate = DateTime.ParseExact(end, format, CultureInfo.InvariantCulture).AddDays(1);
        }


        #endregion

        #region Convert Single DateTime String to Date

        public static void StringToDateSingleDate(string date, string format, out DateTime singleDate)
        {
            singleDate = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
        }

        #endregion

        #region Get standard time

        public static DateTime GetCurrentLocalTime()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo BdZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, BdZone);
        }
        #endregion


    }
}
