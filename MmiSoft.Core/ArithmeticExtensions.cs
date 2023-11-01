using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace MmiSoft.Core
{
	public static class ArithmeticExtensions
	{
		public static bool Between<T>(this T number, T a, T b) where T : IComparable<T>
		{
			return number.CompareTo(a) <= 0 && number.CompareTo(b) >= 0
			       || number.CompareTo(b) <= 0 && number.CompareTo(a) >= 0;
		}

		public static bool Between(this char character, char a, char b)
		{
			return character <= a && character >= b || character <= b && character >= a;
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

		public static bool Between(this TimeSpan t, TimeSpan a, TimeSpan b)
		{
			return t <= a && t >= b || t <= b && t >= a;
		}

		#region RangeChecking

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AssertWithin(this byte number, byte min, byte max)
		{
			if (number < min || number > max)
				throw new ArgumentOutOfRangeException(nameof(number), $"Value is outside expected range [{min}, {max}]");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsWithin(this byte b, byte min, byte max)
		{
			return b >= min && b <= max;
		}

#if NET7_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsWithin<T>(this T number, T min, T max) where T : INumber<T>
		{
			return number >= min && number <= max;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AssertWithin<T>(this T number, T min, T max) where T : INumber<T>
		{
			if (number < min || number > max)
			{
				throw new ArgumentOutOfRangeException(nameof(number), $"Value is outside expected range [{min}, {max}]");
			}
		}
#else

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AssertWithin(this decimal number, decimal min, decimal max)
		{
			if (number < min || number > max)
				throw new ArgumentOutOfRangeException(nameof(number), $"Value is outside expected range [{min}, {max}]");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsWithin(this decimal number, decimal min, decimal max)
		{
			return number >= min && number <= max;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AssertWithin(this double number, double min, double max)
		{
			if (number < min || number > max)
				throw new ArgumentOutOfRangeException(nameof(number), $"Value is outside expected range [{min}, {max}]");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsWithin(this double number, double min, double max)
		{
			return number >= min && number <= max;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AssertWithin(this float number, float min, float max)
		{
			if (number < min || number > max)
				throw new ArgumentOutOfRangeException(nameof(number), $"Value is outside expected range [{min}, {max}]");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsWithin(this float number, float min, float max)
		{
			return number >= min && number <= max;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AssertWithin(this int number, int min, int max)
		{
			if (number < min || number > max)
				throw new ArgumentOutOfRangeException(nameof(number), $"Value is outside expected range [{min}, {max}]");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsWithin(this int number, int min, int max)
		{
			return number >= min && number <= max;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AssertWithin(this long number, long min, long max)
		{
			if (number < min || number > max)
				throw new ArgumentOutOfRangeException(nameof(number), $"Value is outside expected range [{min}, {max}]");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsWithin(this long number, long min, long max)
		{
			return number >= min && number <= max;
		}

#endif

		#endregion
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

		public static Percent Percent(this int value) => new Percent(value);
	}
}
