using System;
using System.Drawing;
using MmiSoft.Core.Math;

namespace MmiSoft.Core
{
	public static class DrawingExtensions
	{
		public static Size Half(this Size s) => new Size(s.Width / 2, s.Height / 2);

		public static Point Center(this Size s) => new Point(s.Width / 2, s.Height / 2);

		public static Point Center(this Rectangle r) => new Point(r.X + r.Width / 2, r.Y + r.Height / 2);

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

		/// <summary>
		/// Returns the azimuth or bearing of the second argument relative to the first one.
		/// </summary>
		/// <param name="from">The start point or the "observer"</param>
		/// <param name="to">The end point or the "destination"</param>
		/// <param name="cs">The coordinates system to use </param>
		/// <returns>The azimuth of <c>to</c> relative to <c>from</c> in radians.</returns>
		public static double AzimuthTo(this Point from, Point to, CoordinatesSystem cs = CoordinatesSystem.Arithmetic)
		{
			int dx = to.X - from.X;
			int dy = to.Y - from.Y;

			if ((CoordinatesSystem.NegateX & cs) == CoordinatesSystem.NegateX)
			{
				dx = -dx;
			}

			if ((CoordinatesSystem.NegateY & cs) == CoordinatesSystem.NegateY)
			{
				dy = -dy;
			}

			if ((CoordinatesSystem.SwapAxles & cs) == CoordinatesSystem.SwapAxles)
			{
#if NET47_OR_GREATER
				(dx, dy) = (dy, dx);
#else
				Util.Swap(ref dx, ref dy);
#endif
			}

			return System.Math.Atan2(dx, dy);
		}

		public static Point ExtrapolateTo(this Point from, double bearing, double distance,
			CoordinatesSystem cs = CoordinatesSystem.Arithmetic)
		{
			int ex = (distance * System.Math.Cos(bearing)).RoundToInt();
			int ey = (distance * System.Math.Sin(bearing)).RoundToInt();

			if ((cs & CoordinatesSystem.SwapAxles) == CoordinatesSystem.SwapAxles)
			{
				Util.Swap(ref ex, ref ey);
			}
			if ((cs & CoordinatesSystem.NegateX) == CoordinatesSystem.NegateX)
			{
				ex = -ex;
			}
			if ((cs & CoordinatesSystem.NegateY) == CoordinatesSystem.NegateY)
			{
				ey = -ey;
			}

			return new Point(from.X + ex, from.Y + ey);
		}

		public static Rectangle Subtract(this Rectangle r, Size subtraction)
		{
			return new Rectangle(r.X, r.Y, r.Width - subtraction.Width, r.Height - subtraction.Height);
		}
	}

}
