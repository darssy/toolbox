using System;

namespace MmiSoft.Core
{
    [Serializable]
	public class ResultBase<R>
	{
		private string errorMessage;
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

		private ResultBase(string errorMessage, R value)
		{
			this.errorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
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
