using NUnit.Framework;

namespace MmiSoft.Core
{
	[TestFixture]
	public class ValueRangeTest
	{
		[Test]
		public void Intersects_NonOverlappingRanges_ReturnsFalse()
		{
			Assert.False(new ValueRange<int>(5, 10).Intersects(new ValueRange<int>(11, 13)));
		}

		[Test]
		public void Intersects_EngulfedRange_ReturnsTrue()
		{
			Assert.True(new ValueRange<int>(5, 10).Intersects(new ValueRange<int>(6, 7)));
		}

		[Test]
		public void Intersects_OuterRangeRange_ReturnsFalse()
		{
			Assert.False(new ValueRange<int>(6, 7).Intersects(new ValueRange<int>(5, 10)));
		}

		[Test]
		public void Intersects_TouchingRange_ReturnsTrue()
		{
			Assert.True(new ValueRange<int>(1, 5).Intersects(new ValueRange<int>(5, 10)));
			Assert.True(new ValueRange<int>(1, 5).Intersects(new ValueRange<int>(-5, 1)));
		}

		[Test]
		public void Intersects_NullRange_ReturnsFalse()
		{
			Assert.False(new ValueRange<int>(1, 5).Intersects(null));
		}

		[Test]
		public void Intersects_SameReferenceRange_ReturnsTrue()
		{
			ValueRange<int> range = new ValueRange<int>(1, 5);
			Assert.True(range.Intersects(range));
		}

		[Test]
		public void Constructor_TransposedValues_CreatesObjectWithMinLtMax()
		{
			ValueRange<int> range = new ValueRange<int>(13, 2);
			Assert.AreEqual(2, range.MinValue);
			Assert.AreEqual(13, range.MaxValue);
		}
	}
}
