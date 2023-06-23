using System;

namespace MmiSoft.Core.Text
{

	public interface IResultTextParser<T>
	{
		ResultBase<T> Parse(string text);
	}
}
