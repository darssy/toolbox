using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using MmiSoft.Core.Math;

namespace MmiSoft.Core
{
	/// <summary>
	/// A struct to represent a percent value. Technically a pretty wrapper around a floating point number
	/// </summary>
	public readonly struct Percent : IComparable<Percent>, IFormattable
	{
		public static readonly Percent Zero = new Percent(0);
		public static readonly Percent Hundred = new Percent(100);

		private readonly float value;

		private Percent(float value, bool _)
		{
			this.value = value;
		}

		/// <summary>
		/// Creates an instance of Percent from the value. Note that the value is the percent itself. If the value is
		/// 73,45 you will get a 73,45% at hand. If you already have the fractional 0,7345 and want to create a percent
		/// from this don't multiply it by 100 -if performance is important. Use the <see cref="FromFractional(float)"/>
		/// static method that will avoid this back and forth.
		/// </summary>
		/// <param name="value">The value representing a percent. Negative and over 100 is allowed</param>
		public Percent(float value)
		{
			this.value = value / 100;
		}

		/// <summary>
		/// Creates an instance of Percent from the value. Note that the value is the percent itself. If the value is
		/// 73,45 you will get a 73,45% at hand. If you already have the fractional 0,7345 and want to create a percent
		/// from this don't multiply it by 100 -if performance is important. Use the <see cref="FromFractional(double)"/>
		/// static method that will avoid this back and forth.
		/// </summary>
		/// <param name="value">The value representing a percent. Negative and over 100 is allowed</param>
		public Percent(double value)
		{
			this.value = (float) (value / 100);
		}

		/// <summary>
		/// Returns the raw value of this percent. For 20% it will be 0.2, for 100% 1 etc.
		/// </summary>
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
			return $"Percent raw value: '{value}' computed {GetDisplayValue()}";
		}

		/// <summary>
		/// Convenience method for calling <c>ToString("P", CultureInfo.CurrentCulture);</c>
		/// </summary>
		/// <returns></returns>
		public string GetDisplayValue()
		{
			return ToString("P", CultureInfo.CurrentCulture);
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
		/// Creates a percent from a coefficient. eg 30% from 1,3 or -10% from 0.9
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Percent FromCoefficient(double value)
		{
			return new Percent((float)value - 1, false);
		}

		/// <summary>
		/// Creates a percent from a fractional value. eg 30% from 0,3 or 90% from 0.9. Use that if you already have the
		/// fractional on hand and want to avoid multiplying by 100 only to have the constructor to divide again by 100
		/// to create the raw value.
		/// </summary>
		/// <param name="value">The fractional value to create the percent from</param>
		/// <returns>A Percent representing the fractional value</returns>
		public static Percent FromFractional(double value)
		{
			return new Percent((float)value, false);
		}

		/// <summary>
		/// Creates a percent from a fractional value. eg 30% from 0,3 or 90% from 0.9. Use that if you already have the
		/// fractional on hand and want to avoid multiplying by 100 only to have the constructor to divide again by 100
		/// to create the raw value.
		/// </summary>
		/// <param name="value">The fractional value to create the percent from</param>
		/// <returns>A Percent representing the fractional value</returns>
		public static Percent FromFractional(float value)
		{
			return new Percent(value, false);
		}

		private static Dictionary<IFormatProvider, Regex> Validators = new Dictionary<IFormatProvider, Regex>();

		public static Regex GetValidator(IFormatProvider provider)
		{
			NumberFormatInfo formatInfo = NumberFormatInfo.GetInstance(provider);
			return Validators.GetOrCreate(provider, formatInfo,
				info => new Regex($@"^-?[0-9]+({info.PercentDecimalSeparator}[0-9]+)?{info.PercentSymbol}$"));
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
