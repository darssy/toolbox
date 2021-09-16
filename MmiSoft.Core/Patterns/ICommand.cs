using System;

namespace MmiSoft.Core.Patterns
{
	public interface ICommand
	{
		void Execute();
	}

	public interface ICommand<in T>
	{
		bool Execute(T p);
	}

	public interface ICommand<in T, out S>
	{
		S Execute(T p);
	}
}
