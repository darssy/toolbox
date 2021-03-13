using System;
using System.Runtime.Serialization;

namespace MmiSoft.Core
{
	[Serializable]
	public class ApplicationStateException : Exception
	{
		public ApplicationStateException() { }

		public ApplicationStateException(string message) : base(message) { }

		protected ApplicationStateException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}

	[Serializable]
	public class MultipleDispatchException : Exception
	{
		public MultipleDispatchException() { }

		public MultipleDispatchException(string message) : base(message) { }

		protected MultipleDispatchException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}

	[Serializable]
	public class TimerException : Exception
	{
		public TimerException() { }
		public TimerException(string message) : base(message) { }
		public TimerException(string message, Exception inner) : base(message, inner) { }
		protected TimerException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context) { }
	}
}
