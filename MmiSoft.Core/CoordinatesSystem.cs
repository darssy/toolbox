using System;

namespace MmiSoft.Core
{
	[Flags]
	public enum CoordinatesSystem : byte
	{
		/// <summary>
		/// The "normal" cartesian system, y-y' is vertical and is increasing up, x-x' is horizontal increasing right.
		/// </summary>
		Arithmetic = 0,

		/// <summary>
		/// x and y are swapped. This conveniently converts from mathematical plane angles to navigational angles
		/// </summary>
		SwapAxles = 1,

		/// <summary>
		///	y and y' are swapped, ie values increase down. Useful for converting to screen coordinates where 0,0 is on the
		/// upper left corner as opposed to lower left that would make it conformal to mathematical coordinate system.
		/// </summary>
		NegateY = 2,

		/// <summary>
		/// Shortcut for SwapAxles | NegateY.
		/// </summary>
		GeographicScreen = SwapAxles | NegateY,

		/// <summary>
		///	x and x' are swapped, ie values increase left. No real practical use so far, included for sake of completeness
		/// </summary>
		NegateX = 4
	}
}
