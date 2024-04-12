using System;

namespace MmiSoft.Core.ComponentModel
{
	public class EditableObjectHolder<T> : IEditableObjectWrapper where T : class, new()
	{
		private T memento;
		private Func<T, T> copyAction;

		public EditableObjectHolder(T o, Func<T, T> copyAction = null)
		{
			Object = o;
			memento = null;
			this.copyAction = copyAction;
		}

		public T Object { get; }

		public bool IsEditing { get; private set; }

		public void BeginEdit()
		{
			if (IsEditing) return;
			IsEditing = true;
			if (Object is IEditableObjectWithEvents et) et.BeginEdit();
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
			if (Object is IEditableObjectWithEvents et) et.CancelEdit();
		}

		public void EndEdit()
		{
			if (!IsEditing) return;
			memento = null;
			IsEditing = false;
			if (Object is IEditableObjectWithEvents et) et.EndEdit();
		}

		object IEditableObjectWrapper.Object => Object;
	}
}
