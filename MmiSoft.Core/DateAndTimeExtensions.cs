using System;

namespace MmiSoft.Core
{
	public static class DateAndTimeExtensions
	{
		public static DateTime RoundToMinute(this DateTime date)
		{
			int second = date.Second;
			if (second >= 30)
			{
				date = date.AddSeconds(60 - second);
			}
			else
			{
				date = date.AddSeconds(-second);
			}
			return date;
		}

		public static TimeSpan Magnify(in this TimeSpan span, int factor)
		{
			return new TimeSpan(span.Ticks * factor);
		}
	}
}
