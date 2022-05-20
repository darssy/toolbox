using System;
using System.IO;
using System.Net;
using MmiSoft.Core.ComponentModel;
using MmiSoft.Core.Math;
using MmiSoft.Core.Math.Units;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace MmiSoft.Core.Configuration
{
	[TestFixture]
	public class ConfigurationBaseTest
	{
		[Test]
		public void ReadConfigurationFromFileWithConverterAddedInSubclass()
		{
			string filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Configuration", "TestConfig.json");
			TestConfigExt config = ConfigurationBase.ReadConfig<TestConfigExt>(filePath);
			Assert.AreEqual(config.Test.Value, 123);
		}

		[Test]
		public void ConfigurationEditing_Canceled()
		{
			EditableObjectHolder<TestConfig> editable = new EditableObjectHolder<TestConfig>(new TestConfig());
			editable.BeginEdit();
			editable.Object.AccInputMethod = TestEnum.One;
			editable.Object.AccMaxCmdDelay = 5;
			editable.Object.ConnectionAddress = IPAddress.Parse("192.168.1.1");
			editable.Object.Acceleration = new FeetPerSecondSquared(9.99);
			editable.CancelEdit();
			Assert.AreEqual(editable.Object.AccInputMethod, TestEnum.Two);
			Assert.AreEqual(editable.Object.ConnectionAddress, IPAddress.Loopback);
			Assert.AreEqual(new FeetPerSecondSquared(1.25), editable.Object.Acceleration);
		}

		[Test]
		public void ConfigurationEditing_Applied()
		{
			EditableObjectHolder<TestConfig> editable = new EditableObjectHolder<TestConfig>(new TestConfig());
			editable.BeginEdit();
			editable.Object.AccInputMethod = TestEnum.One;
			editable.Object.AccMaxCmdDelay = 5;
			editable.Object.ConnectionAddress = IPAddress.Parse("192.168.1.1");
			editable.Object.Acceleration = new FeetPerSecondSquared(9.99);
			editable.EndEdit();
			Assert.AreEqual(TestEnum.One, editable.Object.AccInputMethod);
			Assert.AreEqual(IPAddress.Parse("192.168.1.1"), editable.Object.ConnectionAddress);
			Assert.AreEqual(new FeetPerSecondSquared(9.99), editable.Object.Acceleration);
		}

		[Test]
		public void ReadConfigurationFromFile()
		{
			string filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Configuration", "TestConfig.json");
			TestConfig config = ConfigurationBase.ReadConfig<TestConfig>(filePath);
			Assert.AreEqual(config.AccInputMethod, TestEnum.Three);
			Assert.AreEqual(config.ConnectionAddress, IPAddress.Parse("192.168.1.1"));
			Assert.AreEqual(config.Inflation, new Percent(2.34));
			Assert.AreEqual(config.Acceleration, new FeetPerSecondSquared(4.56));
		}

		[UnitConverter("el-gr")]
		[EnumConverter]
		[PercentConverter("el-gr")]
		[IpAddressConverter]
		private class TestConfig : ConfigurationBase
		{
			public IPAddress ConnectionAddress {get; set; } = IPAddress.Loopback;
			public TestEnum AccInputMethod {get; set; } = TestEnum.Two;
			public int AccMaxCmdDelay { get; set; } = 7;
			public Percent Inflation { get; set; } = new Percent(32.5);
			public FeetPerSecondSquared Acceleration { get; set; } = new FeetPerSecondSquared(1.25);
		}

		[RoTypeJsonConverter]
		private class TestConfigExt: TestConfig
		{
			public TypeWith2Ctors Test { get; set; }
		}

		private class RoTypeJsonConverter : JsonConverter<TypeWith2Ctors>
		{
			public override void WriteJson(JsonWriter writer, TypeWith2Ctors value, JsonSerializer serializer)
			{
				var property = new JProperty(nameof(TypeWith2Ctors.Value), value.Value);
				JObject obj = new JObject(property);
				serializer.Serialize(writer, obj);
			}

			public override TypeWith2Ctors ReadJson(JsonReader reader, Type objectType, TypeWith2Ctors existingValue, bool hasExistingValue,
				JsonSerializer serializer)
			{
				JObject jObject = JObject.Load(reader);
				if (jObject.Count != 1) throw new JsonException($"Unexpected number of tokens: {jObject.Count}");

				int value = jObject[nameof(TypeWith2Ctors.Value)].Value<int>();
				return new TypeWith2Ctors(value);
			}
		}

		internal class RoTypeJsonConverterAttribute : JsonConverterBaseAttribute
		{
			public override JsonConverter Create() => new RoTypeJsonConverter();
		}

		public enum TestEnum
		{
			One, Two, Three
		}

		public readonly struct TypeWith2Ctors
		{
			public int Value { get; }

			public TypeWith2Ctors(int value)
			{
				Value = value;
			}

			public TypeWith2Ctors(double value)
			{
				Value = value.RoundToInt();
			}

		}
	}
}
