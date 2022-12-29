using System;
using System.IO;
using MmiSoft.Core.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MmiSoft.Core.IO
{
	public static class JsonFileIO
	{
		private static readonly StringEnumConverter StringEnumConverter = new StringEnumConverter();
		private static readonly IpAddressJsonConverter IpAddressConverter = new IpAddressJsonConverter();

		public static T Read<T>(string filename, params JsonConverter[] converters)
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

			return Read<T>(filename, settings);
		}

		public static T Read<T>(string filename, JsonSerializerSettings serializerSettings = null)
		{
			using var streamReader = new StreamReader(filename);
			return Read<T>(streamReader, serializerSettings);
		}

		public static T Read<T>(TextReader reader, JsonSerializerSettings serializerSettings = null)
		{
			using var jsonTextReader = new JsonTextReader(reader);

			serializerSettings ??= CreateSettingsWithConvertersForClass(typeof(T));
			JsonSerializer serializer = JsonSerializer.CreateDefault(serializerSettings);

			return serializer.Deserialize<T>(jsonTextReader);
		}

		public static void WriteWithConverters(string filename, object obj, params JsonConverter[] converters)
		{
			JsonSerializerSettings settings = CreateJsonSettings(null);
			settings.Converters.Add(new StringEnumConverter());
			settings.Converters.Add(new IpAddressJsonConverter());
			foreach (JsonConverter converter in converters)
			{
				settings.Converters.Add(converter);
			}

			Write(filename, obj, settings);
		}

		public static void Write(string filename, object obj, JsonSerializerSettings settings = null)
		{
			using StreamWriter streamWriter = new StreamWriter(filename);
			Write(streamWriter, obj, settings);
		}

		public static void Write(TextWriter writer, object obj, JsonSerializerSettings settings = null)
		{
			if (obj == null) throw new ArgumentNullException(nameof(obj));

			settings ??= CreateSettingsWithConvertersForClass(obj.GetType());

			JsonSerializer serializer = JsonSerializer.CreateDefault(settings);
			serializer.Serialize(writer, obj);
		}

		internal static JsonSerializerSettings CreateSettingsWithConvertersForClass(Type type,
			ITraceWriter traceWriter = null)
		{
			JsonSerializerSettings jsonSettings = CreateJsonSettings(traceWriter);

			foreach (JsonConverter jsonConverter in type.GetJsonConverters())
			{
				jsonSettings.Converters.Add(jsonConverter);
			}
			return jsonSettings;
		}

		internal static JsonSerializerSettings CreateJsonSettings(ITraceWriter traceWriter)
		{
			return new JsonSerializerSettings
			{
				TraceWriter = traceWriter,
				Formatting = Formatting.Indented,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
				DefaultValueHandling = DefaultValueHandling.Include
			};
		}
	}
}
