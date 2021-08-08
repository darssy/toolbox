using System;
using NUnit.Framework;

namespace MmiSoft.Core.Test
{
	[TestFixture]
	public class DateAndTimeExtensionsTest
	{

		[Test]
		public void DateAt30SecondsAndSomeMillisRoundsToNextMinute()
		{
			DateTime date = new DateTime(2000, 10, 1, 13, 5, 30, 350);
			DateTime expected = new DateTime(2000, 10, 1, 13, 6, 0);
			Assert.AreEqual(expected, date.RoundToMinute());
		}

		[Test]
		public void DateAt30SecondsExactlyRoundsToNextMinute()
		{
			DateTime date = new DateTime(2000, 10, 1, 13, 5, 30);
			DateTime expected = new DateTime(2000, 10, 1, 13, 6, 0);
			Assert.AreEqual(expected, date.RoundToMinute());
		}

		[Test]
		public void DateAt29SecondsAnd999MillisRoundsToCurrentMinute()
		{
			DateTime date = new DateTime(2000, 10, 1, 13, 5, 29, 999);
			DateTime expected = new DateTime(2000, 10, 1, 13, 5, 0);
			Assert.AreEqual(expected, date.RoundToMinute());
		}

		[Test]
		public void TimeSpanMultipliedBy2ReturnsTwiceAsBigValue()
		{
			TimeSpan magnified = TimeSpan.FromMinutes(2.5).Magnify(2);
			Assert.AreEqual(TimeSpan.FromMinutes(5), magnified);
		}
	}
}
