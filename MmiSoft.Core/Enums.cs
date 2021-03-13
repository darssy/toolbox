using System;

namespace MmiSoft.Core
{
	public static class Enums
	{
		public static T[] GetValues<T>() where T : Enum
		{
			return (T[]) Enum.GetValues(typeof(T));
		}

		public static T Parse<T>(string text, bool ignoreCase=false) where T : Enum
		{
			return (T) Enum.Parse(typeof(T), text, ignoreCase);
		}

		public static int GetOrdinal(this Enum e)
		{
			string name = Enum.GetName(e.GetType(), e);
			return Array.IndexOf(Enum.GetNames(e.GetType()), name);
		}
	}
}
