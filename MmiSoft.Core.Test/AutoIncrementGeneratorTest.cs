using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace MmiSoft.Core
{
	[TestFixture]
	public class AutoIncrementGeneratorTest
	{
		[Test]
		public void ConcurrentThreadedAccess_AssertsThatEachDigitWasReturnedOnce()
		{
			List<int> autoincrement = new List<int>();
			AutoIncrementGenerator aig = new AutoIncrementGenerator();

			List<Thread> t = new List<Thread>();
			const int participantCount = 8;
			const int resultsPerThread = 3000;

			Barrier b = new Barrier(participantCount);
			for (int i = 0; i < participantCount; i++)
			{
				t.Add(new Thread(o =>
				{
					List<int> l = new List<int>(resultsPerThread);
					b.SignalAndWait();
					for (int j = 0; j < resultsPerThread; j++)
					{
						l.Add(aig.NextDigit());
					}

					lock (autoincrement)
					{
						autoincrement.AddRange(l);
					}
				}));
			}

			for (int i = 0; i < t.Count; i++)
			{
				Thread thread = t[i];
				thread.Start();
			}

			foreach (Thread thread in t)
			{
				thread.Join();
			}
			autoincrement.Sort();
			Assert.That(new HashSet<int>(autoincrement).Count, Is.EqualTo(participantCount * resultsPerThread));
			for (int i = 0; i < autoincrement.Count; i++)
			{
				Assert.That(autoincrement[i], Is.EqualTo(i + 1));
			}
			
		}
	}
}
