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
			if (!rwLock.TryEnterReadLock(timeoutMillis))
			{
				EventLogger.Warn($"Unable to take read lock within {timeoutMillis} ms");
				return default;
			}
			try
			{
				return producer();
			}
			finally
			{
				rwLock.ExitReadLock();
			}
		}

		public static void DoReadProtected(this ReaderWriterLockSlim rwLock, Action action, int timeoutMillis = DefaultLockTimeout)
		{
			if (!rwLock.TryEnterReadLock(timeoutMillis))
			{
				EventLogger.Warn($"Unable to take read lock within {timeoutMillis} ms");
				return;
			}
			try
			{
				action();
			}
			finally
			{
				rwLock.ExitReadLock();
			}
		}

		public static void DoWriteProtected(this ReaderWriterLockSlim rwLock, Action action, int timeoutMillis = DefaultLockTimeout)
		{
			if (!rwLock.TryEnterWriteLock(timeoutMillis))
			{
				EventLogger.Warn($"Unable to take write lock within {timeoutMillis} ms");
				return;
			}
			try
			{
				action();
			}
			finally
			{
				rwLock.ExitWriteLock();
			}
		}

		public static bool SwapIfNotEqual<T>(this ReaderWriterLockSlim rwLock, ref T variable, T value) where T : class
		{
			rwLock.EnterUpgradeableReadLock();
			try
			{
				if (variable == value) return false;
				rwLock.EnterWriteLock();
				try
				{
					variable = value;
				}
				finally
				{
					rwLock.ExitWriteLock();
				}
			}
			finally
			{
				rwLock.ExitUpgradeableReadLock();
			}
			return true;
		}
	}
}
