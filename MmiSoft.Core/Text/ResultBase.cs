using System;

namespace MmiSoft.Core.Text
{
	public class ResultBase<R>
	{
		private string errorMessage;
		private R value;

		public ResultBase(string errorMessage)
			: this(errorMessage, default)
		{ }

		public ResultBase(R value)
			: this("", value)
		{
			this.value = value;
		}

		private ResultBase(string errorMessage, R value)
		{
			this.errorMessage = errorMessage;
			this.value = value;
		}

		public bool Succeeded => errorMessage == "";

		public R Value => Succeeded ? value : throw new InvalidOperationException("Result is not successful");

		public override string ToString()
		{
			return Succeeded ? "<Success>" : errorMessage;
		}
	}
}
