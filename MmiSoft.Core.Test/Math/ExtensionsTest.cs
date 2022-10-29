using NUnit.Framework;

namespace MmiSoft.Core.Math
{
	[TestFixture]
	public class ExtensionsTest
	{
		[Test]
		public void AlmostEqual_ZeroMustBeAlmostEqualToZero()
		{
			Assert.That(0.0.AlmostEqual(0.0), Is.True);
		}
	}
}
