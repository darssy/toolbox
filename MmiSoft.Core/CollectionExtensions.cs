using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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
		/// Convenience method for list[list.Count - 1]
		/// </summary>
		/// <returns>The last element of the list</returns>
		/// <exception cref="IndexOutOfRangeException">If the list is empty</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Last<T>(this IList<T> list) => list[list.Count - 1];

		/// <summary>
		/// Returns the first node matching the given condition
		/// </summary>
		/// <param name="list">The list to work on</param>
		/// <param name="condition">The function providing the condition</param>
		/// <typeparam name="T"></typeparam>
		/// <returns>The first node matching the given condition or null in case of empty list or no match</returns>
		public static LinkedListNode<T> GetNode<T>(this LinkedList<T> list, Func<T, bool> condition)
		{
			LinkedListNode<T> temp = list.First;
			while (temp != null && !condition.Invoke(temp.Value))
			{
				temp = temp.Next;
			}
			return temp;
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
