using System;

namespace MmiSoft.Core
{
	public interface IEventLogProvider
	{
		event EventHandler<ValueEventArgs<LogEntry>> LogEntryCreated;
	}
}
