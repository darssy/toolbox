namespace MmiSoft.Parser;

public readonly struct Token<T>
{
	public Token(T type, string lexeme, int lineNumber, int position)
	{
		Type = type;
		Lexeme = lexeme;
		LineNumber = lineNumber;
		Position = position;
	}

	public T Type { get; }
	public string Lexeme { get; }
	public int LineNumber { get; }
	public int Position { get; }

	public static implicit operator string (Token<T> token) => token.Lexeme;

	private string PrintableLexeme => Lexeme.All(char.IsWhiteSpace) ? "" : Lexeme;
	
	public override string ToString()
	{
		return $"@{$"{LineNumber}:{Position}",-10} {Type} '{Lexeme}'";
	}
}
