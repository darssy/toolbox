using System;

namespace MmiSoft.Core.Patterns
{
	public class DelegateCommand: ICommand
	{
		private Action action;
		
		public DelegateCommand(Action action)
		{
			this.action = action ?? throw new ArgumentNullException(nameof(action));
		}
		
		public void Execute() => action();
	}
}
