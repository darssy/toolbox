namespace MmiSoft.Parser;

public readonly struct TextSegment
{
	private readonly string source;
	private readonly int start;
	private readonly int end;

	public TextSegment(string source, int start, int end, string tokenType, int newIndex = -1)
	{
		if (start < 0) throw new ArgumentException("Negative start argument");
		if (start > end) throw new ArgumentException("Start must be less than or equal to end");
		this.source = source ?? throw new ArgumentNullException(nameof(source));
		this.start = start;
		this.end = end;
		NewIndex = newIndex == -1 ? end : newIndex;
		TokenType = tokenType;
	}

	public string GetToken() => source.Substring(start, end - start);

	public string TokenType { get; }

	public int NewIndex { get; }

	public int ConsumedCharCount => NewIndex - start;

	public bool Empty => start == end;

	//public override string ToString() => $" {TokenType} {nameof(Token)}: '{Token}' @{NewIndex}";
}
