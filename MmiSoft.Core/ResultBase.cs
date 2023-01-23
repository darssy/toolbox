using System;

namespace MmiSoft.Core
{
	[Serializable]
	public class ResultBase<R> : SimpleResult, IResult<R>
	{
		private R value;

		public ResultBase(string errorMessage)
			: this(errorMessage, default)
		{
			if (errorMessage == "")
			{
				throw new ArgumentException("Error message can't be empty string -it will \"impersonate\" Success");
			}
		}

		public ResultBase(R value)
			: this("", value)
		{
			this.value = value;
		}

		private ResultBase(string errorMessage, R value) : base(errorMessage)
		{
			this.value = value;
		}

		public R Value => Succeeded ? value : throw new InvalidOperationException("Result is not successful");
	}

	public interface IResult<out R>
	{
		R Value { get; }
		bool Succeeded { get; }
	}
}
