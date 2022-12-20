using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using MmiSoft.Core.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MmiSoft.Core.IO
{
	[Obsolete("This class was experimental and is now obsolete")]
	public static class FileReader
	{
		public static event EventHandler<ValueEventArgs<LogEntry>> LogEntryCreated;

		public static IList<string> ReadFile(string file)
		{
			return ReadFile(new StreamReader(file));
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

		public static T ReadXml<T>(string filename)
		{
			if (!File.Exists(filename)) throw new FileNotFoundException($"File '{filename}' does not exist", filename);
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			using TextReader textReader = new StreamReader(filename);
			return (T)serializer.Deserialize(textReader);
		}

		[Obsolete("Use JsonFileIO.Read<T>() instead")]
		public static T ReadJson<T>(string filename, JsonSerializerSettings serializerSettings)
		{
			return JsonFileIO.Read<T>(filename, serializerSettings);
		}

		[Obsolete("Use JsonFileIO.Read<T>() instead")]
		public static T ReadJson<T>(string filename, params JsonConverter[] converters)
		{
			return JsonFileIO.Read<T>(filename, converters);
		}

		[Obsolete("This method was experimental and is now obsolete")]
		public static StreamReader CreateStream(string file)
		{
			if (!File.Exists(file)) throw new FileNotFoundException($"File '{file}' does not exist", file);
			try
			{
				return new StreamReader(file);
			}
			catch (Exception e)
			{
				string msg = $"Failed to read file: {e.Message}";
				LogEntry entry = new LogEntry(msg, LogSeverity.Error, "Generic I/O");
				OnLogEntryCreated(new ValueEventArgs<LogEntry>(entry));
			}
			return null;
		}

		private static void OnLogEntryCreated(ValueEventArgs<LogEntry> e)
		{
			LogEntryCreated?.Invoke(null, e);
		}
	}
}
