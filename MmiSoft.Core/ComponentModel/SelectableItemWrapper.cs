using System;

namespace MmiSoft.Core.ComponentModel
{
	/// <summary>
	/// Class for wrapping IDesignatedItems where SelectableItemBase cannot be directly inherited eg when the item is a Control.
	/// </summary>
	[Serializable]
	public class SelectableItemWrapper : SelectableItemBase
	{
		protected readonly IDesignatedItem wrapped;

		[field: NonSerialized]
		public override event EventHandler SelectedChanged;

		public SelectableItemWrapper(IDesignatedItem wrapped)
			:base(wrapped?.Designation ?? throw new ArgumentNullException(nameof(wrapped)))
		{
			this.wrapped = wrapped;
		}

		/// <summary>
		/// Triggers the SelectedChanged event. The sender is not "this" but the object wrapped in this wrapper.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnSelectedChanged(EventArgs e)
		{
			SelectedChanged?.Invoke(wrapped, e);
		}
	}
}
