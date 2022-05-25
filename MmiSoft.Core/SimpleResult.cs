using System;

namespace MmiSoft.Core
{
	/// <summary>
	/// Operation result without a value and only an error message. Can be used in cases where an exception is an overkill
	/// and a simple true/false for an operation status is not enough -you also want to know what happened.
	/// </summary>
	[Serializable]
	public class SimpleResult
	{
		/// <summary>
		/// A default <c>Success</c> result. Use it if you want to avoid extra allocations in hot spots
		/// </summary>
		public static readonly SimpleResult Success = new SimpleResult("");

		private readonly string errorMessage;

		/// <summary>
		/// Initializes a result with the specified error message
		/// </summary>
		/// <param name="errorMessage">The error message to convey to the client code</param>
		/// <exception cref="ArgumentNullException">If the <paramref name="errorMessage"/> is null</exception>
		public SimpleResult(string errorMessage)
		{
			this.errorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
		}

		/// <summary>
		/// Returns true if this is a result of an operation that succeeded
		/// </summary>
		public bool Succeeded => errorMessage == "";

		/// <summary>
		/// Will return the error message or (Success) if the operation was successful
		/// </summary>
		/// <returns>The error message or (Success) if the operation was successful</returns>
		public override string ToString()
		{
			return Succeeded ? "(Success)" : errorMessage;
		}
	}
}
