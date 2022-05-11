using System;

namespace MmiSoft.Core
{
	/// <summary>
	/// Operation result without a value and only an error message.
	/// </summary>
	[Serializable]
	public class SimpleResult
	{
		public static readonly SimpleResult Success = new SimpleResult("");

		private readonly string errorMessage;

		public SimpleResult(string errorMessage)
		{
			this.errorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
		}

		public bool Succeeded => errorMessage == "";

		public override string ToString()
		{
			return Succeeded ? "(Success)" : errorMessage;
		}
	}
}
