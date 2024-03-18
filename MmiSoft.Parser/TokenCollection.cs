using System.Collections;
using System.Diagnostics;
using MmiSoft.Core;

namespace MmiSoft.Parser;

[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof (CollectionDebugView<>))]
public class TokenCollection<T> : IReadOnlyList<Token<T>>
{
	private Token<T>[] reversedTokens;

	public TokenCollection(ICollection<Token<T>> tokens)
	{
		reversedTokens = tokens?.Reverse().ToArray() ?? throw new ArgumentNullException(nameof(tokens));
		Count = reversedTokens.Length;
	}

	public Token<T> this[Index idx] => idx.IsFromEnd ? reversedTokens[idx.Value - 1] : reversedTokens[^idx.Value];

	public Token<T> Peek(int i = 0)
	{
		if (i < 0) throw new IndexOutOfRangeException("Negative index not allowed");
		if (i >= reversedTokens.Length) throw new IndexOutOfRangeException("Index must less than count");
		return reversedTokens[^(i + 1)];
	}

	public Token<T> Pop(int i = 0)
	{
		Token<T> result = Peek(i);
		reversedTokens[^(i + 1)] = default;
		Count--;
		return result;
	}

	public int Count { get; private set; }

	public IEnumerator<Token<T>> GetEnumerator() => new ReverseEnumerator(reversedTokens);

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public Token<T> this[int index] => reversedTokens[^(index + 1)];

	private class ReverseEnumerator : IEnumerator<Token<T>>
	{
		private Token<T>[] reversedTokens;
		private int index;
		private int capturedLength;

		public ReverseEnumerator(Token<T>[] reversedTokens)
		{
			this.reversedTokens = reversedTokens ?? throw new ArgumentNullException(nameof(reversedTokens));
			capturedLength = this.reversedTokens.Length;
		}

		public void Dispose()
		{
			
		}

		public bool MoveNext()
		{
			if (index >= reversedTokens.Length) return false;
			if (capturedLength != reversedTokens.Length)
			{
				throw new Exception("Collection modified during iteration");
			}
			index++;
			return true;
		}

		public void Reset()
		{
			index = 0;
			capturedLength = reversedTokens.Length;
		}

		public Token<T> Current => reversedTokens[^(index + 1)];

		object IEnumerator.Current => Current;
	}
}

internal sealed class CollectionDebugView<T>
{
	private IReadOnlyCollection<Token<T>> collection;

	public CollectionDebugView(IReadOnlyCollection<Token<T>> collection)
	{
		this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
	}

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	public Token<T>[] Items
	{
		get
		{
			Token<T>[] array = new Token<T>[collection.Count];
			int ctr = 0;
			foreach (Token<T> t in collection)
			{
				array[ctr++] = t;
			}
			return array;
		}
	}
}


public static class TokenCollectionExtensions
{
	public static List<TokenCollection<T>> SplitOnTokenType<T> (this ICollection<Token<T>> tokens, T tokenType,
	                                                            bool include = false, bool removeEmpty = true)
	{
		int count = 0;
		foreach (Token<T> t in tokens)
		{
			if (EqualityComparer<T>.Default.Equals(t.Type, tokenType)) count++;
		}

		if (count == 0) return new List<TokenCollection<T>> { new(tokens) };

		List<TokenCollection<T>> result = new(count + 1);
		List<Token<T>> last = new();
		foreach (Token<T> t in tokens)
		{
			if (EqualityComparer<T>.Default.Equals(t.Type, tokenType))
			{
				if (include) last.Add(t);
				if (last.Count > 0 || !removeEmpty)
				{
					result.Add(new TokenCollection<T>(last));
					last.Clear();
				}
			}
			else last.Add(t);
		}

		if (last.Count > 0 || !removeEmpty)
		{
			result.Add(new TokenCollection<T>(last));
		}

		return result;
	}
}
