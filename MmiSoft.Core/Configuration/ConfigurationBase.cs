using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MmiSoft.Core.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MmiSoft.Core.Configuration
{
	public abstract class ConfigurationBase
	{
		public event EventHandler<ValueEventArgs<string>> Error;

		protected virtual void OnError(ValueEventArgs<string> e)
		{
			Error?.Invoke(this, e);
		}

		public static T ReadConfig<T>(string fullFilePath) where T : ConfigurationBase
		{
			if (!File.Exists(fullFilePath)) throw new FileNotFoundException($"Could not locate '{fullFilePath}'");
			EventLogger.Info($"Reading configuration from {fullFilePath}");
			return JsonFileIO.Read<T>(fullFilePath);
		}

		public void WriteConfig(string fullFilePath)
		{
			JsonFileIO.Write(fullFilePath, this);
			EventLogger.Info($"Configuration saved to {fullFilePath}");
		}

		public void ReadConfig(string fullFilePath, bool withTracing = false)
		{
			string jsonText;
			using (StreamReader fileStream = FileReader.CreateStream(fullFilePath))
			{
				jsonText = fileStream.ReadToEnd();
			}

			ITraceWriter traceWriter =
				withTracing ? new DiagnosticsTraceWriter { LevelFilter = TraceLevel.Verbose } : null;
			try
			{
				JsonConvert.PopulateObject(jsonText, this,
					JsonFileIO.CreateSettingsWithConvertersForClass(GetType(), traceWriter));
			}
			catch (JsonSerializationException e)
			{
				OnError(new ValueEventArgs<string>(e.Message));
			}
		}
	}
}
