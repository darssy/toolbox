using System;

namespace MmiSoft.Core.ComponentModel
{
	/// <summary>
	/// Default implementation of ISelectableItem for types that can afford deriving directly from SelectableItemBase
	/// </summary>
	[Serializable]
	public class SelectableItemBase : ISelectableItem
	{
		protected string designation;
		private bool selected;

        [field:NonSerialized]
		public virtual event EventHandler SelectedChanged;

		public SelectableItemBase(string designation)
		{
			this.designation = designation;
		}

		public virtual string Designation => designation;

		public virtual bool Selected
		{
			get => selected;
			set
			{
				if (selected == value) return;
				selected = value;
				OnSelectedChanged(EventArgs.Empty);
			}
		}

		public override string ToString()
		{
			return Designation;
		}

		protected virtual void OnSelectedChanged(EventArgs e)
		{
			SelectedChanged?.Invoke(this, e);
		}
	}
}
