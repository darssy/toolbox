namespace MmiSoft.Core.Geometry
{
	public readonly struct Arc
	{
		public const double FullCircle = 360;

		private readonly double start;
		private readonly double end;

		public Arc(double start, double end)
		{
			this.start = Normalize(start);
			this.end = Normalize(end);
		}

		public bool Contains(double angle)
		{
			angle = Normalize(angle);
			if (start < end)
			{
				return angle >= start && angle <= end;
			}
			return angle >= start || angle <= end;
		}

		/// <summary>
		/// Maps any angle to its equivalent in the <c>[0, 360)</c> range, so negative and over-360 inputs
		/// are treated consistently whether they come from the constructor or <see cref="Contains"/>.
		/// </summary>
		private static double Normalize(double angle)
		{
			angle %= FullCircle;
			return angle < 0 ? angle + FullCircle : angle;
		}

		public override string ToString()
		{
			return $"Arc [{start:0.###}->{end:0.###}] Degrees";
		}
	}
}
