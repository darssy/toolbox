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

		/// <summary>
		/// Returns the value of the key if it exists in the dictionary. Otherwise it creates the value using the creator
		/// function, stores the value in the dictionary and returns the newly created value.
		/// </summary>
		/// <remarks>
		/// This overload is more straightforward syntactically, but if the creator captures variables from the call site
		/// you might find yourself with extra allocations (and garbage collections later on) that might impact performance
		/// if this method is called in a hotspot. Use this function if readability is more important than performance.
		/// Otherwise consider using one of the other 2 overloads.
		/// </remarks>
		/// <param name="dictionary">The dictionary to operate on</param>
		/// <param name="key">The key to search with</param>
		/// <param name="creator">A function that will create the new value if it doesn't exist</param>
		/// <typeparam name="K">Type of the key</typeparam>
		/// <typeparam name="V">Type of the value</typeparam>
		/// <returns>The value corresponding to the key, either existing or created by the creator function</returns>
		public static V GetOrCreate<K, V>(this IDictionary<K, V> dictionary, in K key, Func<V> creator)
		{
			if (dictionary.TryGetValue(key, out V value)) return value;

			value = creator.Invoke();
			dictionary[key] = value;
			return value;
		}

		/// <summary>
		/// Returns the value of the key if it exists in the dictionary. Otherwise it creates the value using the creator
		/// function and the seed, stores the value in the dictionary and returns the newly created value. If the key can
		/// be the seed, then you can use the 2 argument overload.
		/// </summary>
		/// <remarks>
		/// This overload with a seed might seem redundant as the creator function or lambda can capture the seed at the
		/// call site. But this comes at a cost as lambdas capturing locals or this are allocating a Delegate each time
		/// they are called. This trick although not syntactically 'neat' saves us from this extra allocation -and its
		/// respective garbage collection later on.
		/// </remarks>
		/// <param name="dictionary">The dictionary to operate on</param>
		/// <param name="key">The key to search with</param>
		/// <param name="seed">The seed to use for the creation of the value if doesn't exist</param>
		/// <param name="creator">A function that will take the seed as argument and create a new value with it</param>
		/// <typeparam name="K">Type of the key</typeparam>
		/// <typeparam name="V">Type of the value</typeparam>
		/// <typeparam name="S">Type of the seed (in case it's not the same as the key)</typeparam>
		/// <returns>The value corresponding to the key, either existing or created by the creator function</returns>
		public static V GetOrCreate<K, V, S>(this IDictionary<K, V> dictionary, in K key, in S seed, Func<S, V> creator)
		{
			if (dictionary.TryGetValue(key, out V value)) return value;

			value = creator.Invoke(seed);
			dictionary[key] = value;
			return value;
		}

		/// <summary>
		/// Returns the value of the key if it exists in the dictionary. Otherwise it creates the value using the creator
		/// function and the key as a seed, stores the value in the dictionary and returns the newly created value. If the
		/// key can't serve as be the seed, then you can use the 3 argument overload.
		/// </summary>
		/// <remarks>
		/// This overload with a creator function taking the key as argument might seem redundant as the creator function
		/// or lambda can capture the key at the call site. But this comes at a cost as lambdas capturing locals or this
		/// are allocating a Delegate each time they are called. This trick, although not syntactically 'neat', saves us
		/// from this extra allocation -and its respective garbage collection later on.
		/// </remarks>
		/// <param name="dictionary">The dictionary to operate on</param>
		/// <param name="key">The key to search with</param>
		/// <param name="creator">A function that will take the key as argument and create a new value with it</param>
		/// <typeparam name="K">Type of the key</typeparam>
		/// <typeparam name="V">Type of the value</typeparam>
		/// <returns>The value corresponding to the key, either existing or created by the creator function</returns>
		public static V GetOrCreate<K, V>(this IDictionary<K, V> dictionary, in K key, Func<K, V> creator)
		{
			if (dictionary.TryGetValue(key, out V value)) return value;

			value = creator.Invoke(key);
			dictionary[key] = value;
			return value;
		}
	}
}
