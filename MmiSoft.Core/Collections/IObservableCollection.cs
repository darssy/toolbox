using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MmiSoft.Core.Collections;

public interface IObservableCollection<T> : IList<T>, IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
	new int Count { get; }
	new T this[int index] { get; set; }
}
