using System;

namespace NinjaThea.Util
{
    public static class TimeFormat
    {
        // mm:ss.ff
        public static string Format(float seconds)
        {
            return TimeSpan.FromSeconds(seconds).ToString("mm':'ss'.'ff");
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
