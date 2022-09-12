using System;

namespace MmiSoft.Core
{
	/// <summary>
	/// Represents a mathematical interval abstraction like for example (-∞,10] or [-3,70] etc
	/// </summary>
	/// <typeparam name="T"></typeparam>
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

		/// <summary>
		/// Returns whether the value is contained by the interval or not. eg 10 in [10, 20] returns true, 21 returns
		/// false 13 returns true and so on and so forth
		/// </summary>
		/// <param name="value">The value to check against the interval</param>
		/// <returns>true if value is contained otherwise false</returns>
		bool Contains(T value);
		
		/// <summary>
		/// Returns the closes value to that interval. If the value is contained it returns the value. Examples
		/// <list type="bullet">
		/// <item><description>[10, 20].Closest(7) returns 10</description></item>
		/// <item><description>[10, 20].Closest(30) returns 30</description></item>
		/// <item><description>[10, 20].Closest(17) returns 17</description></item>
		/// </list>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		T Closest(T value);
	}

	/// <summary>
	/// Abstraction that represents either a (-∞,x] or a [x,∞) interval
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public abstract class InfiniteEndpointInterval<T> : IInterval<T> where T: IComparable<T>
	{
		public static readonly IInterval<T> Any = new Any<T>();
		public static readonly IInterval<T> None = new None<T>();

		private T endpoint;
		private string format;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="format"></param>
		protected InfiniteEndpointInterval(T endpoint, string format)
		{
			this.endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
			this.format = format ?? throw new ArgumentNullException(nameof(format));
		}

		/// <summary>
		/// 
		/// </summary>
		public T Endpoint => endpoint;

		/// <inheritdoc />
		public abstract bool IsBeforeValue(T value);

		/// <inheritdoc />
		public abstract bool IsAfterValue(T value);

		/// <inheritdoc />
		public abstract bool Contains(T value);

		/// <inheritdoc />
		public virtual T Closest(T value)
		{
			return Contains(value) ? value : Endpoint;
		}

		/// <inheritdoc />
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

		/// <inheritdoc />
		public override bool IsBeforeValue(T value) => false;

		/// <inheritdoc />
		public override bool IsAfterValue(T value) => !Contains(value);

		/// <inheritdoc />
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

		/// <inheritdoc />
		public override bool IsBeforeValue(T value) => !Contains(value);

		/// <inheritdoc />
		public override bool IsAfterValue(T value) => false;

		/// <inheritdoc />
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
			this.lower = lower ?? throw new ArgumentNullException(nameof(lower));
			this.upper = upper ?? throw new ArgumentNullException(nameof(upper));
			this.format = format ?? throw new ArgumentNullException(nameof(format));
		}

		public BoundedInterval(T lower, T upper, string format="[{0},{1}]")
		{
			if (lower.CompareTo(upper) > 0) Util.Swap(ref lower, ref upper);
			this.lower = new GreaterThanInterval<T>(lower);
			this.upper = new LessThanInterval<T>(upper);
			this.format = format;
		}

		/// <inheritdoc />
		public bool IsBeforeValue(T value) => lower.IsBeforeValue(value);

		/// <inheritdoc />
		public bool IsAfterValue(T value) => upper.IsAfterValue(value);

		/// <inheritdoc />
		public bool Contains(T value)
		{
			return lower.Contains(value) && upper.Contains(value);
		}

		/// <inheritdoc />
		public T Closest(T value)
		{
			if (!lower.Contains(value)) return lower.Endpoint;
			if (!upper.Contains(value)) return upper.Endpoint;
			return value;
		}

		/// <inheritdoc />
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
			this.endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
			this.format = format ?? throw new ArgumentNullException(nameof(format));
		}

		/// <inheritdoc />
		public bool IsBeforeValue(T value) => endpoint.CompareTo(value) < 0;

		/// <inheritdoc />
		public bool IsAfterValue(T value) => endpoint.CompareTo(value) > 0;

		/// <inheritdoc />
		public bool Contains(T value) => endpoint.CompareTo(value) == 0;

		/// <inheritdoc />
		public T Closest(T value) => endpoint;

		/// <inheritdoc />
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

		/// <inheritdoc />
		public bool IsBeforeValue(T value) => false;

		/// <inheritdoc />
		public bool IsAfterValue(T value) => false;

		/// <inheritdoc />
		public bool Contains(T value) => true;

		/// <inheritdoc />
		public T Closest(T value) => value;

		/// <inheritdoc />
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

		/// <inheritdoc />
		public bool IsBeforeValue(T value) => true;

		/// <inheritdoc />
		public bool IsAfterValue(T value) => true;

		/// <inheritdoc />
		public bool Contains(T value) => false;

		/// <inheritdoc />
		public T Closest(T value) => default;

		/// <inheritdoc />
		public override string ToString() => displayText;
	}
}
