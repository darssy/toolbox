//---------------------------------------------------------------------------
// Copied and modified from:
// https://github.com/microsoft/referencesource/blob/master/System/compmod/system/collections/objectmodel/observablecollection.cs
//
// File under MIT License according to the license section of the README file:
// https://github.com/microsoft/referencesource/blob/master/README.md
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MmiSoft.Core.Collections;

/// <summary>
/// <para>
/// Implementation of a dynamic data collection based on generic Collection&lt;T&gt;,
/// implementing INotifyCollectionChanged to notify listeners
/// when items get added, removed or the whole list is refreshed.
/// </para>
/// <para>
/// The difference from the original ObservableCollection is that this one behaves like a binding list and changes the underlying
/// collection. The original ObservableCollection is making a copy of the underlying list.
/// </para>
/// </summary>
#if !FEATURE_NETCORE
[Serializable]
#endif
public class ObservableCollectionRef<T> : Collection<T>, IObservableCollection<T>
{
	//------------------------------------------------------
	//
	//  Constructors
	//
	//------------------------------------------------------

	#region Constructors

	/// <summary>
	/// Initializes a new instance of ObservableCollectionRef that is empty and has default initial capacity.
	/// </summary>
	public ObservableCollectionRef()
	{
	}

	/// <summary>
	/// Initializes a new instance of the ObservableCollectionRef class
	/// and reflects all changes to the underlying list 
	/// </summary>
	/// <param name="list">The list whose elements are copied to the new list.</param>
	/// <remarks>
	/// A reference of the list is kept in order to reflect all changes performed through the observable collection to the
	/// underlying list. This can be useful in cases where you want to modify a list, publish the modification event but you can't 
	/// change the implementation of the underlying list. Note also that changes happening to the original list outside this collection
	/// won't be notified and won't be monitored. It's strongly advised to perform all your modifications through the collection
	/// and not modify the underlying list directly.
	/// </remarks>
	/// <exception cref="ArgumentNullException"> list is a null reference </exception>
	public ObservableCollectionRef(IList<T> list) : base(list)
	{
	}

	/// <summary>
	/// Initializes a new instance of the ObservableCollectionRef class that contains
	/// elements copied from the specified collection and has sufficient capacity
	/// to accommodate the number of elements copied.
	/// </summary>
	/// <param name="collection">The collection whose elements are copied to the new list.</param>
	/// <remarks>
	/// The elements are copied onto the ObservableCollectionRef in the
	/// same order they are read by the enumerator of the collection.
	/// </remarks>
	/// <exception cref="ArgumentNullException"> collection is a null reference </exception>
	public ObservableCollectionRef(IEnumerable<T> collection) : base(new List<T>(collection))
	{
	}

	#endregion Constructors


	//------------------------------------------------------
	//
	//  Public Methods
	//
	//------------------------------------------------------

	#region Public Methods

	/// <summary>
	/// Move item at oldIndex to newIndex.
	/// </summary>
	public void Move(int oldIndex, int newIndex)
	{
		MoveItem(oldIndex, newIndex);
	}

	#endregion Public Methods


	//------------------------------------------------------
	//
	//  Public Events
	//
	//------------------------------------------------------

	#region Public Events

	//------------------------------------------------------

	#region INotifyPropertyChanged implementation

	/// <summary>
	/// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
	/// </summary>
	event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
	{
		add { PropertyChanged += value; }
		remove { PropertyChanged -= value; }
	}

	#endregion INotifyPropertyChanged implementation


	//------------------------------------------------------
	/// <summary>
	/// Occurs when the collection changes, either by adding or removing an item.
	/// </summary>
	/// <remarks>
	/// see <seealso cref="INotifyCollectionChanged"/>
	/// </remarks>
#if !FEATURE_NETCORE
	[field: NonSerialized]
#endif
	public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

	#endregion Public Events


	//------------------------------------------------------
	//
	//  Protected Methods
	//
	//------------------------------------------------------

	#region Protected Methods

	/// <summary>
	/// Called by base class Collection&lt;T&gt; when the list is being cleared;
	/// raises a CollectionChanged event to any listeners.
	/// </summary>
	protected override void ClearItems()
	{
		CheckReentrancy();
		base.ClearItems();
		OnPropertyChanged(CountString);
		OnPropertyChanged(IndexerName);
		OnCollectionReset();
	}

