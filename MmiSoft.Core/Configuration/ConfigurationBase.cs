using System;
using System.IO;
using MmiSoft.Core.IO;
using MmiSoft.Core.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MmiSoft.Core.Configuration
{
	public abstract class ConfigurationBase
	{
		protected static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
		{
			Formatting = Formatting.Indented,
			ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
			DefaultValueHandling = DefaultValueHandling.Include
		};

		static ConfigurationBase()
		{
			JsonSettings.Converters.Add(new UnitConverter());
			JsonSettings.Converters.Add(new PercentJsonConverter());
			JsonSettings.Converters.Add(new IpAddressJsonConverter());
			JsonSettings.Converters.Add(new StringEnumConverter());
		}

		public event EventHandler<ValueEventArgs<string>> Error;

		protected virtual void OnError(ValueEventArgs<string> e)
		{
			Error?.Invoke(this, e);
		}

		/// <summary>
		/// If ConfigurationBase is subclassed in many l
		/// </summary>
		/// <param name="fullFilePath"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		/// <exception cref="FileNotFoundException"></exception>
		public static T ReadConfig<T>(string fullFilePath) where T : ConfigurationBase
		{
			if (!File.Exists(fullFilePath)) throw new FileNotFoundException($"Could not locate '{fullFilePath}'");
			EventLogger.Info($"Reading configuration from {fullFilePath}");
			return FileReader.ReadJson<T>(fullFilePath, JsonSettings);
		}

		public void WriteConfig(string fullFilePath)
		{
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
				JsonConvert.PopulateObject(jsonText, this, JsonSettings);
			}
			catch (JsonSerializationException e)
			{
				OnError(new ValueEventArgs<string>(e.Message));
			}
		}
	}
}
