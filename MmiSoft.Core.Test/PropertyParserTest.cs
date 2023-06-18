using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MmiSoft.Core
{
	[TestFixture]
	public class PropertyParserTest
	{
		[Test]
		public void EmptyText_ReturnsEmptyDictionary()
		{
			List<string> lines = new List<string>
			{
				"",
				"    ",
				"  ",
				"\t\t"
			};
			IDictionary<string,string> properties = PropertyParser.ReadProperties(lines);
			Assert.That(properties, Is.Empty);
		}

		[Test]
		public void OneProperty_ResultIsTrimmed()
		{
			List<string> lines = new List<string>
			{
				"     ",
				"  key = value  ",
				"  ",
			};
			IDictionary<string,string> properties = PropertyParser.ReadProperties(lines);
			Assert.That(properties, Has.Count.EqualTo(1));
			Assert.That(properties, Contains.Key("key").WithValue("value"));
		}

		[Test]
		public void KeysAreSetToLowerCase()
		{
			
			List<string> lines = new List<string>
			{
				"  MyKey = SomeValue  ",
			};
			IDictionary<string,string> properties = PropertyParser.ReadProperties(lines);
			Assert.That(properties, Has.Count.EqualTo(1));
			Assert.That(properties, Contains.Key("mykey").WithValue("SomeValue"));
		}

		[Test]
		public void ThreeProperties()
		{
			
			List<string> lines = new List<string>
			{
				"another-key = with some other value",
				"  key = value  ",
				"property = 9",
			};
			IDictionary<string,string> properties = PropertyParser.ReadProperties(lines);
			Assert.That(properties, Has.Count.EqualTo(3));
			Assert.That(properties, Contains.Key("another-key").WithValue("with some other value"));
			Assert.That(properties, Contains.Key("key").WithValue("value"));
			Assert.That(properties, Contains.Key("property").WithValue("9"));
		}

		[Test]
		public void WrongDelimiter()
		{
			List<string> lines = new List<string>
			{
				"  MyKey = SomeValue  ",
				" correct-key:and it's value"
			};
			IDictionary<string,string> properties = PropertyParser.ReadProperties(lines, ':');
			Assert.That(properties, Has.Count.EqualTo(1));
			Assert.That(properties, Contains.Key("correct-key").WithValue("and it's value"));
		}

		[Test]
		public void MultilineProperty_LinesAreTrimmedAndSlashesAreReplacedWithNewLines()
		{
			List<string> lines = new List<string>
			{
				"multi-key = multi \\",
				"  line \\",
				"  value \\",
				"  example"
			};
			IDictionary<string,string> properties = PropertyParser.ReadProperties(lines);
			Assert.That(properties, Has.Count.EqualTo(1));
			string value = string.Join(Environment.NewLine, @"multi", "line", "value", "example");
			Assert.That(properties, Contains.Key("multi-key").WithValue(value));
			
		}

		[Test]
		public void MultilineProperty_WithEmptyLines()
		{
			List<string> lines = new List<string>
			{
				"multi-key = multi\\",
				"  line \\",
				"\\",
				"  value\\",
				"   \\",
				"  example"
			};
			IDictionary<string,string> properties = PropertyParser.ReadProperties(lines);
			Assert.That(properties, Has.Count.EqualTo(1));
			string value = string.Join(Environment.NewLine, @"multi", "line", "", "value", "", "example");
			Assert.That(properties, Contains.Key("multi-key").WithValue(value));
			
		}

		[Test]
		public void DelimiterAppearsTwice_SecondDelimiterIsPartOfValue()
		{
			List<string> lines = new List<string>
			{
				"key = this = that",
			};
			IDictionary<string,string> properties = PropertyParser.ReadProperties(lines);
			Assert.That(properties, Has.Count.EqualTo(1));
			Assert.That(properties, Contains.Key("key").WithValue("this = that"));
		}
	}
}
