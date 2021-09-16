using System;
using System.Collections.Generic;

namespace MmiSoft.Core.Patterns
{
	public class ChainOfResponsibility<T>: ICommand<T>
	{
		private List<Func<T, bool>> nodes = new List<Func<T, bool>>();
		
		public bool Execute(T request)
		{
			foreach (Func<T, bool> func in nodes)
			{
				if (func(request)) return true;
			}
			return false;
		}

		public void AddNode(Func<T, bool> node)
		{
			nodes.Add(node ?? throw new ArgumentNullException(nameof(node)));
		}

		public void RemoveNode(Func<T, bool> node)
		{
			nodes.Remove(node ?? throw new ArgumentNullException(nameof(node)));
		}
	}
}
