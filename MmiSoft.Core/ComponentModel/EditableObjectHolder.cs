using System;

namespace MmiSoft.Core.ComponentModel
{
	public class EditableObjectHolder<T> : IEditableObjectWrapper where T : class, new()
	{
		private T original;
		private Func<T, T> copyAction;
		private bool isEditing;

		public EditableObjectHolder(T o, Func<T, T> copyAction = null)
		{
			Object = o;
			original = null;
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
				original = new T();
				Object.Copy(original);
			}
			else
			{
				original = copyAction.Invoke(Object);
			}
		}

		public void CancelEdit()
		{
			if (!IsEditing) return;
			original.Copy(Object);
			original = null;
			IsEditing = false;
		}

		public void EndEdit()
		{
			original = null;
			IsEditing = false;
		}

		object IEditableObjectWrapper.Object => Object;
	}
}
