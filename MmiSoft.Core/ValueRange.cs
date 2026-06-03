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
		/// Checks if this range intersects with another. Two ranges intersect if they share at least one value,
		/// which includes the case where one range fully contains the other.
		/// </summary>
		/// <param name="range"></param>
		/// <returns>True if the two ranges overlap (bounds included), false otherwise</returns>
		public bool Intersects(ValueRange<T> range)
		{
			if (range == null) return false;
			if (ReferenceEquals(this, range)) return true;

			return minValue.CompareTo(range.maxValue) <= 0 && range.minValue.CompareTo(maxValue) <= 0;
		}

		public override string ToString()
		{
			return $"[{minValue},{maxValue}]";
		}
	}
}
