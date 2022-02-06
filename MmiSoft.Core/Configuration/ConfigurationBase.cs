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
		private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
		{
			Formatting = Formatting.Indented,
			ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
			DefaultValueHandling = DefaultValueHandling.Include
		};

		public event EventHandler<ValueEventArgs<string>> Error;

		protected virtual void OnError(ValueEventArgs<string> e)
		{
			Error?.Invoke(this, e);
		}

		public static T ReadConfig<T>(string fullFilePath) where T : ConfigurationBase
		{
			if (!File.Exists(fullFilePath)) throw new FileNotFoundException($"Could not locate '{fullFilePath}'");
			CreateConvertersForClass(typeof(T));
			EventLogger.Info($"Reading configuration from {fullFilePath}");
			return FileReader.ReadJson<T>(fullFilePath, JsonSettings);
		}

		public void WriteConfig(string fullFilePath)
		{
			CreateConvertersForClass(GetType());
			FileWriter.WriteJson(fullFilePath, this, JsonSettings);
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
				CreateConvertersForClass(GetType());
				JsonConvert.PopulateObject(jsonText, this, JsonSettings);
			}
			catch (JsonSerializationException e)
			{
				OnError(new ValueEventArgs<string>(e.Message));
			}
		}

		private static readonly Dictionary<Type, List<JsonConverter>> ConverterCache = new Dictionary<Type, List<JsonConverter>>();

		private static void CreateConvertersForClass(Type type)
		{
			JsonSettings.Converters.Clear();

			//Could use some more clever caching on Converter level as well.
			List<JsonConverter> converters = ConverterCache.GetOrCreate(type, () => type
				.GetCustomAttributes(true)
				.OfType<JsonConverterBaseAttribute>()
				.Select(attr => attr.Create())
				.ToList());

			foreach (JsonConverter jsonConverter in converters)
			{
				JsonSettings.Converters.Add(jsonConverter);
			}
		}
	}
}
