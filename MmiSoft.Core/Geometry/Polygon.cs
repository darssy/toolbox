using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MmiSoft.Core.Geometry
{
	[Serializable]
	public class Polygon
	{
		private Rectangle bounds;
		private Point[] points;

		public Polygon(params Point[] points)
		{
			this.points = points;
			bounds = CalculateBounds();
		}

		public Polygon()
		{
			points = new Point[]{};
		}

		public Point[] Points => points;

		private Rectangle CalculateBounds()
		{
			if (points.Length == 0) return Rectangle.Empty;
			int right = points.Max(p => p.X);
			int left = points.Min(p => p.X);
			int bottom = points.Max(p => p.Y);
			int top = points.Min(p => p.Y);
			return new Rectangle(left, top, right - left, bottom - top);
		}

		public bool MbrContains(Point p)
		{
			return bounds.Contains(p);
		}

		public bool Encloses(Point p)
		{
			return Contains(p.X, p.Y);
		}

		public void Offset(int x, int y)
		{
			for (int i = 0; i < points.Length; i++)
			{
				points[i].Offset(x, y);
			}
			bounds.Offset(x, y);
		}

		/// <summary>
		/// Copyright (c) 1970-2003, Wm. Randolph Franklin MIT license. Taken from https://wrf.ecse.rpi.edu/Research/Short_Notes/pnpoly.html
		/// There is a heated debate (https://stackoverflow.com/q/217578/1303323) on which is the right "contains" method
		/// for a polygon. The intentions on this one is  to work with pixels which happen to be integer points. Franklin's
		/// algorithm seems to be suitable for this purpose.
		/// </summary>
		public bool Contains(int x, int y)
		{
			bool c = false;
			for (int i = 0, j = points.Length - 1; i < points.Length; j = i++)
			{
				if ((points[i].Y > y != points[j].Y > y) &&
				    //   |---------------------------λ-----------------------------------|
				    (x < (points[j].X - points[i].X) / (double)(points[j].Y - points[i].Y) * (y - points[i].Y) + points[i].X))
					c = !c;
			}
			return c;
		}

		public ICollection<LineSegment> GetLineSegments()
		{
			List<LineSegment> segments = new List<LineSegment>(Count);

			for (int i = 0; i < Count - 1; i++)
			{
				segments.Add(new LineSegment(this[i], this[i + 1]));
			}
			segments.Add(new LineSegment(this[Count - 1], this[0]));
			return segments;
		}

		public Point this[int index] => points[index];

		public int Count => points.Length;
	}
}
