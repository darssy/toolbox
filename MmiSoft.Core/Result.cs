using System;
using System.Collections.Generic;

namespace MmiSoft.Core
{
	public readonly ref struct Result<R, E> where E : class
	{
		private readonly E error;
		private readonly R value;

		public Result(E error)
		{
			this.error = error ?? throw new ArgumentException("Error state can't be null -it will \"impersonate\" Success");
			value = default;
		}

		public Result(R value)
		{
			this.value = value;
			error = null;
		}

		public bool Succeeded => error == null;

		public R Value => Succeeded ? value : throw new InvalidOperationException("Result is not successful");

		public E Error => error;

		public override string ToString()
		{
			return Succeeded ? "<Success>" : error.ToString();
		}

		public static implicit operator Result<R, E>(R success) => new Result<R, E>(success);
		public static implicit operator Result<R, E>(E failure) => new Result<R, E>(failure);
	}

	/// <summary>
	/// This is a ref struct version of <see cref="SimpleResult"/> if you want to use it for cases where allocations matter.
	/// </summary>
	/// <remarks>
	/// <para>E can be
	/// of any type, struct or ref but the former is to allow enum types to be used as errors. Otherwise I don't see any
	/// reason of having a struct as an error. If you do so though, take care to make the default value of the struct
	/// (the one returned by the empty constructor plugged in by default) the one that denotes a "no error" (aka success)
	/// situation.
	/// </para>
	/// <para>
	/// In case you want to use an enum, make sure that the very first constant declared, the one that will take the value zero,
	/// to be the one denoting a "no error" situation. Otherwise the setup will fail miserably: you will try to create an
	/// error result with the first error argument and you will get an <see cref="ArgumentException"/>
	/// </para>
	/// </remarks>
	/// <typeparam name="E">The type of the error. Could be string or an enum or a class combining both etc.</typeparam>
	/// <exception cref="ArgumentException"></exception>
	public readonly ref struct Result<E>
	{
		private readonly E error;

		public Result(E error)
		{
			if (EqualityComparer<E>.Default.Equals(error, default))
			{
				string errText = error == null ? "null" : error.ToString();
				throw new ArgumentException($"Error state can't be {errText} -it will \"impersonate\" Success");
			}
			this.error = error;
		}

		/// <summary>
		/// Returns true if the result succeeded, false otherwise.
		/// </summary>
		public bool Succeeded => EqualityComparer<E>.Default.Equals(error, default);

		/// <summary>
		/// Returns the error or <c>default</c> if the result is successful
		/// </summary>
		public E Error => error;

		public override string ToString()
		{
			return Succeeded ? "<Success>" : error.ToString();
		}

		/// <summary>
		/// This implicit operator serves the purpose of reducing verbosity when typing and converting methods easily.
		/// </summary>
		/// <example>
		/// <para>
		/// Instead of writing <code>return new Result&lt;string&gt;("Some error");</code> you can go straight to
		/// <code>return "Some error";</code>
		/// </para>
		/// Default values are handled and are considered "success". So in the previous scenario
		/// the <code>return new Result&lt;string&gt;()</code> can become <code>return null</code>
		/// </example>
		/// <param name="error">The error code to wrap into a result</param>
		/// <returns>A Result with the error or a successful result if the error is <c>default</c></returns>
		public static implicit operator Result<E>(E error) =>
			EqualityComparer<E>.Default.Equals(error, default)
				? new Result<E>()
				: new Result<E>(error);
	}
}
