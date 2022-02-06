using System;

namespace MmiSoft.Core.Logging
{
	public interface ILogWrapper
	{
		LogSeverity LogLevel { get; set; }
		void Log(LogSeverity severity, string message, string category);
		void Log(LogSeverity severity, Func<string> messageProvider, string category);
		void Exception(Exception e, LogSeverity severity, string message, string category);
	}

}
