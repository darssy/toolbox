using System;

namespace MmiSoft.Core
{
	public readonly ref struct Result<R, E> where E : class
	{
		private readonly E error;
		private readonly R value;

		public Result(E error)
		{
			this.error = error ?? throw new ArgumentException("Error state can't be null -it will \"impersonate\" Success");
			value = default;
		}

		public Result(R value)
		{
			this.value = value;
			error = null;
		}

		public bool Succeeded => error == null;

		public R Value => Succeeded ? value : throw new InvalidOperationException("Result is not successful");

		public override string ToString()
		{
			return Succeeded ? "<Success>" : error.ToString();
		}
	}
}
