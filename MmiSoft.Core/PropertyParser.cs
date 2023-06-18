using System;
using System.Collections.Generic;

namespace MmiSoft.Core
{
	/// <summary>
	/// Parses a text that is expected to contain key value pairs, one on each line
	/// </summary>
	public static class PropertyParser
	{
		/// <summary>
		/// Parses a series of line that are expected to contain key value pairs and returns a dictionary with the parsed values.
		/// Each line is expected to contain one property and its value. Results are trimmed.
		/// </summary>
		/// <param name="lines">The lines containing the properties</param>
		/// <param name="separator">The delimiter separating the key from its value</param>
		/// <returns></returns>
		public static IDictionary<string, string> ReadProperties(IList<string> lines, char separator = '=')
		{
			char[] trimChars = {' ', '\t'};
			char[] separatorArr = { separator };
			Dictionary<string, string> properties = new();
			for (int i = 0; i < lines.Count; i++)
			{
				string line = lines[i].Trim();
				if (line == "") continue;
				(string name, string value, _) = line.Split(separatorArr, 2, StringSplitOptions.None);
				if (name == line) //delimiter was not found
				{
					continue;
				}
				value = value.TrimStart();
				const string lineContinuation = "\\";
				while (line.EndsWith(lineContinuation) && i < lines.Count)
				{
					value = value.Substring(0, value.Length - lineContinuation.Length).Trim(trimChars);
					line = lines[++i];
					value += Environment.NewLine + line.Trim();
				}
				properties.Add(name.Trim().ToLower(), value);
			}

			return properties;
		}
	}
}
