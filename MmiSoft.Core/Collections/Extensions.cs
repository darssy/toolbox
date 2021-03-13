using System;
using System.Collections.Generic;
using System.Linq;

namespace MmiSoft.Core.Collections
{
	public static class Extensions
	{
		public static V GetOrCreate<K, V>(this IDictionary<K, V> dictionary, in K key, Func<V> creator)
		{
			if (dictionary.ContainsKey(key)) return dictionary[key];

			V value = creator.Invoke();
			dictionary[key] = value;
			return value;
		}
	}
}
