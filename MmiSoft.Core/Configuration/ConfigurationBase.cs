using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MmiSoft.Core.IO;
using Newtonsoft.Json;

namespace MmiSoft.Core.Configuration
{
	public abstract class ConfigurationBase
	{
		private static readonly Dictionary<Type, List<JsonConverter>> ConverterCache = new Dictionary<Type, List<JsonConverter>>();

		public event EventHandler<ValueEventArgs<string>> Error;

		protected virtual void OnError(ValueEventArgs<string> e)
		{
			Error?.Invoke(this, e);
		}

		public static T ReadConfig<T>(string fullFilePath) where T : ConfigurationBase
		{
			if (!File.Exists(fullFilePath)) throw new FileNotFoundException($"Could not locate '{fullFilePath}'");
			EventLogger.Info($"Reading configuration from {fullFilePath}");
			return FileReader.ReadJson<T>(fullFilePath, CreateConvertersForClass(typeof(T)));
		}

		public void WriteConfig(string fullFilePath)
		{
			FileWriter.WriteJson(fullFilePath, this, CreateConvertersForClass(GetType()));
			EventLogger.Info($"Configuration saved to {fullFilePath}");
		}

		public void ReadConfig(string fullFilePath)
		{
			string jsonText;
			using (StreamReader fileStream = FileReader.CreateStream(fullFilePath))
			{
				jsonText = fileStream.ReadToEnd();
			}

			try
			{
				JsonConvert.PopulateObject(jsonText, this, CreateConvertersForClass(GetType()));
			}
			catch (JsonSerializationException e)
			{
				OnError(new ValueEventArgs<string>(e.Message));
			}
		}

		private static JsonSerializerSettings CreateConvertersForClass(Type type)
		{
			JsonSerializerSettings jsonSettings = CreateJsonSettings();

			List<JsonConverter> converters = ConverterCache.GetOrCreate(type, () => type
				.GetCustomAttributes(true)
				.OfType<JsonConverterBaseAttribute>()
				.Select(attr => attr.Create())
				.ToList());

			foreach (JsonConverter jsonConverter in converters)
			{
				jsonSettings.Converters.Add(jsonConverter);
			}
			return jsonSettings;
		}

		private static JsonSerializerSettings CreateJsonSettings()
		{
			return new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
				DefaultValueHandling = DefaultValueHandling.Include
			};
		}
	}
}
