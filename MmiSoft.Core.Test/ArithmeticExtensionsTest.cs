using System;
using NUnit.Framework;

namespace MmiSoft.Core
{
	[TestFixture]
	public class ArithmeticExtensionsTest
	{
		// Issue A (part 1): zero interior points is a valid request -the result is an empty array.
		[Test]
		public void FillSeries_ZeroSeries_ReturnsEmptyArray()
		{
			Assert.That(0f.FillSeries(10f, 0), Is.Empty);
		}

		[Test]
		public void FillSeries_ReturnsInteriorDivisionPoints()
		{
			Assert.That(0f.FillSeries(10f, 3), Is.EqualTo(new[] { 2.5f, 5f, 7.5f }));
		}

		[Test]
		public void FillSeries_NegativeSeries_Throws()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => 0f.FillSeries(10f, -1));
		}

		// Issue A (part 2): a percentage within a zero-width range is undefined, so it fails fast instead
		// of silently producing a non-finite Percent.
		[Test]
		public void PercentBetween_FromEqualsTo_Throws()
		{
			Assert.Throws<ArgumentException>(() => 5.PercentBetween(3, 3));
		}

		[Test]
		public void PercentBetween_ValueInRange_ReturnsPosition()
		{
			Assert.That(5.PercentBetween(0, 10), Is.EqualTo(new Percent(50)));
		}

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
