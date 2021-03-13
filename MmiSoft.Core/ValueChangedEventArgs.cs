using System;

namespace MmiSoft.Core
{
	public class ValueChangedEventArgs<T> : EventArgs
	{
		private T oldValue;
		private T newValue;

		public ValueChangedEventArgs(T newValue, T oldValue)
		{
			this.newValue = newValue;
			this.oldValue = oldValue;
		}

		public T NewValue => newValue;

		public T OldValue => oldValue;
	}
}
