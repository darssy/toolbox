using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace MmiSoft.Core.ComponentModel
{
	public class NoTypeExpandableConverter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return destinationType == typeof(string)
				? value is ICollection ? "[Collection]" : ""
				: base.ConvertTo(context, culture, value, destinationType);
		}
	}

	public class NoStringExpandableConverter : NoTypeExpandableConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType != typeof(string) && base.CanConvertTo(context, destinationType);
		}
	}

#if !NET
	public class ConstrainedFontConverter : FontConverter
	{
		private static readonly List<string> Fields = new List<string>
		{
			nameof(Font.Name),
			nameof(Font.Size)
		};

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = base.GetProperties(context, value, attributes);
			for (var i = 0; i < properties.Count; i++)
			{
				if (!Fields.Contains(properties[i].Name))
				{
					properties.RemoveAt(i--);
				}
			}
			return properties;
		}
	}
#endif

}
