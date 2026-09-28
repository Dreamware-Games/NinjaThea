using System;

namespace NinjaThea.Util
{
    public static class TimeFormat
    {
        public const int OneHourHundredths = 360000;

        // mm:ss.ff; past an hour the minutes keep counting (e.g. 75:02.10)
        public static string Format(float seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return $"{(int)time.TotalMinutes:00}:{time.Seconds:00}.{time.Milliseconds / 10:00}";
        }

        // Orders Format() strings: more minute digits means a longer time
        public static int Compare(string a, string b)
        {
            if (a.Length != b.Length) return a.Length.CompareTo(b.Length);
            return string.CompareOrdinal(a, b);
        }

        // Allocation free version of Format, writes exactly 8 chars (mm:ss.ff) at offset
        public static void Write(float seconds, char[] buffer, int offset = 0)
        {
            int hundredths = (int)(seconds * 100f);
            int minutes = hundredths / 6000 % 60;
            int secs = hundredths / 100 % 60;
            int fraction = hundredths % 100;
            buffer[offset] = (char)('0' + minutes / 10);
            buffer[offset + 1] = (char)('0' + minutes % 10);
            buffer[offset + 2] = ':';
            buffer[offset + 3] = (char)('0' + secs / 10);
            buffer[offset + 4] = (char)('0' + secs % 10);
            buffer[offset + 5] = '.';
            buffer[offset + 6] = (char)('0' + fraction / 10);
            buffer[offset + 7] = (char)('0' + fraction % 10);
        }
    }
}
