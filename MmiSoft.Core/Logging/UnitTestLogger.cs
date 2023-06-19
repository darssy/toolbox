using System;
using System.Collections.Generic;
using System.Linq;

namespace MmiSoft.Core.Logging
{
	/// <summary>
	/// This class is not yet fully implemented. It can be used in case you want to include logging assertions in your unit tests.
	/// </summary>
	public class UnitTestLogger : ILogWrapper
	{
		private Dictionary<LogSeverity, List<string>> loggedEntries = new();
		private Dictionary<LogSeverity, List<Exception>> exceptions = new();

		public UnitTestLogger()
		{
			foreach (LogSeverity severity in Enums.GetValues<LogSeverity>())
			{
				loggedEntries[severity] = new List<string>();
				exceptions[severity] = new List<Exception>();
			}
		}

		public IReadOnlyList<string> this[LogSeverity severity] => loggedEntries[severity];
		
		public IReadOnlyList<string> GetEntries(LogSeverity severity) => loggedEntries[severity];

		/// <summary>
		/// Returns true if there is nothing at all logged, regardless of severity.
		/// </summary>
		public bool Empty => loggedEntries.Values.All(e => e.Count == 0);

		public void Log(LogSeverity severity, string message, string category)
		{
			loggedEntries[severity].Add(message);
		}

		public void Log(LogSeverity severity, Func<string> messageProvider, string category)
		{
			loggedEntries[severity].Add(messageProvider.Invoke());
		}

		public void Exception(Exception e, LogSeverity severity, string message, string category)
		{
			loggedEntries[severity].Add(message);
			exceptions[severity].Add(e);
		}

		public bool IsLevelSet(LogSeverity severity, string loggerCategory) => LogLevel == severity;

		public void SetLevel(LogSeverity severity, string loggerCategory) =>
			loggedEntries[LogSeverity.Warning].Add($"Per category level is not supported for {nameof(UnitTestLogger)}");

		public LogSeverity LogLevel { get; set; }
	}
}
