using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MmiSoft.Core
{
	public static class StringExtensions
	{
		/// <summary>
		/// Regex pattern to match camel case "humps" or pascal case (except the first character).
		/// <example>
		/// For "ThisIsANewItem" the regex will match <c>I</c> <c>A</c> and <c>I</c> but not the <c>N</c>.
		/// <para>
		///	For thatsAnApple the regex will match the 2 capital <c>A</c>s.
		/// </para>
		/// </example>
		/// </summary>
		public static readonly Regex PascalCaseRegex = new Regex("(?<=[a-z])([A-Z])", RegexOptions.Compiled);

		private static readonly string[] NewLines = { "\r\n", "\n" };

		public static string SplitCamelCase(this string input) => PascalCaseRegex.Replace(input, " $1").Trim();

		public static string[] SplitNewLine(this string s) => s?.Split(NewLines, StringSplitOptions.None);

		/// <summary>
		/// A more straightforward syntax for <c>elements.Any(s.Contains) are strings</c>
		/// </summary>
		/// <param name="s">The string to test</param>
		/// <param name="elements">The substrings to test if they are contained in <c>s</c></param>
		/// <returns><c>true</c> if <paramref name="s" /> contains at least one of the <paramref name="elements" /></returns>
		public static bool ContainsAnyOf(this string s, IEnumerable<string> elements) => elements.Any(s.Contains);

		/// <summary>
		/// A more straightforward syntax for <c>elements.Any(s.Contains) when elements are characters</c>
		/// </summary>
		/// <param name="s">The string to test</param>
		/// <param name="elements">The characters to test if they are contained in <c>s</c></param>
		/// <returns><c>true</c> if <paramref name="s" /> contains at least one of the <paramref name="elements" /></returns>
		public static bool ContainsAnyOf(this string s, IEnumerable<char> elements) => elements.Any(s.Contains);

		/// <summary>
		/// Joins a series of characters with the separator into a string
		/// </summary>
		/// <param name="chars">The chars to join</param>
		/// <param name="separator">The separator to use. Default is space</param>
		/// <returns></returns>
		public static string Join(this IEnumerable<char> chars, char separator = ' ')
		{
			if (chars is not char[] charArray)
			{
				charArray = chars.ToArray();
			}
			if (charArray.Length == 0) return "";
			StringBuilder builder = new(charArray.Length * 2);
			for (var i = 0; i < charArray.Length - 1; i++)
			{
				builder.Append(charArray[i]).Append(separator);
			}
			builder.Append(charArray.Last());
			return builder.ToString();
		}

		/// <summary>
		/// An easier way of getting the last character of a string. So instead of writing
		/// <c>myVeryLongNamedString[myVeryLongNamedString.Length - 1]</c> you can write instead
		/// <c>myVeryLongNamedString.Last()</c>
		/// </summary>
		/// <param name="str">The string to return the last character of</param>
		/// <returns>Last character of a string</returns>
		/// <exception cref="T:System.IndexOutOfRangeException">
		/// <paramref name="str"/> is empty string</exception>
		public static char Last(this string str) => str[str.Length - 1];
	}
}
