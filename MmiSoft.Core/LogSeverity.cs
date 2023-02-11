namespace MmiSoft.Core
{
	/// <summary>
	/// A log severity enumeration containing the most common levels. For more information you should consult the
	/// documentation of the corresponding levels of the library that is wrapped in <see cref="Logging.ILogWrapper"/> 
	/// </summary>
	public enum LogSeverity : byte
	{
		/// <summary>
		/// Usually after that the application must exit
		/// </summary>
		Fatal = 0,

		/// <summary>
		/// An unexpected behavior but can go on with that
		/// </summary>
		Error = 1,

		/// <summary>
		/// Not necessarily an error but definitely something out of order that the user must know of
		/// </summary>
		Warning = 2,
		
		/// <summary>
		/// An "ordinary" and default log entry. The "FYI" of logging -hence the Info part. 
		/// </summary>
		Info = 3,
		
		/// <summary>
		/// Used to print internal class or method states for debugging purposes
		/// </summary>
		Debug = 4,
		
		/// <summary>
		/// Same purpose as <see cref="Debug"/> but for more fine grained situations where you need to <i>trace</i> an
		/// algorithm a method execution etc.
		/// </summary>
		Trace = 5
	}
}
