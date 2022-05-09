using NUnit.Framework;

namespace MmiSoft.Core.Patterns
{
	[TestFixture]
	public class ChainOfResponsibilityTest
	{
		[Test]
		public void ChainCanBeBroken()
		{
			int endResult = 0;
			ChainOfResponsibility<string> chain = new ChainOfResponsibility<string>();

			Assert.IsFalse(chain.Execute("data"));

			bool @break = true;

			chain.AddNode(s =>
			{
				endResult = 1;
				return false;
			});
			chain.AddNode(s =>
			{
				endResult = 2;
				return @break;
			});
			chain.AddNode(s =>
			{
				endResult = 3;
				return false;
			});

			chain.Execute("foo");
			Assert.AreEqual(2, endResult);

			@break = false;
			chain.Execute("bar");
			Assert.AreEqual(3, endResult);
		}

		[Test]
		public void NodeCanBeRemoved()
		{
			ChainOfResponsibility<string> chain = new ChainOfResponsibility<string>();

			bool Node(string s) => true;
			chain.AddNode(Node);
			Assert.IsTrue(chain.Execute("data"));
			
			chain.RemoveNode(Node);
			Assert.IsFalse(chain.Execute("data"));
		}

	}
}