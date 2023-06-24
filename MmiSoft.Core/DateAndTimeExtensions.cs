using System;
using System.Runtime.CompilerServices;
using MmiSoft.Core.Math;

namespace MmiSoft.Core
{
	public static class DateAndTimeExtensions
	{
		/// <summary>
		/// Rounds a DateTime to the nearest minute.
		/// </summary>
		/// <param name="date">The date to round</param>
		/// <returns>The date rounded to the closest minute</returns>
		/// <example>
		/// <c>12:45:39</c> becomes <c>12:46:00</c> <c>21:03:20</c> becomes <c>21:03:00</c> etc.
		/// </example>
		public static DateTime RoundToMinute(this DateTime date)
		{
			double totalMinutes = date.Ticks / (double)TimeSpan.TicksPerMinute;
			return new DateTime((long) (totalMinutes.Round() * TimeSpan.TicksPerMinute));
		}

		/// <summary>
		/// Rounds a DateTime to the nearest second.
		/// </summary>
		/// <param name="date">The date to round</param>
		/// <returns>The date rounded to the closest second</returns>
		/// <example>
		/// <c>12:45:39.501</c> becomes <c>12:45:40</c> <c>21:03:20.394</c> becomes <c>21:03:20</c> etc.
		/// </example>
		public static DateTime RoundToSecond(this DateTime date)
		{
			long mod = date.Ticks % TimeSpan.TicksPerSecond;
			return mod >= TimeSpan.TicksPerSecond / 2
				? new DateTime(date.Ticks - mod + TimeSpan.TicksPerSecond)
				: new DateTime(date.Ticks - mod);
		}

		/// <summary>
		/// Truncates the milliseconds of a DateTime.
		/// </summary>
		/// <param name="date">The date to truncated</param>
		/// <returns>The date with zero milliseconds</returns>
		/// <example>
		/// <c>12:45:39.795</c> becomes <c>12:45:39</c> <c>21:03:20.394</c> becomes <c>21:03:20</c> etc.
		/// </example>
		public static DateTime TruncateMillis(this DateTime date)
		{
			long mod = date.Ticks % TimeSpan.TicksPerSecond;
			return new DateTime(date.Ticks - mod, date.Kind);
		}

		/// <summary>
		/// Magnifies a time span by a specific factor
		/// </summary>
		/// <param name="span">The time span to magnify</param>
		/// <param name="factor">The factor to magnify the span</param>
		/// <returns>The magnified span</returns>
		/// <example>
		/// <c>00:12:45</c> magnified by 3 becomes <c>00:38:15</c>
		/// </example>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeSpan Magnify(in this TimeSpan span, int factor)
		{
			return new TimeSpan(span.Ticks * factor);
		}
	}
}
