using System;

namespace MmiSoft.Core
{
	public class LogEntry
	{
		private DateTime writeTime;
		private LogSeverity severity;
		private string module;
		private string message;
		private Exception exception;

		private LogEntry(string message, LogSeverity severity, string module, Exception exception)
		{
			writeTime = DateTime.Now;
			this.severity = severity;
			this.module = module;
			this.message = exception == null ? message : $"{message} {exception.Message}";
			this.exception = exception;
		}

		public LogEntry(string message, Exception cause, string module = "N/A")
			: this(message, LogSeverity.Error, module, cause)
		{
		}

		public LogEntry(string message, string module = "", LogSeverity severity = LogSeverity.Info)
			: this(message, severity, module, null)
		{
		}

		public DateTime WriteTime => writeTime;

		public LogSeverity Severity => severity;

		public string Module => module;

		public string Message => message;

		public Exception Exception => exception;

		/// <summary>
		/// Convenience property for Exception != null.
		/// </summary>
		public bool IsSevere => exception != null;

		public override string ToString()
		{
			return $"Log Entry: '{writeTime}\t{severity}\t{message}'";
		}
	}
}
