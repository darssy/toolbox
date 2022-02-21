using System;
using System.IO;
using System.Xml.Serialization;
using MmiSoft.Core.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MmiSoft.Core.IO
{
	public static class FileWriter
	{
		public static void WriteText(string filename, string text)
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

		public static void WriteXml<T>(string filename, T item)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			using var textWriter = new StreamWriter(filename);
			try
			{
				serializer.Serialize(textWriter, item);
			}
			catch (Exception exc)
			{
				exc.Log("XML serialization failed");
			}
		}

		public static void WriteJson(string filename, object obj, params JsonConverter[] converters)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
				DefaultValueHandling = DefaultValueHandling.Include
			};
			settings.Converters.Add(new StringEnumConverter());
			settings.Converters.Add(new IpAddressJsonConverter());
			foreach (JsonConverter converter in converters)
			{
				settings.Converters.Add(converter);
			}

			WriteJson(filename, obj, settings);
		}

		public static void WriteJson(string filename, object obj, JsonSerializerSettings settings)
		{
			using TextWriter writer = new StreamWriter(filename);
			JsonSerializer serializer = JsonSerializer.CreateDefault(settings);
			serializer.Serialize(writer, obj);
		}
	}
}
