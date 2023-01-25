using System;
using System.Collections.Generic;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace MmiSoft.Core.Logging
{
	/// <summary>
	/// The caveat with this class is that the calls to <see cref="IsLevelSet"/> and <see cref="SetLevel"/>
	/// will fail if you don't set <see cref="LogManager"/>.<see cref="LogManager.Configuration"/> manually. It would
	/// be super easy to set it in the constructor but then the "automatic" discovery of the "default" nlog config goes
	/// out the window. Unfortunately the mechanism of retrieving the config path is not part of NLog's public API.
	/// </summary>
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
		
		/// <summary>
		/// Sets or gets the global log level. 
		/// <seealso cref="NLog.LogLevel"/>
		/// <seealso cref="LogManager.GlobalThreshold"/>
		/// </summary>
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

		/// <inheritdoc />
		public void Log(LogSeverity severity, Func<string> messageProvider, string category)
		{
			LogManager
				.GetLogger(category)
				.Log(levelMapper[severity], (IFormatProvider)null, new LogMessageGenerator(messageProvider));
		}

		/// <inheritdoc />
		public void Exception(Exception e, LogSeverity severity, string message, string category)
		{
			LogManager.GetLogger(category)
				.Log(levelMapper[severity], e, message);
		}

		/// <summary>
		/// Wrapper for <c>LogManager.GetLogger(logger).IsEnabled(severity)</c>
		/// <seealso cref="Logger.IsEnabled"/>
		/// <seealso cref="LogManager.GetLogger(string)"/>
		/// </summary>
		/// <param name="severity">The severity to check against</param>
		/// <param name="loggerCategory">The logger to check against</param>
		/// <returns>True if a message with that severity for a logger with that name will be logged.</returns>
		public bool IsLevelSet(LogSeverity severity, string loggerCategory)
		{
			foreach (LoggingRule rule in LogManager.Configuration.LoggingRules)
			{
				if (rule.LoggerNamePattern == loggerCategory)
				{
					return rule.IsLoggingEnabledForLevel(levelMapper[severity]);
				}
			}
			return LogManager.GetLogger(loggerCategory).IsEnabled(levelMapper[severity]);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="severity"></param>
		/// <param name="loggerCategory"></param>
		public void SetLevel(LogSeverity severity, string loggerCategory)
		{
			foreach (LoggingRule rule in LogManager.Configuration.LoggingRules)
			{
				if (rule.LoggerNamePattern == loggerCategory)
				{
					rule.EnableLoggingForLevels(levelMapper[severity], NLog.LogLevel.Fatal);
					LogManager.ReconfigExistingLoggers();
					return;
				}
			}
			foreach (Target target in LogManager.Configuration.AllTargets)
			{
				LogManager.Configuration.AddRule(levelMapper[severity], NLog.LogLevel.Fatal, target, loggerCategory);
			}
			LogManager.ReconfigExistingLoggers();
		}
	}
}
