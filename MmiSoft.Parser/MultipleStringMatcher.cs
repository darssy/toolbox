using MmiSoft.Core;

namespace MmiSoft.Parser;

public class MultipleStringMatcher : ITokenizer
{
	private List<string> stringsToMatch;

	public MultipleStringMatcher(string name, params string[] stringsToMatch)
	{
		Name = name;
		this.stringsToMatch = new List<string>(stringsToMatch);
		this.stringsToMatch.Sort((s1, s2) => s1.Length.CompareTo(s2.Length));
	}

	public string Name { get; }

	public Result<TextSegment, string> Consume(string input, int startIndex)
	{
		int remainingInput = input.Length - startIndex;
		if (remainingInput < stringsToMatch[0].Length) return "Unexpected end of file";

		for (int i = 0; i < stringsToMatch.Count; i++)
		{
			string currentStr = stringsToMatch[i];
			if (remainingInput < currentStr.Length) break; //sorted by length; next strings will also not fit the size
			int endIndex = startIndex + currentStr.Length;
			for (int j = startIndex; j <= endIndex; j++)
			{
				if (new StringSegment(input, startIndex, stringsToMatch[i].Length).Equals(stringsToMatch[i]))
				{
					return new TextSegment(input, startIndex, startIndex + stringsToMatch[i].Length, Name);
				}
			}
		}
		return "Mismatch: input given didn't match any of the expected input strings";
	}

	public bool Matches(string input, int currentIndex)
	{
		int remainingInput = input.Length - currentIndex;
		if (remainingInput < stringsToMatch[0].Length) return false;

		for (int i = 0; i < stringsToMatch.Count; i++)
		{
			string currentStr = stringsToMatch[i];
			if (remainingInput < currentStr.Length) break; //sorted by length; next strings will also not fit the size
			int endIndex = currentIndex + currentStr.Length;
			for (int j = currentIndex; j <= endIndex; j++)
			{
				if (new StringSegment(input, currentIndex, stringsToMatch[i].Length).Equals(stringsToMatch[i]))
				{
					return true;
				}
			}
		}
		return false;
	}

	public override string ToString() => Name;
}
