using System.Drawing;
using System.Runtime.CompilerServices;

namespace MmiSoft.Core
{
	using SysMath = System.Math;
	public static class ColorExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Color MakeTransparent(this Color color, Percent opacity)
		{
			return Color.FromArgb(255 * opacity, color.R, color.G, color.B);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Color Negate(this Color c)
		{
			return Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Color Darken(this Color c)
		{
			return Color.FromArgb((int) SysMath.Round(c.R * 0.5),
				(int) SysMath.Round(c.G * 0.5),
				(int) SysMath.Round(c.B * 0.5));
		}

		public static Color Lighten(in this Color c)
		{
			int r = (int)SysMath.Round(c.R * 1.45);
			int g = (int)SysMath.Round(c.G * 1.45);
			int b = (int)SysMath.Round(c.B * 1.45);
			if (r > 255) r = 255;
			if (g > 255) g = 255;
			if (b > 255) b = 255;
			return Color.FromArgb(r, g, b);
		}
	}
}
