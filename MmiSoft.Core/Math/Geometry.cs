using System.Drawing;
using MmiSoft.Core.Geometry;

namespace MmiSoft.Core.Math
{
	using System;

	public static class Geometry
	{
		private const double RadiansPerDegree = Math.PI / 180.0;

		public static double ToRadians(double angdeg)
		{
			return angdeg * RadiansPerDegree;
		}

		public static double ToDegrees(double angrad)
		{
			return angrad / RadiansPerDegree;
		}

		public static float ToDegrees(float angrad)
		{
			return angrad / (float) RadiansPerDegree;
		}

		public static double CalculateHypotenuse(double side1, double side2)
		{
			return Math.Sqrt(side1 * side1 + side2 * side2);
		}

		public static PointF Intersection(LineSegment s1, LineSegment s2, out IntersectionType type)
		{
			int x1 = s1.X1;
			int x2 = s1.X2;
			int x3 = s2.X1;
			int x4 = s2.X2;

			int y1 = s1.Y1;
			int y2 = s1.Y2;
			int y3 = s2.Y1;
			int y4 = s2.Y2;
			int denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
			if (denominator == 0)
			{
				type = IntersectionType.Parallel;
				return Point.Empty;
			}

			float px = (x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4);
			float py = (x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4);

			px /= denominator;
			py /= denominator;

			if (px.Between(x1, x2) && py.Between(y1, y2) && px.Between(x3, x4) && py.Between(y3, y4))
			{
				type = IntersectionType.BothLines;
			}
			else if (px.Between(x1, x2) && py.Between(y1, y2) || px.Between(x3, x4) && py.Between(y3, y4))
			{
				type = IntersectionType.OneLine;
			}
			else type = IntersectionType.NoLine;

			return new PointF(px, py);
		}
	}
}
