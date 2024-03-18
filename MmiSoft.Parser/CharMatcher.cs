using MmiSoft.Core;

namespace MmiSoft.Parser;

public class CharMatcher : ITokenizer
{
	private string name;
	private char match;
	private TokenizationPolicy policy;

	public CharMatcher(string name, char match, TokenizationPolicy policy = TokenizationPolicy.Tokenize)
	{
		this.name = name;
		this.match = match;
		this.policy = policy;
	}

	public string Name => name;

	public bool Matches(string input, int index) => input[index] == match;

	public Result<TextSegment, string> Consume(string input, int startIndex)
	{
		int oldIndex = startIndex;
		switch (policy)
		{
			case TokenizationPolicy.Tokenize:
				for (; startIndex < input.Length; startIndex++)
				{
					if (input[startIndex] != match)
					{
						break;
					}
				}
				break;
			case TokenizationPolicy.Discard:
				for (; startIndex < input.Length && input[startIndex] != match; startIndex++) { }
				startIndex++;
				return new TextSegment(input, oldIndex, oldIndex, name, startIndex);
			case TokenizationPolicy.Include:
				for (int i = startIndex; i < input.Length; i++)
				{
					if (input[i] != match)
					{
						break;
					}
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		return new TextSegment(input, oldIndex, startIndex, name);
	}

	public override string ToString() => $"{name} '{match}'";

	public static CharMatcher WhiteSpace() => new ("Space", ' ', TokenizationPolicy.Discard);
}
