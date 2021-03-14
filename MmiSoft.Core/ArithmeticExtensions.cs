using System;

namespace MmiSoft.Core
{
	public static class ArithmeticExtensions
	{
		public static bool Between<T>(this T number, T a, T b) where T : IComparable<T>
		{
			return number.CompareTo(a) <= 0 && number.CompareTo(b) >= 0
			       || number.CompareTo(b) <= 0 && number.CompareTo(a) >= 0;
		}

		public static bool Between(this int number, int a, int b)
		{
			return number <= a && number >= b || number <= b && number >= a;
		}

		public static bool Between(this float number, float a, float b)
		{
			return number <= a && number >= b || number <= b && number >= a;
		}

		public static bool Between(this decimal number, decimal a, decimal b)
		{
			return number <= a && number >= b || number <= b && number >= a;
		}

		public static bool Between(this TimeSpan number, TimeSpan a, TimeSpan b)
		{
			return number <= a && number >= b || number <= b && number >= a;
		}

		public static Percent PercentBetween(this int value, int from, int to)
		{
			float dif = value - from;
			return new Percent(dif * 100 / (to - from));
		}

		public static float[] FillSeries(this float start, float end, int series)
		{
			int totalSteps = series + 1;
			float step = (end - start) / totalSteps;
			float[] result = new float[series];
			result[0] = start + step;
			for (int i = 1; i < series; i++)
			{
				result[i] = result[i - 1] + step;
			}
			return result;
		}

		public static double InterpolateWith(this int start, double end, Percent percent)
		{
			double diff = end - start;

			return start + diff * percent;
		}

		public static T Max<T>(this T first, T second) where T: IComparable<T>
		{
			return first.CompareTo(second) >= 0 ? first : second;
		}

		public static T Min<T>(this T first, T second) where T: IComparable<T>
		{
			return first.CompareTo(second) < 0 ? first : second;
		}
	}
}
