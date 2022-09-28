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
		}
		
		[Test]
		public void ResultWithEnumIsFailure()
		{
			Assert.IsFalse(new Result<Error>(Error.ServerMeltDown).Succeeded);
		}
		
		[Test]
		public void DefaultResultWithEnumThrows()
		{
			Assert.Throws(typeof(ArgumentException), () => new Result<Error>(Error.None));
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
