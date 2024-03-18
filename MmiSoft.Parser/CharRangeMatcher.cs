using MmiSoft.Core;

namespace MmiSoft.Parser;

public class CharRangeMatcher : ITokenizer
{
	private string name;
	private ValueRange<char> range;

	public CharRangeMatcher(string name, char start, char end)
	{
		this.name = name;
		range = new ValueRange<char>(start, end);
	}

	public string Name => name;

	public bool Matches(string input, int index) => range.IsInRange(input[index]);

	public Result<TextSegment, string> Consume(string input, int startIndex)
	{
		int oldIndex = startIndex;

		for (; startIndex < input.Length; startIndex++)
		{
			if (!range.IsInRange(input[startIndex]))
			{
				break;
			}
		}

		return new TextSegment(input, oldIndex, startIndex, name);
	}

	public override string ToString() => $"{name} '{range}'";

	public static CharRangeMatcher Digit() => new("Digit", '0', '9');

	public static CharRangeMatcher LatinAlpha() => new("LatinAlpha", 'A', 'z');
}

public class CombinedMatcher : ITokenizer
{
	private string name;
	private ITokenizer[] tokenizers;

	public CombinedMatcher(string name, params ITokenizer[] tokenizers)
	{
		this.name = name;
		this.tokenizers = tokenizers;
	}

	public string Name => name;

	public Result<TextSegment, string> Consume(string input, int startIndex)
	{
		int current = startIndex;
		ITokenizer tokenizer = tokenizers.First(t => t.Matches(input, current));
		do
		{
			Result<TextSegment, string> res = tokenizer.Consume(input, current);
			if (!res.Succeeded) return res;
			current = res.Value.NewIndex;
			if (current == input.Length) break;
			tokenizer = tokenizers.FirstOrDefault(t => t.Matches(input, current));
		} while (tokenizer != null);

		return new TextSegment(input, startIndex, current, name);
	}

	public bool Matches(string input, int currentIndex) => tokenizers.Any(t => t.Matches(input, currentIndex));
}

public class SequentiallyAppliedCombinedMatcher : ITokenizer
{
	private string name;
	private ITokenizer[] tokenizers;

	public SequentiallyAppliedCombinedMatcher(string name, params ITokenizer[] tokenizers)
	{
		this.name = name;
		this.tokenizers = tokenizers ?? throw new ArgumentNullException(nameof(tokenizers));
		if (tokenizers.Length == 0) throw new ArgumentException("There must be at least one tokenizer");
	}

	public string Name => name;

	public Result<TextSegment, string> Consume(string input, int startIndex)
	{
		int current = startIndex;
		for (var i = 0; i < tokenizers.Length; i++)
		{
			Result<TextSegment, string> res = tokenizers[i].Consume(input, current);
			if (!res.Succeeded || res.Value.Empty) return new TextSegment(input, startIndex, startIndex, name); // consume no input
				
			current = res.Value.NewIndex;
			if (current == input.Length) break;
		}

		return new TextSegment(input, startIndex, current, name);
	}

	public bool Matches(string input, int currentIndex) => tokenizers[0].Matches(input, currentIndex);
}
