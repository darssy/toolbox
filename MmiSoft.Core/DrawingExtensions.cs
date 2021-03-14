using System.Drawing;
using MmiSoft.Core.Math;

namespace MmiSoft.Core
{
	public static class DrawingExtensions
	{
		public static Size Half(this Size s)
		{
			return new Size(s.Width / 2, s.Height / 2);
		}

		public static Point Center(this Size s)
		{
			return new Point(s.Width / 2, s.Height / 2);
		}

		public static void InvertRef(ref this Point p)
		{
			p.X = -p.X;
			p.Y = -p.Y;
		}

		public static Point Invert(this Point p)
		{
			return new Point(-p.X, -p.Y);
		}

		public static Point Center(this Point p, Point p2)
		{
			return new Point((p.X + p2.X) / 2, (p.Y + p2.Y) / 2);
		}

		public static Point Subtract(this Point p, Point subtraction)
		{
			return new Point(p.X - subtraction.X, p.Y - subtraction.Y);
		}

		public static void SubtractRef(ref this Point p, Point subtraction)
		{
			p.X -= subtraction.X;
			p.Y -= subtraction.Y;
		}

		public static Point Add(this Point p, Point addition)
		{
			return new Point(p.X + addition.X, p.Y + addition.Y);
		}

		public static void Scale(ref this Point p, float scaleRatio)
		{
			p.X = (p.X * scaleRatio).Round();
			p.Y = (p.Y * scaleRatio).Round();
		}

		public static void Scale(ref this Point p, double scaleRatio)
		{
			p.X = (int) (p.X * scaleRatio).Round();
			p.Y = (int) (p.Y * scaleRatio).Round();
		}

		public static double DistanceTo(this Point p1, Point p2)
		{
			int dx = p1.X - p2.X;
			int dy = p1.Y - p2.Y;
			return (dx * dx + dy * dy).Sqrt();
		}

		public static Rectangle Subtract(this Rectangle r, Size subtraction)
		{
			return new Rectangle(r.X, r.Y, r.Width - subtraction.Width, r.Height - subtraction.Height);
		}
	}
}
