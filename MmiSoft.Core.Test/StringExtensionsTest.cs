using System.Linq;
using NUnit.Framework;

namespace MmiSoft.Core
{
	[TestFixture]
	public class StringExtensionsTest
	{
		[Test]
		public void JoinWithSpace()
		{
			string result = new[] {'a', 'b', 'c'}.Join();
			Assert.That(result, Is.EqualTo("a b c"));
		}

		[Test]
		public void JoinOnEmptyCharArray_ReturnsEmptyString()
		{
			string result = new char[] {}.Join();
			Assert.That(result, Is.EqualTo(""));
		}

		[Test]
		public void JoinOnNonArray()
		{
			string result = "This Is an UpperCase Test".Where(char.IsUpper).Join(',');
			Assert.That(result, Is.EqualTo("T,I,U,C,T"));
		}
	}
}
