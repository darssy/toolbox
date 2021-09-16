using System;
using System.Diagnostics;

namespace MmiSoft.Core.Patterns
{
	public class ExecutionTimeGetterDecorator<T, P>
	{
		private Func<P, T> decorated;
		private TimeSpan threshold;
		private string userText; 
		private Action<string> logConsumer;

		public ExecutionTimeGetterDecorator(Func<P, T> decorated, TimeSpan threshold, Action<string> logConsumer = null, string userText = null)
		{
			this.decorated = decorated;
			this.threshold = threshold;
			this.userText = userText;
			this.logConsumer = logConsumer??Console.WriteLine;
		}
		
		public T Execute(P parameter)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			try
			{
				return decorated(parameter);
			}
			finally
			{
				stopwatch.Stop();
				if (stopwatch.Elapsed >= threshold)
				{
					logConsumer($"Executed after {stopwatch.Elapsed} ({this})");
				}
			}
		}

		public override string ToString()
		{
			return userText ?? base.ToString();
		}
	}
}
