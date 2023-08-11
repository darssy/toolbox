using System;

namespace MmiSoft.Core.Text
{
	public interface ITextParser<TResult, TError>
	{
		Result<TResult, TError> Parse(string text);
	}

	[Obsolete("Use ITextParser instead")]
	public interface IResultTextParser<T>
	{
		ResultBase<T> Parse(string text);
	}
}
