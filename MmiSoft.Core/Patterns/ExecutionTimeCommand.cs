using System;
using System.Diagnostics;

namespace MmiSoft.Core.Patterns
{
	public class ExecutionTimeCommand : ICommand
	{
		private ICommand decorated;
		private TimeSpan threshold;
		private string userText; 
		private Action<string> logConsumer;

		private static readonly Action<string> ConsoleWriteLine = Console.WriteLine;

		public ExecutionTimeCommand(ICommand decorated, TimeSpan threshold, Action<string> logConsumer = null, string userText = null)
		{
			this.decorated = decorated;
			this.threshold = threshold;
			this.userText = userText;
			this.logConsumer = logConsumer ?? ConsoleWriteLine;
		}

		public void Execute()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			try
			{
				decorated.Execute();
			}
			finally
			{
				stopwatch.Stop();
			}
			if (stopwatch.Elapsed >= threshold)
			{
				logConsumer($"Executed after {stopwatch.Elapsed} ({this})");
			}
		}

		public override string ToString()
		{
			return userText ?? base.ToString();
		}
	}
}
