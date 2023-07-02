using System;

namespace MmiSoft.Core
{
	/// <summary>
	/// A simple value range between 2 comparable values. Not to be confused with the C# 8 ranges API.
	/// </summary>
	/// <typeparam name="T">Any type that implements Comparable</typeparam>
	[Serializable]
	public sealed class ValueRange<T> where T : IComparable<T>
	{
		private T minValue;
		private T maxValue;

		/// <summary>
		/// Constructs a new Range with specific min and max. If max &lt; min then min and max are swapped.
		/// </summary>
		/// <param name="min">Lowest bound of range, inclusive</param>
		/// <param name="max">highest bound of range, inclusive</param>
		/// <exception cref="ArgumentNullException">If min or max is null</exception>
		public ValueRange(T min, T max)
		{
			if (min == null) throw new ArgumentNullException(nameof(min));
			if (max == null) throw new ArgumentNullException(nameof(max));
			if (max.CompareTo(min) < 0)
			{
				Util.Swap(ref min, ref max);
			}
			minValue = min;
			maxValue = max;
		}

		public T MinValue => minValue;

		public T MaxValue => maxValue;

		/// <summary>
		/// Checks if the argument is between the min and max of this range, bounds are included.
		/// </summary>
		/// <param name="value">The value to check</param>
		/// <returns>True if value &lt;= max and value &gt;= min, false otherwise</returns>
		public bool IsInRange(T value)
		{
			return minValue.CompareTo(value) <= 0 && maxValue.CompareTo(value) >= 0;
		}

		/// <summary>
		/// Checks if this range intersects with another.
		/// </summary>
		/// <param name="range"></param>
		/// <returns>True if either min or max of range are between min and max of this range. False otherwise</returns>
		public bool Intersects(ValueRange<T> range)
		{
			if (range == null) return false;
			if (ReferenceEquals(this, range)) return true;

			return IsInRange(range.minValue) || IsInRange(range.maxValue);
		}

		public override string ToString()
		{
			return $"[{minValue},{maxValue}]";
		}
	}
}
