using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MmiSoft.Parser;

public readonly ref struct StringSegment
{
	private readonly string source;
	private readonly int start;
	private readonly int length;

	public StringSegment(string source)
		: this(source, 0, source.Length)
	{
	}

	public StringSegment(string source, int start, int length)
	{
		this.source = source ?? throw new ArgumentNullException(nameof(source));
		if (start < 0) throw new ArgumentException("Negative start argument");
		if (length < 0) throw new ArgumentException("Negative length argument");
		if (start + length > source.Length) throw new ArgumentException("Start + length exceeds source length");
		this.start = start;
		this.length = length;
	}

	public char this[int index]
	{
		get
		{
			if (index >= length) throw new IndexOutOfRangeException($"'{index}' >= '{length}'");
			if (index < 0) index += length;
			return source[start + index];
		}
	}

	public bool Empty => length == 0;

	public int Length => length;

	public bool IsMatch(Regex regex) => regex.Match(source, start, length).Success;

	public bool Equals(string s)
	{
		if (s == null) return false;
		if (s.Length != length) return false;
		for (int i = 0; i < s.Length; i++)
		{
			if (s[i] != source[start + i]) return false;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSegment SubSegment(int from) => new(source, start + from, length - from);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSegment SubSegment(int from, int length) => new(source, start + from, length);

	public StringSegment Trim()
	{
		int relStart = 0;
		int newLength = Length - 1;

		while (relStart < Length && char.IsWhiteSpace(this[relStart]))
		{
			++relStart;
		}

		while (newLength >= relStart && char.IsWhiteSpace(this[newLength]))
		{
			--newLength;
		}

		newLength -= relStart;

		return new StringSegment(source, start + relStart, newLength + 1);
	}

	public int IndexOf(char character)
	{
		int end = start + length;
		for (int i = start; i < end; i++)
		{
			if (source[i] == character) return i - start;
		}

		return -1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(char character) => IndexOf(character) >= 0;

	public override string ToString() => source.Substring(start, length);
}

public static class StringSegmentExtensions
{

	private static readonly int[] PowersOfTenI =
		{ 1, 10, 100, 1000, 10_000, 100_000, 1_000_000, 10_000_000, 100_000_000, 1_000_000_000 };

	private static readonly long[] PowersOfTen;
	private static readonly double[] HighPowersOfTen;

	static StringSegmentExtensions()
	{
		PowersOfTen = new long[19];
		for (int i = 0; i < PowersOfTen.Length; i++)
		{
			PowersOfTen[i] = (long)Math.Pow(10, i);
		}
		HighPowersOfTen = new double[31];
		for (int i = 0; i < HighPowersOfTen.Length; i++)
		{
			HighPowersOfTen[i] = Math.Pow(10, 19 + i);
		}
	}

	public static int ParseInt(this StringSegment dms)
	{
		int d = 0;
		const char zero = '0';
		int digits = dms.Length;
		for (int i = 0; i < dms.Length; i++)
		{
			d += (dms[i] - zero) * PowersOfTenI[digits-- - 1];
		}

		return d;
	}

	public static float ParseFloat(this StringSegment dms, char decimalSeparator = default)
	{
		if (decimalSeparator == default)
		{
			decimalSeparator = CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator[0];
		}

		float d = 0;
		const char zero = '0';
		int digits = dms.Length;
		int fractionalLength = 0;
		for (int i = 0; i < dms.Length; i++)
		{
			if (dms[i] == decimalSeparator)
			{
				if (fractionalLength != 0)
				{
					throw new FormatException("Multiple decimal separators found");
				}

				fractionalLength = dms.Length - i;
			}
			else
			{
				if (dms[i] is < zero or > '9')
				{
					throw new FormatException($"Unexpected token '{dms[i]}' at position {i}");
				}

				d += (dms[i] - zero) * PowersOfTen[digits-- - 1];
			}
		}

		if (fractionalLength != 0)
		{
			d /= PowersOfTen[fractionalLength];
		}

		return d;
	}

	public static double ParseDouble(this StringSegment dms, char decimalSeparator = default)
	{
		if (decimalSeparator == default)
		{
			decimalSeparator = CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator[0];
		}

		double d = 0;
		const char zero = '0';
		bool negative = false;
		if (dms[0] == '-')
		{
			negative = true;
			dms = dms.SubSegment(1);
		}
		if (dms[0] == '0')
		{
			int zeros = 1;
			for (;zeros < dms.Length; zeros++)
			{
				if (dms[zeros] != '0') break;
			}
			dms = dms.SubSegment(zeros);
		}

		int leadingZeros = 0;
		int fractionalLength = -1;
		if (dms[0] == decimalSeparator)
		{
			for (int i = 1; i < dms.Length; i++)
			{
				if (dms[i] != '0') break;
				leadingZeros++;
			}
			fractionalLength = Math.Min(dms.Length - leadingZeros, 17);
			dms = dms.SubSegment(leadingZeros + 1);
		}

		int length = Math.Min(dms.Length, 17);
		int digits = length;
		for (int i = 0; i < length; i++)
		{
			if (dms[i] == decimalSeparator)
			{
				if (fractionalLength != -1)
				{
					throw new FormatException("Multiple decimal separators found");
				}

				fractionalLength = length - i;
				if (length != dms.Length) length++;
			}
			else
			{
				if (dms[i] is < zero or > '9')
				{
					throw new FormatException($"Unexpected token '{dms[i]}' at position {i}");
				}

				d += (dms[i] - zero) * PowersOfTen[digits-- - 1];
			}
		}

		if (fractionalLength != 0)
		{
			d /= PowersOfTen[fractionalLength];
		}
		if (leadingZeros > 0)
		{
			if (leadingZeros < PowersOfTen.Length)
			{
				d /= PowersOfTen[leadingZeros];
			}
			else
			{
				d /= HighPowersOfTen[leadingZeros - PowersOfTen.Length];
			}
		}

		return negative ? -d : d;
	}
}
