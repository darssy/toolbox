using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MmiSoft.Core.Json
{
	public class PercentJsonConverter : JsonConverter<Percent>
	{
		public override void WriteJson(JsonWriter writer, Percent value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, new JValue(value.GetDisplayValue()));
		}

		public override Percent ReadJson(JsonReader reader, Type objectType, Percent existingValue, bool hasExistingValue,
			JsonSerializer serializer)
		{
			string unitAsText = JToken.Load(reader).Value<string>();
			Percent unit = Percent.Parse(unitAsText, CultureInfo.CurrentCulture);
			return unit;
		}
	}
}
