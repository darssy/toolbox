using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MmiSoft.Core.Collections
{
	/// <summary>
	/// Wraps a StringCollection into an IList&lt;string&gt; implementation. Modifications on the adapter are reflected
	/// on the underlying collection
	/// </summary>
	public class GenericStringCollectionAdapter : IList<string>
	{
		private StringCollection stringCollection;

		public GenericStringCollectionAdapter(StringCollection stringCollection)
		{
			this.stringCollection = stringCollection;
		}

		public IEnumerator<string> GetEnumerator() => new GenericStringEnumerator(stringCollection.GetEnumerator());

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(string item) => stringCollection.Add(item);

		public void Clear() => stringCollection.Clear();

		public bool Contains(string item) => stringCollection.Contains(item);

		public void CopyTo(string[] array, int arrayIndex)
		{
			stringCollection.CopyTo(array, arrayIndex);
		}

		public bool Remove(string item)
		{
			stringCollection.Remove(item);
			return true;
		}

		public int Count => stringCollection.Count;

		public bool IsReadOnly => stringCollection.IsReadOnly;

		public int IndexOf(string item) => stringCollection.IndexOf(item);

		public void Insert(int index, string item) => stringCollection.Insert(index, item);

		public void RemoveAt(int index) => stringCollection.RemoveAt(index);

		public string this[int index]
		{
			get => stringCollection[index];
			set => stringCollection[index] = value;
		}

		private class GenericStringEnumerator : IEnumerator<string>
		{
			private StringEnumerator stringEnumerator;

			public GenericStringEnumerator(StringEnumerator stringEnumerator)
			{
				this.stringEnumerator = stringEnumerator;
			}

			public void Dispose() {}

			public bool MoveNext() => stringEnumerator.MoveNext();

			public void Reset() => stringEnumerator.Reset();

			public string Current => stringEnumerator.Current;

			object IEnumerator.Current => Current;
		}
	}
}
