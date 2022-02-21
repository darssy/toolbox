using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using MmiSoft.Core.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MmiSoft.Core.IO
{
	public static class FileReader
	{
		private static readonly StringEnumConverter StringEnumConverter = new StringEnumConverter();
		private static readonly IpAddressJsonConverter IpAddressConverter = new IpAddressJsonConverter();
		public static event EventHandler<ValueEventArgs<LogEntry>> LogEntryCreated;

		public static IList<string> ReadFile(string file)
		{
			return ReadFile(CreateStream(file));
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
			if (!File.Exists(filename)) throw new FileNotFoundException("", filename);
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			using TextReader textReader = new StreamReader(filename);
			return (T)serializer.Deserialize(textReader);
		}

		public static T ReadJson<T>(string filename, JsonSerializerSettings serializerSettings)
		{
			JsonSerializer serializer = JsonSerializer.CreateDefault(serializerSettings);

			using var streamReader = new StreamReader(filename);
			using var jsonTextReader = new JsonTextReader(streamReader);
			return serializer.Deserialize<T>(jsonTextReader);
		}

		public static T ReadJson<T>(string filename, params JsonConverter[] converters)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
				DefaultValueHandling = DefaultValueHandling.Include
			};
			settings.Converters.Add(StringEnumConverter);
			settings.Converters.Add(IpAddressConverter);

			foreach (JsonConverter converter in converters)
			{
				settings.Converters.Add(converter);
			}

			return ReadJson<T>(filename, settings);
		}

		public static StreamReader CreateStream(string file)
		{
			if (!File.Exists(file)) throw new FileNotFoundException("The file specified does no exist", file);
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
