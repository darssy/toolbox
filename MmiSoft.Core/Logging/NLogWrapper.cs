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
		private static LogLevel[] levelMapper =
		{
			// level ordinals in NLog are reversed compared to LogSeverity
			NLog.LogLevel.Fatal,
			NLog.LogLevel.Error,
			NLog.LogLevel.Warn,
			NLog.LogLevel.Info,
			NLog.LogLevel.Debug,
			NLog.LogLevel.Trace,
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
				LogManager.GlobalThreshold = levelMapper[(int)value];
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
			LogManager.GetLogger(category).Log(levelMapper[(int)severity], new LogMessageGenerator(messageProvider));
		}

		/// <inheritdoc />
		public void Exception(Exception e, LogSeverity severity, string message, string category)
		{
			LogManager.GetLogger(category).Log(levelMapper[(int)severity], e, message);
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
					return rule.IsLoggingEnabledForLevel(levelMapper[(int)severity]);
				}
			}
			return LogManager.GetLogger(loggerCategory).IsEnabled(levelMapper[(int)severity]);
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
					rule.EnableLoggingForLevels(levelMapper[(int)severity], NLog.LogLevel.Fatal);
					LogManager.ReconfigExistingLoggers();
					return;
				}
			}
			foreach (Target target in LogManager.Configuration.AllTargets)
			{
				LogManager.Configuration.AddRule(levelMapper[(int)severity], NLog.LogLevel.Fatal, target, loggerCategory);
			}
			LogManager.ReconfigExistingLoggers();
		}
	}
}
