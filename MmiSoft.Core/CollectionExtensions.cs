using System;
using System.Collections.Generic;
using System.Linq;

namespace MmiSoft.Core
{
	public static class CollectionExtensions
	{
		public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest) {

			first = list.Count > 0 ? list[0] : default;
			rest = list.Skip(1).ToList();
		}

		public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest) {
			first = list.Count > 0 ? list[0] : default;
			second = list.Count > 1 ? list[1] : default;
			rest = list.Skip(2).ToList();
		}

		public static V GetOrCreate<K, V>(this IDictionary<K, V> dictionary, in K key, Func<V> creator)
		{
			if (dictionary.TryGetValue(key, out V value)) return value;

			value = creator.Invoke();
			dictionary[key] = value;
			return value;
		}
	}
}
