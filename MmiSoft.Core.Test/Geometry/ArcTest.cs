using MmiSoft.Core.Math.Units;
using NUnit.Framework;

namespace MmiSoft.Core.Geometry
{
	[TestFixture]
	public class ArcTest
	{
		[Test]
		public void MinorArc_Contains_DegreesAreInsideArc()
		{
			Arc arc = new Arc(30.Degrees(), 60.Degrees());
			Assert.True(arc.Contains(40.Degrees()));
		}
		
		[Test]
		public void MinorArc_Contains_DegreesAreOutsideArc()
		{
			Arc arc = new Arc(30.Degrees(), 60.Degrees());
			Assert.False(arc.Contains(90.Degrees()));
		}

		[Test]
		public void MinorArc_Contains_DegreesAreMoreThanFullCircleAndInsideArc()
		{
			Arc arc = new Arc(30.Degrees(), 60.Degrees());
			Assert.True(arc.Contains(410.Degrees()));
		}

		[Test]
		public void MinorArc_Contains_DegreesAreMoreThanFullCircleAndOutsideArc()
		{
			Arc arc = new Arc(30.Degrees(), 60.Degrees());
			Assert.False(arc.Contains(500.Degrees()));
		}

		[Test]
		public void MinorArcStartIsBiggerThanEnd_Contains_DegreesAreAfter360AndInsideArc()
		{
			Arc arc = new Arc(320.Degrees(), 15.Degrees());
			Assert.True(arc.Contains(10.Degrees()));
		}

		[Test]
		public void MinorArcStartIsBiggerThanEnd_Contains_DegreesAreNegativeAndInsideArc()
		{
			Arc arc = new Arc(320.Degrees(), 15.Degrees());
			Assert.True(arc.Contains(new Degrees(-10)));
		}
	}
}
