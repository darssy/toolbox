using NUnit.Framework;

namespace MmiSoft.Core
{
	[TestFixture]
	public class ArithmeticExtensionsTest
	{
		[Test]
		public void IsWithin_RandomCase_7IsWithin4To9Range()
		{
			Assert.That(7.IsWithin(4, 9), Is.True);
		}

		[Test]
		public void IsWithin_TransposedMinAndMax7IsNotWithin8To3Range()
		{
			Assert.That(7.IsWithin(8, 3), Is.False);
		}

		[Test]
		public void IsWithin_MixedNumberTypes()
		{
			Assert.That((-10.6).IsWithin(-20, 3L), Is.True);
			Assert.That((-10.6f).IsWithin(-20, 3L), Is.True);
			byte b = 64;
			Assert.That(b.IsWithin(0, 128), Is.True);
			Assert.That(4238408204823.IsWithin(0, 128), Is.False);
			Assert.That(3m.IsWithin(0, 128), Is.True);
		}

		[Test]
		public void IsWithin_NumbersOnTheRangeEdges_ReturnTrue()
		{
			Assert.That((-10.6).IsWithin(-10.6, 0), Is.True);
			Assert.That(19.9.IsWithin(0, 19.9), Is.True);
			Assert.That(19.9.IsWithin(19.9, 20), Is.True);
		}
	}
}
