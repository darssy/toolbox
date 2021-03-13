using System;

namespace MmiSoft.Core
{
	public sealed class ValueRange<T> where T : IComparable<T>
	{
		private T minValue;
		private T maxValue;

		public ValueRange(T min, T max)
		{
			if (max.CompareTo(min) < 0)
			{
				Util.Swap(ref min, ref max);
			}
			minValue = min;
			maxValue = max;
		}

		public T MinValue => minValue;

		public T MaxValue => maxValue;

		public bool IsInRange(T value)
		{
			return minValue.CompareTo(value) <= 0 && maxValue.CompareTo(value) >= 0;
		}

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