	/// <summary>
	/// Called by base class Collection&lt;T&gt; when an item is removed from list;
	/// raises a CollectionChanged event to any listeners.
	/// </summary>
	protected override void RemoveItem(int index)
	{
		CheckReentrancy();
		T removedItem = this[index];

		base.RemoveItem(index);

		OnPropertyChanged(CountString);
		OnPropertyChanged(IndexerName);
		OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);
	}

	/// <summary>
	/// Called by base class Collection&lt;T&gt; when an item is added to list;
	/// raises a CollectionChanged event to any listeners.
	/// </summary>
	protected override void InsertItem(int index, T item)
	{
		CheckReentrancy();
		base.InsertItem(index, item);

		OnPropertyChanged(CountString);
		OnPropertyChanged(IndexerName);
		OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
	}

	/// <summary>
	/// Called by base class Collection&lt;T&gt; when an item is set in list;
	/// raises a CollectionChanged event to any listeners.
	/// </summary>
	protected override void SetItem(int index, T item)
	{
		CheckReentrancy();
		T originalItem = this[index];
		base.SetItem(index, item);

		OnPropertyChanged(IndexerName);
		OnCollectionChanged(NotifyCollectionChangedAction.Replace, originalItem, item, index);
	}

	/// <summary>
	/// Called by base class ObservableCollectionRef&lt;T&gt; when an item is to be moved within the list;
	/// raises a CollectionChanged event to any listeners.
	/// </summary>
	protected virtual void MoveItem(int oldIndex, int newIndex)
	{
		CheckReentrancy();

		T removedItem = this[oldIndex];

		base.RemoveItem(oldIndex);
		base.InsertItem(newIndex, removedItem);

		OnPropertyChanged(IndexerName);
		OnCollectionChanged(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex);
	}


	/// <summary>
	/// Raises a PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
	/// </summary>
	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		PropertyChanged?.Invoke(this, e);
	}

	/// <summary>
	/// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
	/// </summary>
#if !FEATURE_NETCORE
	[field: NonSerialized]
#endif
	protected virtual event PropertyChangedEventHandler PropertyChanged;

	/// <summary>
	/// Raise CollectionChanged event to any listeners.
	/// Properties/methods modifying this ObservableCollectionRef will raise
	/// a collection changed event through this virtual method.
	/// </summary>
	/// <remarks>
	/// When overriding this method, either call its base implementation
	/// or call <see cref="BlockReentrancy"/> to guard against reentrant collection changes.
	/// </remarks>
	protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	{
		using (BlockReentrancy())
		{
			CollectionChanged?.Invoke(this, e);
		}
	}

	/// <summary>
	/// Disallow reentrant attempts to change this collection. E.g. a event handler
	/// of the CollectionChanged event is not allowed to make changes to this collection.
	/// </summary>
	/// <remarks>
	/// typical usage is to wrap e.g. a OnCollectionChanged call with a using() scope:
	/// <code>
	///         using (BlockReentrancy())
	///         {
	///             CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
	///         }
	/// </code>
	/// </remarks>
	protected IDisposable BlockReentrancy()
	{
		monitor.Enter();
		return monitor;
	}

	/// <summary> Check and assert for reentrant attempts to change this collection. </summary>
	/// <exception cref="InvalidOperationException"> raised when changing the collection
	/// while another collection change is still being notified to other listeners </exception>
	protected void CheckReentrancy()
	{
		if (monitor.Busy)
		{
			// we can allow changes if there's only one listener - the problem
			// only arises if reentrant changes make the original event args
			// invalid for later listeners.  This keeps existing code working
			// (e.g. Selector.SelectedItems).
			if (CollectionChanged != null && CollectionChanged.GetInvocationList().Length > 1)
			{
				throw new InvalidOperationException(
					$"{nameof(CollectionChanged)} listeners are  not allowed to modify the Collection");
			}
		}
	}

	#endregion Protected Methods


	//------------------------------------------------------
	//
	//  Private Methods
	//
	//------------------------------------------------------

	#region Private Methods

	/// <summary>
	/// Helper to raise a PropertyChanged event  />).
	/// </summary>
	private void OnPropertyChanged(string propertyName)
	{
		OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
	}

	/// <summary>
	/// Helper to raise CollectionChanged event to any listeners
	/// </summary>
	private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
	{
		OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
	}

	/// <summary>
	/// Helper to raise CollectionChanged event to any listeners
	/// </summary>
	private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
	{
		OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
	}

	/// <summary>
	/// Helper to raise CollectionChanged event to any listeners
	/// </summary>
	private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
	{
		OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
	}

	/// <summary>
	/// Helper to raise CollectionChanged event with action == Reset to any listeners
	/// </summary>
	private void OnCollectionReset()
	{
		OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
	}

	#endregion Private Methods

	//------------------------------------------------------
	//
	//  Private Types
	//
	//------------------------------------------------------

	#region Private Types

	// this class helps prevent reentrant calls
#if !FEATURE_NETCORE
	[Serializable]
#endif
	private class SimpleMonitor : IDisposable
	{
		public void Enter()
		{
			++busyCount;
		}

		public void Dispose()
		{
			--busyCount;
		}

		public bool Busy => busyCount > 0;

		private int busyCount;
	}

	#endregion Private Types

	//------------------------------------------------------
	//
	//  Private Fields
	//
	//------------------------------------------------------

	#region Private Fields

	private const string CountString = "Count";

	// This must agree with Binding.IndexerName.  It is declared separately
	// here so as to avoid a dependency on PresentationFramework.dll.
	private const string IndexerName = "Item[]";

	private SimpleMonitor monitor = new();

	#endregion Private Fields

}
