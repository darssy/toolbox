using System;

namespace MmiSoft.Core.Logging
{
	public class ConsoleLogWrapper : ILogWrapper
	{
		public LogSeverity LogLevel { get; set; }

		public void Log(LogSeverity severity, string message, string category)
		{
			if (severity > LogLevel) return;

			if (severity <= LogSeverity.Error)
			{
				Console.Error.WriteLine($"{severity} {category}\t{message}");
			}
			else
			{
				Console.WriteLine($"{severity} {category}\t{message}");
			}
		}

		public void Log(LogSeverity severity, Func<string> messageProvider, string category)
		{
			if (severity > LogLevel) return;

			if (severity <= LogSeverity.Error)
			{
				Console.Error.WriteLine($"{severity} {category}\t{messageProvider.Invoke()}");
			}
			else
			{
				Console.WriteLine($"{severity} {category}\t{messageProvider.Invoke()}");
			}
		}

		public void Exception(Exception e, LogSeverity severity, string message, string category)
		{
			if (severity > LogLevel) return;

			if (severity <= LogSeverity.Error)
			{
				Console.Error.WriteLine($"{severity} {category}\t{message}");
				Console.Error.WriteLine(e.StackTrace);
			}
			else
			{
				Console.WriteLine($"{severity} {category}\t{message}");
				Console.WriteLine(e.StackTrace);
			}
		}
	}
}
