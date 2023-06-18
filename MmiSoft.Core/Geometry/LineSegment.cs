using System.Drawing;
using MmiSoft.Core.Math;

namespace MmiSoft.Core.Geometry
{
	public class LineSegment
	{
		private readonly Point p1;
		private readonly Point p2;

		private readonly int diffX;
		private readonly int diffY;

		public LineSegment(Point p1, Point p2)
		{
			this.p1 = p1;
			this.p2 = p2;
			diffX = p2.X - p1.X;
			diffY = p2.Y - p1.Y;
		}

		public Point P1 => p1;

		public Point P2 => p2;

		public int X1 => p1.X;

		public int Y1 => p1.Y;

		public int X2 => p2.X;

		public int Y2 => p2.Y;

		public double GetLength()
		{
			return p1.DistanceTo(p2);
		}

		public double Distance(Point p)
		{
			int numerator = diffY * p.X - diffX * p.Y + p2.X * p1.Y - p2.Y * p1.X;
			double denominator = (diffY * diffY + diffX * diffX).Sqrt();
			return numerator.Abs() / denominator;
		}

		/*public Rectangle GetBounds()
		{
			int right = Math.Max(p1.X, p2.X);
			int left = Math.Min(p1.X, p2.X);
			BROKEN: bottom and top are referring to screen coordinates. For cartesian they need to be swapped
			int bottom = Math.Max(p1.Y, p2.Y);
			int top = Math.Min(p1.Y, p2.Y);
			return new Rectangle(left, top, right - left, bottom - top);
		}*/

		public Point GetPointAt(Percent percent)
		{
			int percentX = diffX * percent;
			int percentY = diffY * percent;

			return new Point(p1.X + percentX, p1.Y + percentY);
		}

		public bool IsNear(Point p, int sensitivity)
		{
			return Distance(p) <= sensitivity
			       && System.Math.Max(p1.DistanceTo(p), p2.DistanceTo(p)).Round() <= GetLength() + sensitivity;
		}

		public static bool operator ==(LineSegment x, LineSegment y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
			return x.Equals(y);
		}
		
		public static bool operator !=(LineSegment x, LineSegment y)
		{
			return !(x == y);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is LineSegment other)) return false;
			return (p1 == other.p1 && p2 == other.p2) || (p2 == other.p1 && p1 == other.p2);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = (p1.X + 53) * (p1.Y + 127);
				hash += (p2.X + 53) * (p2.Y + 127);
				return hash + 103;
			}
		}

		public override string ToString()
		{
			return $"Line[p1 {p1}, p2 {p2}]";
		}
	}
}
