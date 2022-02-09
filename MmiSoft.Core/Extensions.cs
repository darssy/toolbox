using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using MmiSoft.Core.Configuration;
using Newtonsoft.Json;
using SysMath = System.Math;

namespace MmiSoft.Core
{
	public static class Extensions
	{
		private static readonly Dictionary<Type, List<JsonConverter>> ConverterCache = new Dictionary<Type, List<JsonConverter>>();
		private static readonly BrowsableAttribute BrowsableFalse = new BrowsableAttribute(false);

		private static List<Type> ignoredTypes = new List<Type>
		{
			typeof(string),
			typeof(IPAddress)
		};

		public static Color Negate(in this Color c)
		{
			return Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
		}

		public static Color Darken(in this Color c)
		{
			return Color.FromArgb((int) SysMath.Round(c.R * 0.5),
				(int) SysMath.Round(c.G * 0.5),
				(int) SysMath.Round(c.B * 0.5));
		}

		public static Color Lighten(in this Color c)
		{
			int r = (int)SysMath.Round(c.R * 1.45);
			int g = (int)SysMath.Round(c.G * 1.45);
			int b = (int)SysMath.Round(c.B * 1.45);
			if (r > 255) r = 255;
			if (g > 255) g = 255;
			if (b > 255) b = 255;
			return Color.FromArgb(r, g, b);
		}

		public static UInt64 DoubleToLongBits(this double d)
		{
			BitArray a = new BitArray(BitConverter.GetBytes(d));
			UInt64 longBits = 0;
			for (int i = 0; i < a.Length; i++)
			{
				longBits += Convert.ToUInt16(a[i]) * ((UInt64)SysMath.Pow(2, i));
			}
			return longBits;
		}

		public static IEnumerable<T> OfExactType<T>(this IEnumerable collection)
		{
			foreach (object item in collection)
			{
				if (item.GetType() == typeof(T)) yield return (T)item;
			}
		}

		internal static IList<JsonConverter> GetJsonConverters(this Type type)
		{
			return ConverterCache.GetOrCreate(type, () => type
				.GetCustomAttributes(true)
				.OfType<JsonConverterBaseAttribute>()
				.Select(attr => attr.Create())
				.ToList());
		}

		public static void Copy<T>(this T from, T to)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All,
				Converters = typeof(T).GetJsonConverters()
			};

			IEnumerable<PropertyInfo> properties = typeof(T).GetProperties()
				.Where(p => p.GetGetMethod() != null && p.GetSetMethod() != null);

			foreach (PropertyInfo info in properties)
			{
				if (info.GetCustomAttributes(true).Contains(BrowsableFalse))
				{
					continue;
				}

				MethodInfo propertyGetter = info.GetGetMethod();
				object value = propertyGetter.Invoke(from, new object[] { });
				if (!info.PropertyType.IsValueType && !ignoredTypes.Contains(info.PropertyType))
				{
					string json = JsonConvert.SerializeObject(value, settings);
					value = JsonConvert.DeserializeObject(json, propertyGetter.ReturnType, settings);
				}
				info.GetSetMethod().Invoke(to, new[] { value });
			}
		}

		public static string GetAssemblyDisplayVersion(this Assembly assembly)
		{
			Version ver = assembly.GetName().Version;
			return $"{ver.Major}.{ver.Minor}";
		}
	}
}
