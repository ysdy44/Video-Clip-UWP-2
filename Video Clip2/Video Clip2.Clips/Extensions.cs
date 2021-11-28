using System;

namespace Video_Clip2.Clips
{
    public static class Extensions
    {
        public static TimeSpan ToTimeSpan(this double value) => TimeSpan.FromSeconds(value);
        public static TimeSpan ToTimeSpan(this double value, double trackScale) => TimeSpan.FromSeconds(value / trackScale);
        public static double ToDouble(this TimeSpan value) => value.TotalSeconds;
        public static double ToDouble(this TimeSpan value, double trackScale) => value.TotalSeconds * trackScale;
    }
}