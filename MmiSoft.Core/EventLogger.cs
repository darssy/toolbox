using System;
using NLog;

namespace MmiSoft.Core
{

	public static class EventLogger
	{
		private static Logger nLogger = LogManager.GetCurrentClassLogger();

		public static void DebugIfEnabled(Func<string> messageProducer, Type callerType)
		{
			Logger logger = LogManager.GetLogger(callerType.FullName);

			if (logger.IsDebugEnabled)
			{
				logger.Debug(messageProducer.Invoke());
			}
		}

		public static void Warn(string message, Type callerType = null)
		{
			WriteEntry(message, callerType, LogSeverity.Warning);
		}

		public static void WriteEntry(string message, Type callerType = null, LogSeverity severity = LogSeverity.Info)
		{
			Logger logger = callerType == null ? nLogger : LogManager.GetLogger(callerType.FullName);
			WriteEntry(message, logger, severity);
		}

		public static void WriteEntry(string message, string module, LogSeverity severity = LogSeverity.Info)
		{
			Logger logger = module == null ? nLogger : LogManager.GetLogger(module);
			WriteEntry(message, logger, severity);
		}

		private static void WriteEntry(string message, Logger logger, LogSeverity severity)
		{
			switch (severity)
			{
				case LogSeverity.Error:
					logger.Error(message);
					break;
				case LogSeverity.Warning:
					logger.Warn(message);
					break;
				case LogSeverity.Info:
					logger.Info(message);
					break;
				case LogSeverity.Debug:
					logger.Debug(message);
					break;
				case LogSeverity.Trace:
					logger.Trace(message);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
			}
		}

		public static void WriteEntry(LogEntry entry)
		{
			if (entry.IsSevere)
			{
				nLogger.Error(entry.Exception, entry.Message);
			}
			else WriteEntry(entry.Message, severity:entry.Severity);
		}

		public static void TraceIfSet(Func<string> messageProducer, Type callerType)
		{
			Logger logger = LogManager.GetLogger(callerType.FullName);

			if (logger.IsTraceEnabled)
			{
				logger.Debug(messageProducer.Invoke());
			}
		}

		public static void LogException(Exception exc, string userMessage)
		{
			nLogger.Error(exc, userMessage);
		}

		public static void Log(this Exception exc, string userMessage, string module="N/A")
		{
			LogManager.GetLogger(module).Error(exc, userMessage);
		}
	}
}
