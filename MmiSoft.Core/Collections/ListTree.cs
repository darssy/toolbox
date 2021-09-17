using System;
using System.Collections.Generic;
using System.Linq;

namespace MmiSoft.Core.Collections
{
	public class ListTree<T>
	{
		private T data;
		private LinkedList<ListTree<T>> children;

		public T Data => data;

		public bool IsLeaf => children.Count == 0;

		public ListTree(T data)
		{
			this.data = data;
			children = new LinkedList<ListTree<T>>();
		}

		public int ChildrenCount => children.Count;

		public ListTree<T> Last => children.Last.Value;

		public ListTree<T> AddChild(T child)
		{
			return children.AddLast(new ListTree<T>(child)).Value;
		}

		public bool RemoveChildNode(ListTree<T> child)
		{
			return children.Remove(child);
		}

		public ListTree<T> GetChild(Func<T, bool> condition)
		{
			return children.SingleOrDefault(tree => condition(tree.data));
		}

		public ListTree<T> GetChildNodeAt(int i)
		{
			return children.FirstOrDefault(n => --i == 0);
		}

		public bool ContainsChild(Func<T, bool> condition)
		{
			return children.Any(tree => condition(tree.data));
		}

		public void Traverse(Func<T, bool> action)
		{
			Traverse(this, action);
		}

		private static void Traverse(ListTree<T> node, Func<T, bool> action)
		{
			if (action(node.data)) return;
			foreach (ListTree<T> child in node.children)
			{
				Traverse(child, action);
			}
		}

		public override string ToString()
		{
			return $"NTree Node {data} {children.Count} child(ren)";
		}
	}
}
