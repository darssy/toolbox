using System;

namespace MmiSoft.Core.Patterns
{
	public class ExceptionHandlingCommandWrapper<TExc> : ICommand where TExc : Exception
	{
		private ICommand decorated;

		public ExceptionHandlingCommandWrapper(ICommand decorated)
		{
			this.decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
		}

		public void Execute()
		{
			try
			{
				decorated.Execute();
			}
			catch (TExc e)
			{
				e.Log("Operation failed");
			}
		}
	}
}
