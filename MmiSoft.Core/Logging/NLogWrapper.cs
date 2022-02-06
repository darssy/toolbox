using System;
using System.Collections.Generic;
using NLog;

namespace MmiSoft.Core.Logging
{
	public class NLogWrapper : ILogWrapper
	{
		private static Dictionary<LogSeverity, LogLevel> levelMapper = new Dictionary<LogSeverity, LogLevel>
		{
			[LogSeverity.Error] = NLog.LogLevel.Error,
			[LogSeverity.Warning] = NLog.LogLevel.Warn,
			[LogSeverity.Info] = NLog.LogLevel.Info,
			[LogSeverity.Debug] = NLog.LogLevel.Debug,
			[LogSeverity.Trace] = NLog.LogLevel.Trace,
			[LogSeverity.Fatal] = NLog.LogLevel.Fatal
		};

		public NLogWrapper()
		{
			LogLevel = LogSeverity.Info;
		}

		private LogSeverity localLogLevel;
		public LogSeverity LogLevel
		{
			get => localLogLevel;
			set
			{
				LogManager.GlobalThreshold = levelMapper[value];
				localLogLevel = value;
			}
		}

		public void Log(LogSeverity severity, string message, string category)
		{
			Logger logger = LogManager.GetLogger(category);
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
				case LogSeverity.Fatal:
					logger.Fatal(message);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
			}
		}

		public void Log(LogSeverity severity, Func<string> messageProvider, string category)
		{
			Logger logger = LogManager.GetLogger(category);
			switch (severity)
			{
				case LogSeverity.Error:
					logger.Error(messageProvider);
					break;
				case LogSeverity.Warning:
					logger.Warn(messageProvider);
					break;
				case LogSeverity.Info:
					logger.Info(messageProvider);
					break;
				case LogSeverity.Debug:
					logger.Debug(messageProvider);
					break;
				case LogSeverity.Trace:
					logger.Trace(messageProvider);
					break;
				case LogSeverity.Fatal:
					logger.Fatal(messageProvider);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
			}
		}

		public void Exception(Exception e, LogSeverity severity, string message, string category)
		{
			LogManager.GetLogger(category)
				.Log(levelMapper[severity], e, message);
		}
	}
}
