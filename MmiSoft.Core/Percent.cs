using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using MmiSoft.Core.Math;

namespace MmiSoft.Core
{
	public readonly struct Percent : IComparable<Percent>, IFormattable
	{
		public static readonly Percent Zero = new Percent(0);
		public static readonly Percent Hundred = new Percent(100);

		private readonly float value;

		private Percent(float value, bool _)
		{
			this.value = value;
		}

		public Percent(float value)
		{
			this.value = value / 100;
		}

		public Percent(double value)
		{
			this.value = (float) (value / 100);
		}

		public float Value => value;
		
		public static decimal operator * (decimal l, in Percent r)
		{
			return l * new decimal(r.value);
		}

		public static double operator * (double l, in Percent r)
		{
			return l * r.value;
		}

		public static float operator * (float l, in Percent r)
		{
			return l * r.value;
		}

		public static float operator * (int l, in Percent r)
		{
			return l * r.value;
		}

		public static decimal operator / (decimal l, in Percent r)
		{
			return l / new decimal(r.value);
		}

		public static double operator / (double l, in Percent r)
		{
			return l / r.value;
		}

		public static float operator / (float l, in Percent r)
		{
			return l / r.value;
		}

		public static float operator / (int l, in Percent r)
		{
			return l / r.value;
		}

		public static decimal operator + (decimal l, in Percent r)
		{
			return l * new decimal(1 + r.value);
		}

		public static double operator + (double l, in Percent r)
		{
			return l * (1 + r.value);
		}

		public static float operator + (float l, in Percent r)
		{
			return l * (1 + r.value);
		}

		public static float operator + (int l, in Percent r)
		{
			return l * (1 + r.value);
		}

		public static Percent operator - (Percent l, in Percent r)
		{
			return new Percent(l.value - r.value, false);
		}

		public static bool operator ==(in Percent left, in Percent right)
		{
			return left.value.AlmostEqual(right.value);
		}

		public static bool operator !=(in Percent left, in Percent right)
		{
			return !left.value.AlmostEqual(right.value);
		}

		public override bool Equals(object obj)
		{
			return obj is Percent percent && percent.value.AlmostEqual(value);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode() * 113;
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return value.ToString(format, formatProvider);
		}

		public override string ToString()
		{
			return $"p:{value}";
		}

		public string GetDisplayValue()
		{
			//the real percent value is recomputed to keep the struct short
			return $"{value * 100:0.##}%";
		}

		public int CompareTo(Percent other)
		{
			return value.CompareTo(other.value);
		}

		public static bool operator <(Percent left, Percent right)
		{
			return left.value < right.value;
		}

		public static bool operator >(Percent left, Percent right)
		{
			return left.value > right.value;
		}

		public static bool operator <=(Percent left, Percent right)
		{
			return left.value <= right.value;
		}

		public static bool operator >=(Percent left, Percent right)
		{
			return left.value >= right.value;
		}

		/// <summary>
		/// Creates a percent from a coefficient. eg 30% from 1,3
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Percent FromDecimal(double value)
		{
			return new Percent((value - 1) * 100);
		}

		private static Dictionary<IFormatProvider, Regex> Validators = new Dictionary<IFormatProvider, Regex>();

		public static Regex GetValidator(IFormatProvider provider)
		{
			NumberFormatInfo formatInfo = NumberFormatInfo.GetInstance(provider);
			return Validators.GetOrCreate(provider, formatInfo,
				info => new Regex($@"^-?\d+({info.PercentDecimalSeparator}\d+)?{info.PercentSymbol}$"));
		}

		public static Percent Parse(string text, IFormatProvider provider)
		{
			NumberFormatInfo formatInfo = NumberFormatInfo.GetInstance(provider);
			Regex validator = GetValidator(provider);
			if (!validator.IsMatch(text))
			{
				throw new FormatException($"Text '{text}' does not match a fixed-point format (\"F\") with a trailing % sign");
			}
			text = text.TrimEnd(formatInfo.PercentSymbol[0]);

			float percent = float.Parse(text, NumberStyles.Number, formatInfo);

			return new Percent(percent);
		}
	}
}
