using MmiSoft.Core;

namespace MmiSoft.Parser;

public class Lexer
{
	private SequentialTokenizerAccess tokenizerAccess;

	public Lexer(params ITokenizer[] syntaxBlocks)
	{
		var tokenizers = new List<ITokenizer>(syntaxBlocks) { CharMatcher.WhiteSpace() };
		tokenizerAccess = new SequentialTokenizerAccess(tokenizers);
	}

	public Result<ICollection<Token<string>>, string> Tokenize(string text, string defaultToken = "Text")
	{
		var tokens = new LinkedList<Token<string>>();

		int start = 0;
		int lastLineBreakIndex = -1;
		int lineCount = 1;
		for (int i = 0; i < text.Length;)
		{
			ITokenizer matchedTokenizer = tokenizerAccess.FirstTokenizer(text, i);

			if (matchedTokenizer == null) i++;
			else
			{
				int column = start - lastLineBreakIndex;
				if (start != i) // found characters not matched by any previous tokenizer, they are tokenized under default
				{
					tokens.AddLast(new Token<string>(defaultToken, text.Substring(start, i - start), lineCount, column));
				}

				Retry:
				Result<TextSegment, string> lexResult = matchedTokenizer.Consume(text, i);
				if (!lexResult.Succeeded) return lexResult.ToString();

				i = lexResult.Value.NewIndex;
				if (!lexResult.Value.Empty)
				{
					tokens.AddLast(new Token<string>(matchedTokenizer.Name, lexResult.Value.GetToken(), lineCount, column));
				}
				else if (lexResult.Value.ConsumedCharCount == 0)
				{
					matchedTokenizer = tokenizerAccess.NextTokenizer(text, i);
					if (matchedTokenizer != null)
					{
						goto Retry;
					}
				}
				for (; start < i; start++)
				{
					if (text[start] == '\n')
					{
						lineCount++;
						lastLineBreakIndex = start;
					}
				}
			}
		}

		if (start != text.Length)
		{
			tokens.AddLast(new Token<string>(defaultToken, text[start..], 0, 0));
		}

		return tokens;
	}

	private class SequentialTokenizerAccess
	{
		private List<ITokenizer> tokenizers;
		private int currentTokenizerIndex;

		public SequentialTokenizerAccess(List<ITokenizer> tokenizers)
		{
			this.tokenizers = tokenizers;
		}

		public ITokenizer FirstTokenizer(string text, int currentIndex)
		{
			for (int i = 0; i < tokenizers.Count; i++)
			{
				if (tokenizers[i].Matches(text, currentIndex))
				{
					currentTokenizerIndex = i;
					return tokenizers[i];
				}
			}
			return null;
		}

		public ITokenizer NextTokenizer(string text, int currentIndex)
		{
			for (int i = currentTokenizerIndex + 1; i < tokenizers.Count; i++)
			{
				if (tokenizers[i].Matches(text, currentIndex))
				{
					currentTokenizerIndex = i;
					return tokenizers[i];
				}
			}
			return null;
		}
	}
}
