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

		public LogEntry(string message,
			LogSeverity severity = LogSeverity.Info,
			string module = "N/A",
			Exception exception = null)
		{
			writeTime = DateTime.Now;
			this.severity = severity;
			this.module = module;
			this.message = message;
			this.exception = exception;
		}

		public LogEntry(string message, LogSeverity severity)
			: this(message, severity, "N/A")
		{ }

		public LogEntry(string message, Exception cause)
			: this(message, cause, "N/A")
		{ }

		public LogEntry(string message, Exception cause, string module)
			: this($"{message} {cause.Message}", LogSeverity.Error, module)
		{
			exception = cause;
		}

		public LogEntry(string message, LogSeverity severity, string module)
		{
			writeTime = DateTime.Now;
			this.message = message;
			this.severity = severity;
			this.module = module;
		}

		public DateTime WriteTime => writeTime;

		public LogSeverity Severity => severity;

		public string Module => module;

		public string Message => message;

		public Exception Exception => exception;

		/// <summary>
		/// Conveniece property for Exception != null.
		/// </summary>
		public bool IsSevere => exception != null;

		public override string ToString()
		{
			return writeTime + "\t" + severity + "\t" + message;
		}

		public static LogEntry Parse(String text)
		{
			String[] elements = text.Split('\t');
			DateTime date = DateTime.Parse(elements[0]);
			LogSeverity severity = (LogSeverity)Enum.Parse(typeof(LogSeverity), elements[1], true);
			LogEntry entry = new LogEntry(elements[2], severity);
			entry.writeTime = date;
			return entry;
		}
	}
}
