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
			using var jsonTextReader = new JsonTextReader(streamReader);

			serializerSettings ??= CreateSettingsWithConvertersForClass(typeof(T));
			JsonSerializer serializer = JsonSerializer.CreateDefault(serializerSettings);

			return serializer.Deserialize<T>(jsonTextReader);
		}

		public static void Write(string filename, object obj, JsonSerializerSettings settings = null)
		{
			if (obj == null) throw new ArgumentNullException(nameof(obj));

			settings ??= CreateSettingsWithConvertersForClass(obj.GetType());

			using TextWriter writer = new StreamWriter(filename);
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

		private static JsonSerializerSettings CreateJsonSettings(ITraceWriter traceWriter)
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
