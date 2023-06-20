using NUnit.Framework;

namespace MmiSoft.Core.Geometry
{
	[TestFixture]
	public class ArcTest
	{
		[Test]
		public void MinorArc_Contains_DegreesAreInsideArc()
		{
			Arc arc = new Arc(30, 60);
			Assert.True(arc.Contains(40));
		}
		
		[Test]
		public void MinorArc_Contains_DegreesAreOutsideArc()
		{
			Arc arc = new Arc(30, 60);
			Assert.False(arc.Contains(90));
		}

		[Test]
		public void MinorArc_Contains_DegreesAreMoreThanFullCircleAndInsideArc()
		{
			Arc arc = new Arc(30, 60);
			Assert.True(arc.Contains(410));
		}

		[Test]
		public void MinorArc_Contains_DegreesAreMoreThanFullCircleAndOutsideArc()
		{
			Arc arc = new Arc(30, 60);
			Assert.False(arc.Contains(500));
		}

		[Test]
		public void MinorArcStartIsBiggerThanEnd_Contains_DegreesAreAfter360AndInsideArc()
		{
			Arc arc = new Arc(320, 15);
			Assert.True(arc.Contains(10));
		}

		[Test]
		public void MinorArcStartIsBiggerThanEnd_Contains_DegreesAreNegativeAndInsideArc()
		{
			Arc arc = new Arc(320, 15);
			Assert.True(arc.Contains(-10));
		}
	}
}
