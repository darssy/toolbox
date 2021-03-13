using System.Drawing;

namespace MmiSoft.Core.Math
{
	using System;

	public static class Extensions
	{
		public static double Abs(this double number)
		{
			return Math.Abs(number);
		}

		public static float Abs(this float number)
		{
			return Math.Abs(number);
		}

		public static int Abs(this int number)
		{
			return Math.Abs(number);
		}
		
		public static decimal Abs(this decimal number)
		{
			return Math.Abs(number);
		}
		
		public static double Sqrt(this double number)
		{
			return Math.Sqrt(number);
		}
		
		public static double Sqrt(this int number)
		{
			return Math.Sqrt(number);
		}

		public static int Round(this float number)
		{
			return (int) Math.Round(number);
		}

		public static float Round(this float number, int decimals)
		{
			return (float) Math.Round(number, decimals);
		}

		public static double Round(this double number)
		{
			return Math.Round(number);
		}

		public static double Round(this double number, int decimals)
		{
			return Math.Round(number, decimals);
		}

		public static int Floor(this double number)
		{
			return (int) Math.Floor(number);
		}

		public static int Ceiling(this double number)
		{
			return (int) Math.Ceiling(number);
		}

		public static Point Center(this Size s)
		{
			return new Point(s.Width / 2, s.Height / 2);
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

		/*public static T InterpolateWith<T>(this SpeedUnit start, SpeedUnit end, Percent percent) where T : SpeedUnit, new()
		{
			double startMps = start.To<MetersPerSecond>().UnitValue;
			double diff = end.To<MetersPerSecond>().UnitValue - startMps;

			double newVal = startMps + diff * percent;
			return new MetersPerSecond(newVal).To<T>();
		}

		public static T[] FillSeries<T>(this T start, T end, int series, Func<double, T> ctor) where T : SpeedUnit
		{
			double startMps = start.UnitValue;
			double diff = end.UnitValue - startMps;
			int totalSteps = series + 1;
			double step = diff / totalSteps;
			T[] result = new T[series];
			result[0] = ctor(start.UnitValue + step);

			for (int i = 1; i < series; i++)
			{
				result[i] = ctor(result[i - 1].UnitValue + step);
			}
			return result;
		}

		public static T Diff<T>(this SpeedUnit speed, SpeedUnit other) where T : SpeedUnit, new()
		{
			double diff = (speed.To<MetersPerSecond>().UnitValue - other.To<MetersPerSecond>().UnitValue).Round();
			return new MetersPerSecond(diff).To<T>();
		}*/

		public static double Square(this double number)
		{
			return number * number;
		}

		public static int Square(this int number)
		{
			return number * number;
		}

		public static bool AlmostEqual(this double x, double y) {
			double epsilon = Math.Max(Math.Abs(x), Math.Abs(y)) * 1E-14;
			return Math.Abs(x - y) <= epsilon;
		}

		public static bool AlmostEqual(this float left, float right, float tolerance=0.000001f)
		{
			return (left - right).Abs() < tolerance;
		}

		public static T Max<T>(this T first, T second) where T: IComparable<T>
		{
			return first.CompareTo(second) >= 0 ? first : second;
		}

		public static T Min<T>(this T first, T second) where T: IComparable<T>
		{
			return first.CompareTo(second) <= 0 ? first : second;
		}
	}
}
