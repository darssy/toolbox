namespace MmiSoft.Core
{
	public static class Util
	{
		public static void Swap<T>(ref T x, ref T y)
		{
			T temp = x;
			x = y;
			y = temp;
		}

	}
}
