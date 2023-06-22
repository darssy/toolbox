using System;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace MmiSoft.Core.IO
{
	public static class FileWriter
	{
		[Obsolete("This method was experimental and is now obsolete")]
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

		[Obsolete("Use XmlFileIO.WriteXml() instead")]
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

		[Obsolete("Use JsonFileIO.WriteWithConverters() instead")]
		public static void WriteJson(string filename, object obj, params JsonConverter[] converters)
		{
			JsonFileIO.WriteWithConverters(filename, obj, converters);
		}

		[Obsolete("Use JsonFileIO.Write() instead")]
		public static void WriteJson(string filename, object obj, JsonSerializerSettings settings)
		{
			JsonFileIO.Write(filename, obj, settings);
		}
	}
}
