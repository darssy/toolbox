using NUnit.Framework;

namespace MmiSoft.Core.Collections
{
	[TestFixture]
	public class ListTreeTest
	{
		[Test]
		public void GetChildNodeAt_IsZeroBased()
		{
			ListTree<int> tree = new(0);
			for (int i = 0; i < 5; i++)
			{
				tree.AddChild(i);
			}
			for (int i = 0; i < tree.ChildrenCount; i++)
			{
				Assert.That(tree.GetChildNodeAt(i).Data, Is.EqualTo(i));
			}
		}
		
		[Test]
		public void ListTreeEnumerator_ReturnsOnlyChildren()
		{
			ListTree<int> tree = new(0);
			for (int i = 0; i < 5; i++)
			{
				tree.AddChild(i);
			}

			int ctr = 0;
			foreach (ListTree<int> listTree in tree)
			{
				Assert.That(listTree, Is.EqualTo(tree.GetChildNodeAt(ctr++)));
			}
		}
	}
}
