using System;
namespace XYS.Utility.Common
{
    public class DateTimeUtil
    {
        public DateTime DateAddTime(DateTime date, DateTime time)
        {
            date = date.AddHours(time.Hour);
            date = date.AddMinutes(time.Minute);
            date = date.AddSeconds(time.Second);
            date = date.AddMilliseconds(time.Millisecond);
            return date;
        }
        public string Format(DateTime dt, string formatter, string emptyLabel)
        {
            if (dt == DateTime.MinValue)
            {
                return emptyLabel;
            }
            else
            {
                return dt.ToString(formatter);
            }
        }
        public string Format(DateTime dt, string formatter)
        {
            return Format(dt, formatter, "");
        }
    }
}
