using System;

namespace MmiSoft.Core.Text
{
	[Obsolete("Replaced by " + nameof(IResultTextParser<T>))]
	public interface ITextParser<out T>
	{
		bool IsValid(string text);
		T Parse(string text);
	}

	public interface IResultTextParser<T>
	{
		ResultBase<T> Parse(string text);
	}
}
