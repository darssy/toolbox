using System;

namespace MmiSoft.Core.Patterns
{
	public class LoggingCommandWrapper : ICommand
	{
		private ICommand decorated;

		public LoggingCommandWrapper(ICommand decorated)
		{
			this.decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
		}

		public void Execute()
		{
			try
			{
				decorated.Execute();
			}
			catch (Exception e)
			{
				e.Log($"Operation '{decorated}' failed");
			}
		}
	}

}
