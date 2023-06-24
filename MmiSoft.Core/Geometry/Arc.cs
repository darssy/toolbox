namespace MmiSoft.Core.Geometry
{
	public readonly struct Arc
	{
		public const double FullCircle = 360;

		private readonly double start;
		private readonly double end;

		public Arc(double start, double end)
		{
			this.start = start > FullCircle ? start % FullCircle : start;
			this.end = end > FullCircle ? end % FullCircle : end;
		}

		public bool Contains(double angle)
		{
			if (angle > FullCircle)
			{
				angle %= FullCircle;
			}
			if (angle < 0)
			{
				angle = FullCircle + angle;
			}
			if (start < end)
			{
				return angle >= start && angle <= end;
			}
			return angle >= start || angle <= end;
		}

		public override string ToString()
		{
			return $"Arc [{start:0.###}->{end:0.###}] Degrees";
		}
	}
}
