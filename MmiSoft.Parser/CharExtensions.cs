namespace MmiSoft.Parser;

public static class CharExtensions
{
	public static bool IsAnyOf(this char input, char[] candidates)
	{
		for (int i = 0; i < candidates.Length; i++)
		{
			if (input == candidates[i]) return true;
		}

		return false;
	}
}
