using MmiSoft.Core;

namespace MmiSoft.Parser;

public interface ITokenizer
{
	string Name { get; }
	Result<TextSegment, string> Consume(string input, int startIndex);
	bool Matches(string input, int currentIndex);
}
