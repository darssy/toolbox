using System;
using System.Threading;

namespace MmiSoft.Core
{
	/// <summary>
	/// A simple class that generates sequential integers. It's a wrapper to Interlocked.Increment
	/// </summary>
	public class AutoIncrementGenerator
	{
		private int currentDigit;

		/// <summary>
		/// Initializes an autoincrement generator starting from 1
		/// </summary>
		public AutoIncrementGenerator()
		{
		}

		/// <summary>
		/// This method returns the next digit of the sequence after each call. As a result it violates CQRS and it's not pure.
		/// </summary>
		/// <returns>The next digit of the sequence</returns>
		public int NextDigit() => Interlocked.Increment(ref currentDigit);

	}
}
