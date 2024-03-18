using MmiSoft.Core;

namespace MmiSoft.Parser;

public class BlockMatcher : ITokenizer
{
	private string name;
	private string start;
	private string end;

	public BlockMatcher(string name, string start, string end = "")
	{
		if (string.IsNullOrEmpty(start)) throw new ArgumentException(nameof(start));
		this.name = name;
		this.start = start;
		this.end = end ?? throw new ArgumentNullException(nameof(end));
	}

	public string Name => name;

	public Result<TextSegment, string> Consume(string input, int startIndex)
	{
		if (end == "")
		{
			(int ending, int newIndex) = GetLineEndingIndex(input, startIndex);
			return new TextSegment(input, startIndex + start.Length, ending, name, newIndex);
		}

		startIndex += start.Length;
		int oldIndex = startIndex;

		while (!MatchesImpl(input, startIndex++, end))
		{
			if (startIndex + end.Length > input.Length)
			{
				return $"Closing text '{end}' not found.";
			}
		}

		startIndex--;
		return new TextSegment(input, oldIndex, startIndex, name, startIndex + start.Length);
	}

	public bool Matches(string input, int startIndex)
	{
		return MatchesImpl(input, startIndex, start);
	}

	private bool MatchesImpl(string input, int startIndex, string textToMatch)
	{
		if (input.Length - startIndex < textToMatch.Length) return false;
		for (int i = 0; i < textToMatch.Length; i++)
		{
			if (input[startIndex + i] != textToMatch[i]) return false;
		}

		return true;
	}

	private (int, int) GetLineEndingIndex(string input, int startIndex)
	{
		char[] crlf = {'\r', '\n'};
		while (startIndex < input.Length && !input[startIndex].IsAnyOf(crlf))
		{
			startIndex++;
		}
		if (input.Length == startIndex) return (startIndex, startIndex);
		if (input[startIndex] == '\r' && input[startIndex + 1] == '\n')
		{
			return (startIndex, startIndex + 2);
		}
		return (startIndex, startIndex + 1);
	}

	public override string ToString() => $"{name} '{start}'";

	public static BlockMatcher CStyleBlockComment() => new("C-Style Block Comment", "/*", "*/");

	public static BlockMatcher CStyleLineComment() => new("C-Style Line Comment", "//");
}
