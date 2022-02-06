using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MmiSoft.Core.Json
{
	public class PercentJsonConverter : JsonConverter<Percent>
	{
		private CultureInfo culture;

		public PercentJsonConverter(CultureInfo culture)
		{
			this.culture = culture;
		}

		public override void WriteJson(JsonWriter writer, Percent value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, new JValue(value.GetDisplayValue()));
		}

		public override Percent ReadJson(JsonReader reader, Type objectType, Percent existingValue, bool hasExistingValue,
			JsonSerializer serializer)
		{
			string unitAsText = JToken.Load(reader).Value<string>();
			Percent value = Percent.Parse(unitAsText, culture ?? CultureInfo.CurrentCulture);
			return value;
		}
	}
}
