using System;

namespace MmiSoft.Core.Logging
{
	public static class LogWrapperExtensions
	{

		public static void Fatal(this ILogWrapper logger, string message, string category)
		{
			logger.Log(LogSeverity.Fatal, message, category);
		}

		public static void Info(this ILogWrapper logger, string message, string category)
		{
			logger.Log(LogSeverity.Info, message, category);
		}

		public static void Warn(this ILogWrapper logger, string message, string category)
		{
			logger.Log(LogSeverity.Warning, message, category);
		}

		public static void Error(this ILogWrapper logger, string message, string category)
		{
			logger.Log(LogSeverity.Error, message, category);
		}

		public static void Debug(this ILogWrapper logger, string message, string category)
		{
			logger.Log(LogSeverity.Debug, message, category);
		}

		public static void Trace(this ILogWrapper logger, string message, string category)
		{
			logger.Log(LogSeverity.Trace, message, category);
		}

		public static void Fatal(this ILogWrapper logger, Func<string> messageProvider, string category)
		{
			logger.Log(LogSeverity.Fatal, messageProvider, category);
		}

		public static void Info(this ILogWrapper logger, Func<string> messageProvider, string category)
		{
			logger.Log(LogSeverity.Info, messageProvider, category);
		}

		public static void Warn(this ILogWrapper logger, Func<string> messageProvider, string category)
		{
			logger.Log(LogSeverity.Warning, messageProvider, category);
		}

		public static void Error(this ILogWrapper logger, Func<string> messageProvider, string category)
		{
			logger.Log(LogSeverity.Error, messageProvider, category);
		}

		public static void Debug(this ILogWrapper logger, Func<string> messageProvider, string category)
		{
			logger.Log(LogSeverity.Debug, messageProvider, category);
		}

		public static void Trace(this ILogWrapper logger, Func<string> messageProvider, string category)
		{
			logger.Log(LogSeverity.Trace, messageProvider, category);
		}
	}


}
