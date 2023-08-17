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

		public static IEnumerable<T> OfExactType<T>(this IEnumerable collection)
		{
			foreach (object item in collection)
			{
				if (item.GetType() == typeof(T)) yield return (T)item;
			}
		}

		internal static IList<JsonConverter> GetJsonConverters(this Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
			{
				type = type.GetGenericArguments()[0];
			}
			else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
			{
				type = type.GetGenericArguments()[1];
			}
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
