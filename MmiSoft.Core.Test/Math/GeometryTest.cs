using System.Drawing;
using MmiSoft.Core.Geometry;
using NUnit.Framework;

namespace MmiSoft.Core.Math;

[TestFixture]
public class GeometryTest
{

	[Test]
	public void Intersection_OnLinesCenter()
	{
		var s1 = new LineSegment(new Point(-1, -1), new Point(1, 1));
		var s2 = new LineSegment(new Point(-2, 2), new Point(2, -2));
		Assert.That(Geometry.Intersection(s1, s2, out IntersectionType type), Is.EqualTo(PointF.Empty));
		Assert.That(type, Is.EqualTo(IntersectionType.BothLines));
	}

	[Test]
	public void Intersection_OnOneLine()
	{
		var s1 = new LineSegment(new Point(-1, -1), new Point(1, 1));
		var s2 = new LineSegment(new Point(8, -8), new Point(12, -12));
		Assert.That(Geometry.Intersection(s1, s2, out IntersectionType type), Is.EqualTo(PointF.Empty));
		Assert.That(type, Is.EqualTo(IntersectionType.OneLine));
	}

	[Test]
	public void Intersection_OnNone()
	{
		var s1 = new LineSegment(new Point(1, 1), new Point(5, 5));
		var s2 = new LineSegment(new Point(8, -8), new Point(12, -12));
		Assert.That(Geometry.Intersection(s1, s2, out IntersectionType type), Is.EqualTo(PointF.Empty));
		Assert.That(type, Is.EqualTo(IntersectionType.NoLine));
	}
		
	[Test]
	public void Intersection_LinesAreParallel()
	{
		var s1 = new LineSegment(new Point(3, 7), new Point(15, 5));
		var s2 = new LineSegment(new Point(0, 4), new Point(12, 2));
		Assert.That(Geometry.Intersection(s1, s2, out IntersectionType type), Is.EqualTo(PointF.Empty));
		Assert.That(type, Is.EqualTo(IntersectionType.Parallel));
	}

	// The cases below use coordinates large enough that the products inside Intersection exceed
	// int.MaxValue (2,147,483,647). With the original int arithmetic e.g. x1*y2 = 5e9 wrapped around and
	// the computed point was garbage; with a wider numeric type the result must land on the known
	// analytic intersection. The tolerance is generous (1 px) -enough to absorb float rounding yet far
	// tighter than any wrapped-around value, so a regression to int math would fail these.
	private const float Tolerance = 1f;

	[Test]
	public void Intersection_ProductExceedsIntMax_PerpendicularLines_DoesNotWrap()
	{
		// x1*y2 = 50000 * 100000 = 5e9 > int.MaxValue
		var vertical = new LineSegment(new Point(50000, 0), new Point(50000, 100000));
		var horizontal = new LineSegment(new Point(0, 50000), new Point(100000, 50000));

		PointF p = Geometry.Intersection(vertical, horizontal, out IntersectionType type);

		Assert.That(type, Is.EqualTo(IntersectionType.BothLines));
		Assert.That(p.X, Is.EqualTo(50000f).Within(Tolerance));
		Assert.That(p.Y, Is.EqualTo(50000f).Within(Tolerance));
	}

	[Test]
	public void Intersection_VeryLargeCoordinates_DiagonalLines_DoesNotWrap()
	{
		// the (a*b)*(c-d) terms reach ~1e16
		var rising = new LineSegment(new Point(0, 0), new Point(200000, 200000));    // y = x
		var falling = new LineSegment(new Point(0, 200000), new Point(200000, 0));   // y = 200000 - x

		PointF p = Geometry.Intersection(rising, falling, out IntersectionType type);

		Assert.That(type, Is.EqualTo(IntersectionType.BothLines));
		Assert.That(p.X, Is.EqualTo(100000f).Within(Tolerance));
		Assert.That(p.Y, Is.EqualTo(100000f).Within(Tolerance));
	}

	[Test]
	public void Intersection_VeryLargeCoordinates_AxisAlignedLines_DoesNotWrap()
	{
		var vertical = new LineSegment(new Point(100000, 0), new Point(100000, 200000));
		var horizontal = new LineSegment(new Point(0, 100000), new Point(200000, 100000));

		PointF p = Geometry.Intersection(vertical, horizontal, out IntersectionType type);

		Assert.That(type, Is.EqualTo(IntersectionType.BothLines));
		Assert.That(p.X, Is.EqualTo(100000f).Within(Tolerance));
		Assert.That(p.Y, Is.EqualTo(100000f).Within(Tolerance));
	}

	[Test]
	public void Intersection_VeryLargeCoordinates_IntersectionOutsideBothSegments_ReturnsNoLine()
	{
		// Infinite lines cross at (150000, 150000), which lies on neither short segment.
		var a = new LineSegment(new Point(0, 0), new Point(50000, 50000));             // y = x
		var b = new LineSegment(new Point(200000, 100000), new Point(250000, 50000));  // y = -x + 300000

		PointF p = Geometry.Intersection(a, b, out IntersectionType type);

		Assert.That(type, Is.EqualTo(IntersectionType.NoLine));
		Assert.That(p.X, Is.EqualTo(150000f).Within(Tolerance));
		Assert.That(p.Y, Is.EqualTo(150000f).Within(Tolerance));
	}

	[Test]
	public void Intersection_VeryLargeCoordinates_IntersectionOnOneSegmentOnly_ReturnsOneLine()
	{
		// Crossing point (100000, 100000) is on the long segment 'a' but outside the short segment 'b'.
		var a = new LineSegment(new Point(0, 0), new Point(200000, 200000));            // y = x
		var b = new LineSegment(new Point(10000, 190000), new Point(40000, 160000));    // y = -x + 200000

		PointF p = Geometry.Intersection(a, b, out IntersectionType type);

		Assert.That(type, Is.EqualTo(IntersectionType.OneLine));
		Assert.That(p.X, Is.EqualTo(100000f).Within(Tolerance));
		Assert.That(p.Y, Is.EqualTo(100000f).Within(Tolerance));
	}

	[Test]
	public void Intersection_VeryLargeCoordinates_ParallelLines_ReturnsParallel()
	{
		var a = new LineSegment(new Point(0, 0), new Point(200000, 200000));        // y = x
		var b = new LineSegment(new Point(0, 100000), new Point(200000, 300000));   // y = x + 100000

		PointF p = Geometry.Intersection(a, b, out IntersectionType type);

		Assert.That(type, Is.EqualTo(IntersectionType.Parallel));
		Assert.That(p, Is.EqualTo(PointF.Empty));
	}
}
