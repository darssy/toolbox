namespace MmiSoft.Core
{
	public static class Util
	{
		/// <summary>
		/// A method for swapping two values in a one liner. Apparently in modern C# this can be done with
		/// <c>(x, y) = (y, x)</c>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <typeparam name="T"></typeparam>
		public static void Swap<T>(ref T x, ref T y)
		{
			T temp = x;
			x = y;
			y = temp;
		}

	}
}
