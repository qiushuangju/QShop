using System;

namespace Utils
{
    public static class DateUtils
    {
        public static string GetTimestamp()
        {
             TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
    }
}