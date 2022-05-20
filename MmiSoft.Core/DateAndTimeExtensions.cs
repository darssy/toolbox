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

		public static DateTime RoundToSecond(this DateTime date)
		{
			long mod = date.Ticks % TimeSpan.TicksPerSecond;
			return mod >= TimeSpan.TicksPerSecond / 2
				? new DateTime(date.Ticks - mod + TimeSpan.TicksPerSecond)
				: new DateTime(date.Ticks - mod);
		}

		public static DateTime TruncateMillis(this DateTime date)
		{
			long mod = date.Ticks % TimeSpan.TicksPerSecond;
			return new DateTime(date.Ticks - mod, date.Kind);
		}

		public static TimeSpan Magnify(in this TimeSpan span, int factor)
		{
			return new TimeSpan(span.Ticks * factor);
		}
	}
}
