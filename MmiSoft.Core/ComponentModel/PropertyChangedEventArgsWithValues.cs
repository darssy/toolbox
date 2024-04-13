using System.ComponentModel;
using System.Diagnostics;

namespace MmiSoft.Core.ComponentModel;

/// <summary>
/// Extension of PropertyChangedEventArgs that includes the values that changed. You will have to type check and cast the events
/// produced by PropertyChanged. This might be cumbersome but that way you maintain interoperability with the controls that rely
/// on INotifyPropertyChanged without having to implement the same event mechanism twice.
/// </summary>
[DebuggerDisplay("{PropertyName}: {ExistingValue}>{NewValue}")]
public class PropertyChangedEventArgsWithValues : PropertyChangedEventArgs
{
	private object newValue;
	private object existingValue;

	public PropertyChangedEventArgsWithValues(string propertyName, object newValue, object existingValue) : base(propertyName)
	{
		this.newValue = newValue;
		this.existingValue = existingValue;
	}

	/// <summary>
	/// The value after the change
	/// </summary>
	public object NewValue => newValue;
	
	/// <summary>
	/// The original value, before the change
	/// </summary>
	public object ExistingValue => existingValue;
}
