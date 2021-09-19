using System;
using MmiSoft.Core.Math.Units;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MmiSoft.Core.Json
{
	public class UnitConverter : JsonConverter<UnitBase>
	{
		public override void WriteJson(JsonWriter writer, UnitBase value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, new JValue(value.ToString()));
		}

		public override UnitBase ReadJson(JsonReader reader, Type objectType, UnitBase existingValue, bool hasExistingValue,
			JsonSerializer serializer)
		{
			string unitAsText = JToken.Load(reader).Value<string>();
			UnitBase unit = unitAsText?.ParseUnit();
			if (unit?.GetType() != objectType && !objectType.IsInstanceOfType(unit))
			{
				throw new ArgumentException("Unit mismatch");
			}
			return unit;
		}
	}
}
