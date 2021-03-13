using System;

namespace MmiSoft.Core
{
	public delegate void ValueEventHandler<T>(object sender, ValueEventArgs<T> e);

	[Serializable]
	public class ValueEventArgs<T> : EventArgs
	{
		private T value;

		public ValueEventArgs(T value)
		{
			this.value = value;
		}

		public T Value => value;
	}
}
