using System;

namespace MmiSoft.Core
{
	public delegate void ValueEventHandler<T>(object sender, ValueEventArgs<T> e);

	/// <summary>
	/// There was a time that you <i>had</i> to extend EventArgs. According to Microsoft, this isn't necessary anymore
	/// https://docs.microsoft.com/en-us/dotnet/csharp/modern-events (I hope that this stays alive for some time) and
	/// you can pass whatever type you like in lieu of a derivative of EventArgs.
	/// <para>
	/// This class is from that time. Use it if you need to pass a value to the event subscribers and you need to extend
	/// EventArgs at the same time (for any reason).
	/// </para>
	/// </summary>
	/// <typeparam name="T">The type of the value to pass to the subscriber</typeparam>
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
