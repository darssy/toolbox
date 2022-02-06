using System;
using System.Globalization;
using MmiSoft.Core.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MmiSoft.Core.Configuration
{
	[AttributeUsage(AttributeTargets.Class)]
	public abstract class JsonConverterBaseAttribute : Attribute
	{
		public abstract JsonConverter Create();
	}

	public sealed class UnitConverterAttribute : JsonConverterBaseAttribute
	{
		private CultureInfo culture;
		public UnitConverterAttribute(string culture)
		{
			this.culture = CultureInfo.GetCultureInfo(culture);
		}

		public override JsonConverter Create() => new UnitConverter(culture);
	}

	public class EnumConverterAttribute : JsonConverterBaseAttribute
	{
		public override JsonConverter Create() => new StringEnumConverter();
	}

	public class IpAddressConverterAttribute : JsonConverterBaseAttribute
	{
		public override JsonConverter Create() => new IpAddressJsonConverter();
	}

	public class PercentConverterAttribute : JsonConverterBaseAttribute
	{
		private CultureInfo culture;

		public PercentConverterAttribute(string culture)
		{
			this.culture = CultureInfo.GetCultureInfo(culture);
		}

		public override JsonConverter Create() => new PercentJsonConverter(culture);
	}
}
