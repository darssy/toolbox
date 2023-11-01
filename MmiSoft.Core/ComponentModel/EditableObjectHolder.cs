using System;

namespace MmiSoft.Core.ComponentModel
{
	public class EditableObjectHolder<T> : IEditableObjectWrapper where T : class, new()
	{
		private T memento;
		private Func<T, T> copyAction;
		private bool isEditing;

		public EditableObjectHolder(T o, Func<T, T> copyAction = null)
		{
			Object = o;
			memento = null;
			this.copyAction = copyAction;
		}

		public T Object { get; }

		public bool IsEditing
		{
			get => isEditing;
			private set
			{
				isEditing = value;
				if (Object is IExternallyEditable et) et.IsEdited = IsEditing;
			}
		}

		public void BeginEdit()
		{
			if (IsEditing) return;
			IsEditing = true;
			if (copyAction == null)
			{
				memento = new T();
				Object.Copy(memento);
			}
			else
			{
				memento = copyAction.Invoke(Object);
			}
		}

		public void CancelEdit()
		{
			if (!IsEditing) return;
			memento.Copy(Object);
			memento = null;
			IsEditing = false;
		}

		public void EndEdit()
		{
			memento = null;
			IsEditing = false;
		}

		object IEditableObjectWrapper.Object => Object;
	}
}
