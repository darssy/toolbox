using System;
using System.Collections.Generic;

namespace MmiSoft.Core.Logging
{
	/// <summary>
	/// A log wrapper that redirects the log calls to the Console. All methods will log to the <see cref="Console.Out"/>
	/// writer unless the severity is either <see cref="LogSeverity.Error"/> or <see cref="LogSeverity.Fatal"/>. In that
	/// case messages are written in <see cref="Console.Error"/>
	/// </summary>
	public class ConsoleLogWrapper : ILogWrapper
	{
		private Dictionary<string, LogSeverity> perLoggerLevels = new Dictionary<string, LogSeverity>();
			
		/// <inheritdoc />
		public LogSeverity LogLevel { get; set; }

		/// <summary>
		/// Logs a message with the specified log severity and category. The format is <c>[severity] [category][tab][message]</c>
		/// </summary>
		/// <param name="severity">The "severity" or level to use for logging this message. If it is higher than the
		/// global level set, the message is not logged.</param>
		/// <param name="message">The message to log</param>
		/// <param name="category">The category to log this message at. Usually this corresponds to a logger based on
		/// the type name of the calling class</param>
		public void Log(LogSeverity severity, string message, string category)
		{
			if (!IsLevelSet(severity, category)) return;

			if (severity <= LogSeverity.Error)
			{
				Console.Error.WriteLine($"{severity} {category}\t{message}");
			}
			else
			{
				Console.WriteLine($"{severity} {category}\t{message}");
			}
		}

		/// <summary>
		/// This overload is to be used when the message creation is an expensive process and might yield extra string
		/// allocations, format operations etc. If the <see cref="LogLevel"/> won't allow the message to be written
		/// then you don't want this extra performance penalty. It's effectiveness though can be dubious as the creation
		/// of the <paramref name="messageProvider"/> <i>is</i> an allocation by itself as you will need to allocate a
		/// lambda. You might be better off checking the logging level with a plain ol' <c>if</c>.
		/// </summary>
		/// <param name="severity">The "severity" or level to use for logging this message. If it is higher than the
		/// global level set, the message should not be logged.</param>
		/// <param name="messageProvider">A function that will provide the message to log</param>
		/// <param name="category">The category to log this message at. Usually this corresponds to a logger based on
		/// the type name of the calling class</param>
		/// <exception cref="ArgumentNullException">If <paramref name="messageProvider"/> is null</exception>
		public void Log(LogSeverity severity, Func<string> messageProvider, string category)
		{
			if (!IsLevelSet(severity, category)) return;
			if (messageProvider == null) throw new ArgumentNullException(nameof(messageProvider));

			if (severity <= LogSeverity.Error)
			{
				Console.Error.WriteLine($"{severity} {category}\t{messageProvider.Invoke()}");
			}
			else
			{
				Console.WriteLine($"{severity} {category}\t{messageProvider.Invoke()}");
			}
		}

		/// <inheritdoc />
		public void Exception(Exception e, LogSeverity severity, string message, string category)
		{
			if (!IsLevelSet(severity, category)) return;

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

		/// <summary>
		/// Checks if a logging level is set for that logger.
		/// </summary>
		/// <param name="severity"></param>
		/// <param name="loggerCategory"></param>
		/// <returns></returns>
		public bool IsLevelSet(LogSeverity severity, string loggerCategory)
		{
			if (string.IsNullOrWhiteSpace(loggerCategory)
			    || !perLoggerLevels.TryGetValue(loggerCategory, out LogSeverity existing))
			{
				return severity < LogLevel;
			}
			return existing >= severity;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="severity"></param>
		/// <param name="loggerCategory"></param>
		public void SetLevel(LogSeverity severity, string loggerCategory) => perLoggerLevels[loggerCategory] = severity;
	}
}
