using System;
using System.Threading;

namespace MmiSoft.Core
{
	public class AutoIncrementGenerator
	{
		private volatile int nextDigit;

		public AutoIncrementGenerator()
		{
			nextDigit = 0;
		}

		public int NextDigit()
		{
			return Interlocked.Increment(ref nextDigit);
		}

	}
}
