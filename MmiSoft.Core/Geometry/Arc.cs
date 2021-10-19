using MmiSoft.Core.Math.Units;

namespace MmiSoft.Core.Geometry
{
	public class Arc
	{
		private Degrees start;
		private Degrees end;

		public Arc(Degrees start, Degrees end)
		{
			this.start = start > Degrees.FullCircle ? new Degrees(start.UnitValue % Degrees.FullCircle.UnitValue) : start;
			this.end = end > Degrees.FullCircle ? new Degrees(end.UnitValue % Degrees.FullCircle.UnitValue) : end;
		}

		public bool Contains(Degrees angle)
		{
			return Contains(angle.UnitValue);
		}

		public bool Contains(double angle)
		{
			if (angle > Degrees.FullCircle.UnitValue)
			{
				angle %= Degrees.FullCircle.UnitValue;
			}
			if (angle < 0)
			{
				angle = Degrees.FullCircle.UnitValue + angle;
			}
			if (start < end)
			{
				return angle >= start.UnitValue && angle <= end.UnitValue;
			}
			return angle >= start.UnitValue || angle <= end.UnitValue;
		}

		public override string ToString()
		{
			return $"Arc [{start.UnitValue}->{end.UnitValue}] Degrees";
		}
	}
}
