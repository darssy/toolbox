using System;

namespace MmiSoft.Core.Logging
{
	/// <summary>
	/// Interface to wrap a logger implementation and use in <see cref="EventLogger.LoggerImplementation"/>.
	/// Currently this wrapper supports only setting the global <see cref="LogLevel"/> of the underlying logger.
	/// </summary>
	public interface ILogWrapper
	{
		/// <summary>
		/// Sets or gets the global log level. 
		/// </summary>
		LogSeverity LogLevel { get; set; }
		
		/// <summary>
		/// Logs a message with the specified log severity. It's the implementor's responsibility to check if the log
		/// level is set before logging.
		/// </summary>
		/// <param name="severity">The "severity" or level to use for logging this message. If it is higher than the
		/// global level set, the message should not be logged.</param>
		/// <param name="message">The message to log</param>
		/// <param name="category">The category to log this message at. Usually this corresponds to a logger based on
		/// the type name of the calling class</param>
		void Log(LogSeverity severity, string message, string category);
		
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
		void Log(LogSeverity severity, Func<string> messageProvider, string category);
		
		/// <summary>
		/// Convenience method for logging an exception's StackTrace and message.
		/// </summary>
		/// <param name="e">The exception to log</param>
		/// <param name="severity">The severity of the exception. Usually that is <see cref="LogSeverity.Error"/>
		/// or <see cref="LogSeverity.Fatal"/> but it's up to the caller to decide.
		/// </param>
		/// <param name="message">A supplementary message apart from the exception's message</param>
		/// <param name="category">The category to log this message at. Usually this corresponds to a logger based on
		/// the type name of the calling class</param>
		void Exception(Exception e, LogSeverity severity, string message, string category);
	}

}
