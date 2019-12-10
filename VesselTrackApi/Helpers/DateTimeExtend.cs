using System;

namespace VesselTrackApi.Helpers
{
    public static class DateTimeExtend
    {
        public static long ToEpoch(this DateTime dateTime) => (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;

        public static DateTime FromEpoch(this long epoch) => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddSeconds(epoch);

        public static long? ToEpoch(this DateTime? dateTime) => dateTime.HasValue ? (long?)ToEpoch(dateTime.Value) : null;

        public static DateTime? FromEpoch(this long? epoch) => epoch.HasValue ? (DateTime?)FromEpoch(epoch.Value) : null;


        public static DateTime? ConvertToUtcTime(this DateTime? dt, DateTimeKind? sourceDateTimeKind)
        {
            if (dt == null) return null;

            dt = DateTime.SpecifyKind((DateTime)dt, (DateTimeKind)sourceDateTimeKind);
            if (sourceDateTimeKind == DateTimeKind.Local && TimeZoneInfo.Local.IsInvalidTime((DateTime)dt))
                return dt;

            return TimeZoneInfo.ConvertTimeToUtc((DateTime)dt);
        }
    }
}