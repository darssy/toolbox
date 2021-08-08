using System;
using MmiSoft.Core.Math;

namespace MmiSoft.Core
{
	public static class DateAndTimeExtensions
	{
		public static DateTime RoundToMinute(this DateTime date)
		{
			double totalMinutes = date.Ticks / (double)TimeSpan.TicksPerMinute;
			return new DateTime((long) (totalMinutes.Round() * TimeSpan.TicksPerMinute));
		}

		public static TimeSpan Magnify(in this TimeSpan span, int factor)
		{
			return new TimeSpan(span.Ticks * factor);
		}
	}
}
