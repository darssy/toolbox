using MmiSoft.Core;

namespace MmiSoft.Parser;

/// <summary>
/// [] or () or "" or '' (well you get the idea)
/// </summary>
public class CharSyntaxBlock : ITokenizer
{
	private string name;
	private char start;
	private char end;
	private char escape;

	private char[] all;

	public CharSyntaxBlock(string name, char start, char end, char escape = '\0')
	{
		this.start = start;
		this.end = end;
		this.escape = escape;
		this.name = name;
		all = end == start ? new[] {start, escape} : new[] {start, end, escape};
	}

	public bool Matches(string input, int currentIndex) => start == input[currentIndex];

	public string Name => name;

	public Result<TextSegment, string> Consume(string input, int startIndex)
	{
		int occurenceCount = 1;

		for (int i = startIndex + 1; i < input.Length; i++)
		{
			if (input[i] == escape)
			{
				int nextIndex = i + 1;
				if (nextIndex < input.Length && input[nextIndex].IsAnyOf(all))
				{
					i++;
				}
				else
				{
					if (input[i] == end && --occurenceCount == 0)
					{
						return new TextSegment(input, startIndex, i + 1, name);
					}
				}
				//if (end != escape) return new ResultBase<TokenizationResult>("Invalid escape sequence");
			}
			else
			{
				if (start != end && input[i] == start) occurenceCount++;
				if (input[i] == end && --occurenceCount == 0)
				{
					return new TextSegment(input, startIndex, i + 1, name);
				}
			}

		}

		return new Result<TextSegment, string>($"Closing character '{end}' not found");
	}

	public override string ToString() => $"{name} block";

	public static CharSyntaxBlock DoubleQuotedString(char escapeChar)
		=> new CharSyntaxBlock("DQuoted Text", '"', '"', escapeChar);
}
