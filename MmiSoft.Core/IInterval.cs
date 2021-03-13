using System;

namespace MmiSoft.Core
{
	public interface IInterval<T>
	{
		bool Contains(T value);
		T Closest(T value);
		//Temporary value until altitude selection is finalized
		T Lower {get;}
	}

	public abstract class InfiniteEndpointInterval<T> : IInterval<T> where T: IComparable<T>
	{
		private T endpoint;
		private string format;

		protected InfiniteEndpointInterval(T endpoint, string format)
		{
			this.endpoint = endpoint;
			this.format = format;
		}

		public T Endpoint => endpoint;

		public abstract bool Contains(T value);

		public virtual T Closest(T value)
		{
			return Contains(value) ? value : Endpoint;
		}

		public T Lower => endpoint;

		public override string ToString()
		{
			return string.Format(format, endpoint);
		}
	}

	public class LessThanInterval<T> : InfiniteEndpointInterval<T> where T: IComparable<T>
	{
		public LessThanInterval(T endpoint, string format="[-\u221E,{0}]") : base(endpoint, format)
		{ }

		public override bool Contains(T value)
		{
			return value.CompareTo(Endpoint) <= 0;
		}
	}

	public class GreaterThanInterval<T> : InfiniteEndpointInterval<T> where T: IComparable<T>
	{

		public GreaterThanInterval(T endpoint, string format="[{0},\u221E]") : base(endpoint, format)
		{ }

		public override bool Contains(T value)
		{
			return value.CompareTo(Endpoint) >= 0;
		}
	}

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

		public T Lower => lower.Endpoint;

		public override string ToString()
		{
			return string.Format(format, lower.Endpoint, upper.Endpoint);
		}
	}

	public class DegenerateInterval<T> : InfiniteEndpointInterval<T> where T: IComparable<T>
	{
		public DegenerateInterval(T endpoint, string format="[{0},{0}]") : base(endpoint, format)
		{
		}

		public override bool Contains(T value)
		{
			return value.CompareTo(Endpoint) == 0;
		}

		public override T Closest(T value)
		{
			return Endpoint;
		}
	}
}
