using System;
using NUnit.Framework;

namespace MmiSoft.Core
{
	[TestFixture]
	public class ResultTest
	{
		[Test]
		public void DefaultResultIsSuccess()
		{
			Assert.IsTrue(new Result<string>().Succeeded);
		}
		
		[Test]
		public void DefaultResultWithEnumIsSuccess()
		{
			Assert.IsTrue(new Result<Error>().Succeeded);
			Assert.AreEqual(Error.None, new Result<Error>().Error);

			Assert.IsTrue(new Result<string, Error>().Succeeded);
			Assert.AreEqual(Error.None, new Result<string, Error>().Error);
		}
		
		[Test]
		public void ResultWithEnumIsFailure()
		{
			Assert.IsFalse(new Result<Error>(Error.ServerMeltDown).Succeeded);

			Assert.IsFalse(new Result<string, Error>(Error.ServerMeltDown).Succeeded);
			Assert.Throws<InvalidOperationException>(() =>
			{
				string _ = new Result<string, Error>(Error.ServerMeltDown).Value;
			});
		}

		[Test]
		public void ResultWithValueIsSuccess()
		{
			Assert.IsTrue(new Result<string, Error>("Job well done").Succeeded);
		}
		
		[Test]
		public void DefaultResultWithEnumThrows()
		{
			Assert.Throws(typeof(ArgumentException), () => new Result<Error>(Error.None));
			Assert.Throws(typeof(ArgumentException), () => new Result<string, Error>(Error.None));
		}
		
		[Test]
		public void ResultImplicitOperator_TreatsNullAsSuccess()
		{
			Result<string> temp = null;
			Assert.IsTrue(temp.Succeeded);
		}
		
		[Test]
		public void ResultWithEnumImplicitOperator_TreatsZeroValueAsSuccess()
		{
			Result<Error> temp = Error.None;
			Assert.IsTrue(temp.Succeeded);
		}
		
		[Test]
		public void ResultImplicitOperator_TreatsNonNullValuesAsFailure()
		{
			Result<string> temp = "foo";
			Assert.IsFalse(temp.Succeeded);
		}
		
		private enum Error
		{
			None,
			ServerFault,
			ServerBurnt,
			ServerMeltDown
		}
	}
}
