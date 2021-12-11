using System;

namespace Video_Clip2.Elements
{
    public static class Extensions
    {
        public static TimeSpan ToTimeSpan(this double value) => TimeSpan.FromSeconds(value);
        public static TimeSpan ToTimeSpan(this double value, double trackScale) => TimeSpan.FromSeconds(value / trackScale);
        public static double ToDouble(this TimeSpan value) => value.TotalSeconds;
        public static double ToDouble(this TimeSpan value, double trackScale) => value.TotalSeconds * trackScale;
        public static string ToText(this TimeSpan value) => value.ToString("mm':'ss'.'ff");
        public static TimeSpan Scale(this TimeSpan value, double scale) => scale == 1 ? value : TimeSpan.FromSeconds(value.TotalSeconds * scale);
    }
}