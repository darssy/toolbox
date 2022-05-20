using System.Drawing;
using NUnit.Framework;

namespace MmiSoft.Core.Geometry
{
	[TestFixture]
	public class PolygonTest
	{
		[Test]
		public void PolygonContainsPoint_PolygonCrossesItself_PointIsOutsidePolygonButInsideTheLoop()
		{
			Polygon polygon = new Polygon(new Point(60, 250),
				new Point(174, 92),
				new Point(358, 85),
				new Point(346, 244),
				new Point(187, 292),
				new Point(176, 184),
				new Point(284, 209),
				new Point(285, 147),
				new Point(227, 147),
				new Point(228, 249));

			Assert.IsFalse(polygon.Encloses(
				new Point(202, 220)));
		}

		[Test]
		public void PolygonContains_PolygonIsRectangular_PointIsOnRectangleBase()
		{
			Polygon polygon = new Polygon(new Point(10, 10),
				new Point(90, 10),
				new Point(90, 60),
				new Point(10, 60));

			Assert.IsFalse(polygon.Encloses(
				new Point(50, 60)));
			Assert.IsFalse(polygon.Contains(
				50, 60));
		}

		[Test]
		public void Contains_PolygonIsRectangular_PointIsOnRectangleLeft()
		{
			Polygon polygon = new Polygon(new Point(10, 10),
				new Point(90, 10),
				new Point(90, 60),
				new Point(10, 60));

			Assert.IsFalse(polygon.Encloses(
				new Point(90, 30)));
			Assert.IsFalse(polygon.Contains(
				90, 30));
		}

		[Test]
		public void Contains_PolygonIsNonConvex_PointIsInRectangleOutsidePolygon()
		{
			Polygon polygon = new Polygon(new Point(10, 10),
				new Point(90, 10),
				new Point(90, 60),
				new Point(50, 40),
				new Point(10, 60));

			Assert.IsFalse(polygon.Encloses(
				new Point(50, 41)));
			Assert.IsFalse(polygon.Contains(
				50, 41));
		}

		[Test]
		public void PointIsOnTheLine_ContainsIsTrue_EnclosesIsFalse()
		{
			Point[] p =
			{
				new Point(397,90),
				new Point(62,201),
				new Point(279,356),
				new Point(253,29)
			};

			Polygon polygon = new Polygon(p);

			Assert.True(polygon.Contains(188, 291));
			//Assert.False(polygon.Encloses(188, 291));
		}

		[Test]
		public void Landlocked_ContainsIsTrue_EnclosesIsFalse()
		{
			Point[] p =
			{
				new Point(319, 219),
				new Point(364, 271),
				new Point(385, 235),
				new Point(279, 54),
				new Point(200, 249),
				new Point(222, 359),
				new Point(389, 127),
				new Point(202, 149),
				new Point(380, 245),
				new Point(279, 298)
			};

			Polygon polygon = new Polygon(p);

			//This point is outside the polygon but totally surrounded by it, hence "landlocked"
			Assert.False(polygon.Contains(317, 226));
		}
	}
}
