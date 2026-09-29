using NUnit.Framework;

namespace MmiSoft.Core.Math
{
	[TestFixture]
	public class MathExtensionsTest
	{
		[Test]
		public void AlmostEqual_ZeroMustBeAlmostEqualToZero()
		{
			Assert.That(0.0.AlmostEqual(0.0), Is.True);
		}
	}
}
