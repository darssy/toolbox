using System;
using MmiSoft.Core.Logging;

namespace MmiSoft.Core
{
	/**
	 * <para>
	 * A bit of history behind this class: many years ago it was my primary logging class. Quite primitive but got the job
	 * done. At some point and when the application that I was using it in became more serious, I converted it as a wrapper
	 * for NLog. The decision to make this class static (btw Mark Seemann calls that "Ambient Context" in his book) made
	 * the whole transition so smooth that I dare to call it a B E A U T Y.
	 * </para>
	 *
	 * <para>
	 * Fast forward to today, where this class is part of an open source library, hard coding NLog in this static wrapper
	 * might not be very handy as it forces you to get onboard a logging library you might not want to use. In the first
	 * phase of refactorings this class will be converted to a shell for an NLog implementation. In the second and final
	 * one, it will be a wrapper for a simple console logging mechanism and NLog will be removed from the dependency list.
	 * </para>
	 *
	 * <para>
	 * The benefit of using this class will be that you could access it from everywhere without wondering "how". Just as
	 * you do <c>Environment.NewLine</c> you could do <c>EventLogger.Log()</c>. Then if for any reason you want to change
	 * logging library, you could do it in one place and then set the <c>LoggerImplementation</c> property. Alternatively,
	 * if you are hard core into constructor injection, you could pass <c>ILogWrapper</c> to <i>every</i> class that needs
	 * to do logging -which IMHO is an overkill.
	 * </para>
	 */
	public static class EventLogger
	{
		private static ILogWrapper loggerImplementation = new NLogWrapper();
		// private static ILogWrapper loggerImplementation = new ConsoleLogWrapper();

		public static ILogWrapper LoggerImplementation
		{
			get => loggerImplementation;
			set => loggerImplementation = value;
		}

		[Obsolete("For backwards compatibility. Use Info(message) instead")]
		public static void WriteEntry(string message) => Info(message);

		[Obsolete("For backwards compatibility.")]
		public static void WriteEntry(string message, Type type, LogSeverity severity)
		{
			loggerImplementation.Log(severity, message, type.FullName);
		}

		[Obsolete("For backwards compatibility.")]
		public static void WriteEntry(LogEntry entry)
		{
			if (entry.IsSevere)
			{
				loggerImplementation.Exception(entry.Exception, entry.Severity, entry.Message, entry.Module);
			}
			else loggerImplementation.Log(entry.Severity, entry.Message, entry.Module);
		}

		public static void Fatal(string message, string category)
		{
			loggerImplementation.Log(LogSeverity.Fatal, message, category);
		}

		public static void Info(string message, string category = "")
		{
			loggerImplementation.Log(LogSeverity.Info, message, category);
		}

		public static void Info(string message, Type callerType)
		{
			loggerImplementation.Log(LogSeverity.Info, message, callerType?.FullName ?? "General");
		}

		public static void Warn(string message, string category = "")
		{
			loggerImplementation.Log(LogSeverity.Warning, message, category);
		}

		public static void Warn(string message, Type callerType)
		{
			loggerImplementation.Log(LogSeverity.Warning, message, callerType?.FullName ?? "General");
		}

		public static void Error(string message, string category = "")
		{
			loggerImplementation.Log(LogSeverity.Error, message, category);
		}

		public static void Debug(string message, string category = "")
		{
			loggerImplementation.Log(LogSeverity.Debug, message, category);
		}

		public static void Debug(string message, Type callerType)
		{
			loggerImplementation.Log(LogSeverity.Debug, message, callerType?.FullName ?? "General");
		}

		public static void Trace(string message, string category = "")
		{
			loggerImplementation.Log(LogSeverity.Trace, message, category);
		}

		public static void Fatal(Func<string> messageProvider, string category = "")
		{
			loggerImplementation.Log(LogSeverity.Fatal, messageProvider, category);
		}

		public static void Info(Func<string> messageProvider, string category = "")
		{
			loggerImplementation.Log(LogSeverity.Info, messageProvider, category);
		}

		public static void Warn(Func<string> messageProvider, string category = "")
		{
			loggerImplementation.Log(LogSeverity.Warning, messageProvider, category);
		}

		public static void Error(Func<string> messageProvider, string category = "")
		{
			loggerImplementation.Log(LogSeverity.Error, messageProvider, category);
		}

		public static void Debug(Func<string> messageProvider, string category = "")
		{
			loggerImplementation.Log(LogSeverity.Debug, messageProvider, category);
		}

		public static void Trace(Func<string> messageProvider, string category = "")
		{
			loggerImplementation.Log(LogSeverity.Trace, messageProvider, category);
		}

		public static void Log(this Exception exc, string userMessage, string module="N/A")
		{
			loggerImplementation.Exception(exc, LogSeverity.Error, userMessage, module);
		}
	}
}
