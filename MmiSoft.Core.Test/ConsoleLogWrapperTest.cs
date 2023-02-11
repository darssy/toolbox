using MmiSoft.Core.Logging;
using NUnit.Framework;

namespace MmiSoft.Core
{
	[TestFixture]
	public class ConsoleLogWrapperTest
	{
		[Test]
		public void IsLevelSet_TestingForLowerHigherAndExact()
		{
			ConsoleLogWrapper wrapper = new ConsoleLogWrapper
			{
				LogLevel = LogSeverity.Info
			};
			
			Assert.That(wrapper.IsLevelSet(LogSeverity.Info, null));
			Assert.That(wrapper.IsLevelSet(LogSeverity.Error, ""));
			Assert.That(!wrapper.IsLevelSet(LogSeverity.Debug, ""));
		}
	}
}
