using System;

namespace MmiSoft.Core.ComponentModel
{
	/// <summary>
	/// Interface providing functionality for selecting one or more similar items
	/// </summary>
	public interface ISelectableItem : IDesignatedItem
	{
		/// <summary>
		/// Sets or gets the selection status of the object
		/// </summary>
		bool Selected { get; set; }

		/// <summary>
		/// Occurs when the Selected property value changes.
		/// </summary>
		event EventHandler SelectedChanged;
	}
}
