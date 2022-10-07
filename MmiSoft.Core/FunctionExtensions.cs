using System;

namespace MmiSoft.Core
{
	public static class FunctionExtensions
	{
		/// <summary>
		/// Returns a new function that will evaluate to true if either of the 2 argument functions will be true
		/// </summary>
		/// <param name="first">First function to evaluate</param>
		/// <param name="second">Second function to evaluate</param>
		/// <returns>True if either <c>first</c> or <c>second</c> invocations results to true</returns>
		/// <exception cref="NullReferenceException">If either of the 2 functions is null (actually you can get away with
		/// it if <c>second</c> is null and <c>first</c> yields true)</exception>
		public static Func<T, bool> Or<T>(this Func<T, bool> first, Func<T, bool> second)
		{
			return t => first.Invoke(t) || second.Invoke(t);
		}

		/// <summary>
		/// Returns a new function that will evaluate to true if both of the 2 argument functions will be true.
		/// </summary>
		/// <param name="first">First function to evaluate</param>
		/// <param name="second">Second function to evaluate</param>
		/// <returns>True if both <c>first</c> and <c>second</c> invocations results to true</returns>
		/// <exception cref="NullReferenceException">If either of the 2 functions is null</exception>
		public static Func<T, bool> And<T>(this Func<T, bool> first, Func<T, bool> second)
		{
			return t => first.Invoke(t) && second.Invoke(t);
		}

		/// <summary>
		/// Convenience method to replace a <c>null Func&lt;T, bool&gt;</c> with a function returning always <c>true</c>
		/// </summary>
		/// <param name="func">The function to evaluate</param>
		/// <returns>A function returning always <c>true</c> if <c>func</c> is null or the <c>func</c> otherwise</returns>
		public static Func<T, bool> TrueIfNull<T>(this Func<T, bool> func) => func ?? (t => true);

		/// <summary>
		/// Convenience method to replace a <c>null Func&lt;T, bool&gt;</c> with a function returning always <c>false</c>
		/// </summary>
		/// <param name="func">The function to evaluate</param>
		/// <returns>A function returning always <c>false</c> if <c>func</c> is null or the <c>func</c> otherwise</returns>
		public static Func<T, bool> FalseIfNull<T>(this Func<T, bool> func) => func ?? (t => false);
	}

	public static class ActionExtensions
	{
		public static Action<T> Then<T>(this Action<T> first, Action<T> second)
		{
			return t =>
			{
				first.Invoke(t);
				second.Invoke(t);
			};
		}
		
		public static Action Then(this Action first, Action second)
		{
			return () =>
			{
				first.Invoke();
				second.Invoke();
			};
		}
	}
}
