using System;

namespace MmiSoft.Core
{
	public interface IEventLogProvider
	{
		event EventHandler<ValueEventArgs<LogEntry>> LogEntryCreated;
	}

	/// <summary>
	/// "Better" version of IEventLogProvider that isn't using ValueEventArgs
	/// </summary>
	public interface IEventProducer
	{
		event EventHandler<LogEntry> LogEntryCreated;
	}
}
