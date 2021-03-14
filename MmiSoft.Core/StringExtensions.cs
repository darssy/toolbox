using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MmiSoft.Core
{
	public static class StringExtensions
	{
		public static readonly Regex PascalCaseRegex = new Regex("(?<=[a-z])([A-Z])", RegexOptions.Compiled);

		private static readonly string[] NewLines = { "\r\n", "\n" };

		public static string SplitCamelCase(this string input) => PascalCaseRegex.Replace(input, " $1").Trim();

		public static string[] SplitNewLine(this string s) => s?.Split(NewLines, StringSplitOptions.None);

		public static bool ContainsAnyOf(this string s, IEnumerable<string> elements) => elements.Any(s.Contains);
	}
}
