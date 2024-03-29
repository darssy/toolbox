﻿using System;
using System.Net;
using Newtonsoft.Json;

namespace MmiSoft.Core.Json
{
	internal class IpAddressJsonConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, ((IPAddress)value).ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return IPAddress.Parse(serializer.Deserialize<string>(reader) ?? "");
		}

		public override bool CanConvert(Type objectType)
		{
#if NET5_0_OR_GREATER
			return objectType.IsAssignableTo(typeof(IPAddress));
#else
			return objectType == typeof(IPAddress);
#endif
		}
	}
}
