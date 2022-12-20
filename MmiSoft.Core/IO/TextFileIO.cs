using System;
using System.Collections.Generic;
using System.IO;

namespace MmiSoft.Core.IO
{
	[Obsolete("This class was experimental and is now obsolete")]
	public class TextFileIO
	{
		public static IList<string> ReadFile(string file)
		{
			return ReadFile(CreateStreamReader(file));
		}

		public static IList<string> ReadFile(StreamReader fileStream)
		{
			if (fileStream == null) return new List<string>();
			List<string> lines = new List<string>();

			using (fileStream)
			{
				string line;
				while ((line = fileStream.ReadLine()) != null)
				{
					lines.Add(line);
				}
			}
			return lines;
		}

		public static void WriteFile(string filename, string text)
		{
			using var textWriter = new StreamWriter(filename);
			try
			{
				textWriter.Write(text);
			}
			catch (Exception exc)
			{
				exc.Log($"Failed to write to file '{filename}'");
			}
		}

		public static StreamReader CreateStreamReader(string file)
		{
			if (!File.Exists(file)) throw new FileNotFoundException($"File '{file}' does not exist", file);
			try
			{
				return new StreamReader(file);
			}
			catch (Exception e)
			{
				e.Log($"Failed to read file '{file}'", "File I/O");
			}
			return null;
		}

		public static StreamWriter CreateStreamWriter(string file)
		{
			if (!File.Exists(file)) throw new FileNotFoundException($"File '{file}' does not exist", file);
			try
			{
				return new StreamWriter(file);
			}
			catch (Exception e)
			{
				e.Log($"Failed to read file '{file}'", "File I/O");
			}
			return null;
		}
	}
}
