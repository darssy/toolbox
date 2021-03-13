using System;
using System.Threading;

namespace MmiSoft.Core
{
	public static class ReaderWriterLockSlimExtensions
	{

#if DEBUG
		private const int DefaultLockTimeout = -1;
#else
		private const int DefaultLockTimeout = 5000;
#endif

		public static R GetReadProtected<R>(this ReaderWriterLockSlim rwLock, Func<R> producer, int timeoutMillis = DefaultLockTimeout)
		{
			try
			{
				if (rwLock.TryEnterReadLock(timeoutMillis)) return producer();

				EventLogger.Warn($"Unable to take read lock within {timeoutMillis} ms");
				return default;
			}
			finally
			{
				rwLock.ExitReadLock();
			}
		}

		public static void DoReadProtected(this ReaderWriterLockSlim rwLock, Action action, int timeoutMillis = DefaultLockTimeout)
		{
			try
			{
				if (!rwLock.TryEnterReadLock(timeoutMillis))
				{
					EventLogger.Warn($"Unable to take read lock within {timeoutMillis} ms");
				}
				else action();
			}
			finally
			{
				rwLock.ExitReadLock();
			}
		}

		public static void DoWriteProtected(this ReaderWriterLockSlim rwLock, Action action, int timeoutMillis = DefaultLockTimeout)
		{
			try
			{
				if (!rwLock.TryEnterReadLock(timeoutMillis))
				{
					EventLogger.Warn($"Unable to take read lock within {timeoutMillis} ms");
				}
				else action();
			}
			finally
			{
				rwLock.ExitWriteLock();
			}
		}
	}
}
