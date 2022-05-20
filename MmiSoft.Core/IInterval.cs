using System;

namespace MmiSoft.Core
{
	public interface IInterval<T>
	{
		/// <summary>
		/// Checks if the endpoint of the interval is before the value in the argument. eg 4 in [1,4] is before 5
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool IsBeforeValue(T value);

		/// <summary>
		/// Checks if the start point of the interval is after the value in the argument. eg 1 in [1,4] is after -2
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool IsAfterValue(T value);

		bool Contains(T value);
		T Closest(T value);
	}

	[Serializable]
	public abstract class InfiniteEndpointInterval<T> : IInterval<T> where T: IComparable<T>
	{
		public static readonly IInterval<T> Any = new Any<T>();
		public static readonly IInterval<T> None = new None<T>();

		private T endpoint;
		private string format;

		protected InfiniteEndpointInterval(T endpoint, string format)
		{
			this.endpoint = endpoint;
			this.format = format;
		}

		public T Endpoint => endpoint;

		public abstract bool IsBeforeValue(T value);
		public abstract bool IsAfterValue(T value);
		public abstract bool Contains(T value);

		public virtual T Closest(T value)
		{
			return Contains(value) ? value : Endpoint;
		}

		public override string ToString()
		{
			return string.Format(format, endpoint);
		}
	}

	[Serializable]
	public class LessThanInterval<T> : InfiniteEndpointInterval<T> where T: IComparable<T>
	{
		public LessThanInterval(T endpoint, string format="(-\u221E,{0}]") : base(endpoint, format)
		{ }

		public override bool IsBeforeValue(T value) => false;
		public override bool IsAfterValue(T value) => !Contains(value);

		public override bool Contains(T value)
		{
			return value.CompareTo(Endpoint) <= 0;
		}
	}

	[Serializable]
	public class GreaterThanInterval<T> : InfiniteEndpointInterval<T> where T: IComparable<T>
	{

		public GreaterThanInterval(T endpoint, string format="[{0},\u221E)") : base(endpoint, format)
		{ }

		public override bool IsBeforeValue(T value) => !Contains(value);
		public override bool IsAfterValue(T value) => false;

		public override bool Contains(T value)
		{
			return value.CompareTo(Endpoint) >= 0;
		}
	}

	[Serializable]
	public class BoundedInterval<T> : IInterval<T> where T: IComparable<T>
	{
		private GreaterThanInterval<T> lower;
		private LessThanInterval<T> upper;
		private string format;

		public BoundedInterval(GreaterThanInterval<T> lower, LessThanInterval<T> upper, string format="[{0},{1}]")
		{
			this.lower = lower;
			this.upper = upper;
			this.format = format;
		}

		public BoundedInterval(T lower, T upper, string format="[{0},{1}]")
		{
			if (lower.CompareTo(upper) > 0) Util.Swap(ref lower, ref upper);
			this.lower = new GreaterThanInterval<T>(lower);
			this.upper = new LessThanInterval<T>(upper);
			this.format = format;
		}

		public bool IsBeforeValue(T value) => lower.IsBeforeValue(value);
		public bool IsAfterValue(T value) => upper.IsAfterValue(value);

		public bool Contains(T value)
		{
			return lower.Contains(value) && upper.Contains(value);
		}

		public T Closest(T value)
		{
			if (!lower.Contains(value)) return lower.Endpoint;
			if (!upper.Contains(value)) return upper.Endpoint;
			return value;
		}

		public override string ToString()
		{
			return string.Format(format, lower.Endpoint, upper.Endpoint);
		}
	}

	/// <summary>
	/// Interval matching an exact value. eg [32,32]
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class DegenerateInterval<T> : IInterval<T> where T: IComparable<T>
	{
		private T endpoint;
		private string format;

		public DegenerateInterval(T endpoint, string format="[{0},{0}]")
		{
			this.endpoint = endpoint;
			this.format = format;
		}

		public bool IsBeforeValue(T value) => endpoint.CompareTo(value) < 0;
		public bool IsAfterValue(T value) => endpoint.CompareTo(value) > 0;

		public bool Contains(T value) => endpoint.CompareTo(value) == 0;

		public T Closest(T value) => endpoint;

		public override string ToString()
		{
			return string.Format(format, endpoint);
		}
	}

	/// <summary>
	/// Equivalent to (-∞,∞)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class Any<T>: IInterval<T>
	{
		private string displayText;
		public Any(string displayText = "(-\u221E,\u221E)")
		{
			this.displayText = displayText;
		}

		public bool IsBeforeValue(T value) => false;

		public bool IsAfterValue(T value) => false;

		public bool Contains(T value) => true;

		public T Closest(T value) => value;

		public override string ToString() => displayText;
	}

	/// <summary>
	/// Equivalent to {∅}
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class None<T>: IInterval<T>
	{
		private string displayText;
		public None(string displayText = "{∅}")
		{
			this.displayText = displayText;
		}

		public bool IsBeforeValue(T value) => true;

		public bool IsAfterValue(T value) => true;

		public bool Contains(T value) => false;

		public T Closest(T value) => default;

		public override string ToString() => displayText;
	}
}
