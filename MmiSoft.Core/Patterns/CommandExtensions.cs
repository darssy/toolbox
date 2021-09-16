using System;
using System.Threading;
using System.Threading.Tasks;

namespace MmiSoft.Core.Patterns
{
	public static class CommandExtensions
	{
		public static ICommand MeasureExecutionTime(this ICommand toBeDecorated, TimeSpan threshold,
			Action<string> logConsumer = null, string userText = null)
		{
			return new ExecutionTimeCommand(toBeDecorated, threshold, logConsumer, userText);
		}
		
		public static ICommand AsCommand(this Action toBeWrapped)
		{
			return new DelegateCommand(toBeWrapped);
		}
		
		public static ICommand WithLogging(this ICommand toBeDecorated)
		{
			return new LoggingCommandWrapper(toBeDecorated);
		}

		public static ICommand Try<TExc> (this ICommand toBeDecorated) where TExc : Exception
		{
			return new ExceptionHandlingCommandWrapper<TExc>(toBeDecorated);
		}

		public static void ExecuteInContext(this ICommand cmd, SynchronizationContext context)
		{
			context.Post(state => cmd.Execute(), null);
		}
	}

}
