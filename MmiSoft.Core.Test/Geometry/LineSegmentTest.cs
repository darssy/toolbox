using System.Drawing;
using NUnit.Framework;

namespace MmiSoft.Core.Geometry
{
	[TestFixture]
	public class LineSegmentTest
	{
		[Test]
		public void IsNear_ToleranceIs0_PointIsOnLine_ReturnsTrue()
		{
			var segment = new LineSegment(new Point(5, 5), new Point(10, 10));
			Assert.IsTrue(segment.IsNear(new Point(7, 7), 0));
		}
		
		[Test]
		public void IsNear_ToleranceIs0_PointIsOnLineExtension_ReturnsFalse()
		{
			var segment = new LineSegment(new Point(5, 5), new Point(10, 10));
			Assert.IsFalse(segment.IsNear(new Point(4, 4), 0));
		}
		
		[Test]
		public void IsNear_ToleranceIs3_PointIsNextToLine_ReturnsTrue()
		{
			var segment = new LineSegment(new Point(5, 5), new Point(10, 10));
			Assert.IsTrue(segment.IsNear(new Point(6, 7), 3));
		}
		
		[Test]
		public void IsNear_ToleranceIs2_PointIsNextToLine_ReturnsFalse()
		{
			var segment = new LineSegment(new Point(5, 5), new Point(10, 10));
			Assert.IsFalse(segment.IsNear(new Point(5, 8), 2));
		}
		
		[Test]
		public void IsNear_ToleranceIs2_PointIsOnLineExtension_ReturnsFalse()
		{
			var segment = new LineSegment(new Point(5, 5), new Point(10, 10));
			Assert.IsFalse(segment.IsNear(new Point(12, 12), 2));
		}
		
		[Test]
		public void IsNear_ToleranceIs2_PointIsOnLineExtension_ReturnsTrue()
		{
			var segment = new LineSegment(new Point(5, 5), new Point(10, 10));
			Assert.IsTrue(segment.IsNear(new Point(11, 11), 2));
		}
		
		[Test]
		public void Equals_DifferentInstancesSameValues_ReturnsTrue()
		{
			var s1 = new LineSegment(new Point(5, 5), new Point(10, 10));
			var s2 = new LineSegment(new Point(5, 5), new Point(10, 10));
			Assert.AreEqual(s1, s2);
			
		}
		
		[Test]
		public void Equals_DifferentInstancesTransposedValues_ReturnsTrue()
		{
			var s1 = new LineSegment(new Point(3, 9), new Point(10, 10));
			var s2 = new LineSegment(new Point(10, 10), new Point(3, 9));
			Assert.AreEqual(s1, s2);
			Assert.True(s1 == s2);
			Assert.AreEqual(s1.GetHashCode(), s2.GetHashCode());
		}

		[Test]
		public void HashCode_DifferentInstancesTransposedPointXYValues_HashesMustNotBeEqual()
		{
			var s1 = new LineSegment(new Point(3, 9), new Point(5, 4));
			var s2 = new LineSegment(new Point(9, 3), new Point(4, 5));
			Assert.True(s1 != s2);
			Assert.AreNotEqual(s1.GetHashCode(), s2.GetHashCode());
		}

		[Test]
		public void HashCode_DifferentInstancesTransposedPointXYValuesExtreme_HashesMustNotBeEqual()
		{
			var s1 = new LineSegment(new Point(145986, -98479), new Point(56603, 200186));
			var s2 = new LineSegment(new Point(-98479, 145986), new Point(200186, 56603));
			Assert.AreNotEqual(s1.GetHashCode(), s2.GetHashCode());
		}
	}
}
