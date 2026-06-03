using System.Collections.Specialized;
using NUnit.Framework;

namespace MmiSoft.Core.Collections;

[TestFixture]
public class GenericStringCollectionAdapterTest
{
	private static GenericStringCollectionAdapter Adapter(params string[] items)
	{
		StringCollection collection = new StringCollection();
		collection.AddRange(items);
		return new GenericStringCollectionAdapter(collection);
	}

	[Test]
	public void Remove_ItemPresent_RemovesItAndReturnsTrue()
	{
		GenericStringCollectionAdapter adapter = Adapter("a", "b", "c");

		bool removed = adapter.Remove("b");

		Assert.That(removed, Is.True);
		Assert.That(adapter, Is.EqualTo(new[] { "a", "c" }));
	}

	[Test]
	public void Remove_ItemAbsent_ReturnsFalseAndLeavesCollectionUnchanged()
	{
		GenericStringCollectionAdapter adapter = Adapter("a", "b");

		bool removed = adapter.Remove("x");

		Assert.That(removed, Is.False);
		Assert.That(adapter.Count, Is.EqualTo(2));
	}
}
