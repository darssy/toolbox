using System;
using System.ComponentModel;
using System.Globalization;

namespace MmiSoft.Core.ComponentModel
{
	public class PercentTypeConverter : TypeConverter
	{
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string percentStr = value?.ToString();
			if (percentStr == null)
			{
				throw new ArgumentException("Unable to parse 'null' as percent");
			}
			return Percent.Parse(percentStr, CultureInfo.CurrentCulture);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return destinationType == typeof(string)
				? ((Percent)value).GetDisplayValue()
				: base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
