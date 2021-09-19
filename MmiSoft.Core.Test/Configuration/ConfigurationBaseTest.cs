using System;
using System.Drawing;
using System.IO;
using System.Net;
using MmiSoft.Core.ComponentModel;
using MmiSoft.Core.Configuration;
using MmiSoft.Core.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace MmiSoft.Core.Test.Configuration
{
	[TestFixture]
	public class ConfigurationBaseTest
	{
		[Test]
		[Ignore("The static ctor is not called and the converter is not loaded.")]
		public void ReadConfigurationFromFileWithConverterAddedInSubclass()
		{
			string filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Configuration", "TestConfig.json");
			TestConfigExt config = ConfigurationBase.ReadConfig<TestConfigExt>(filePath);
			Assert.AreEqual(config.Test.Value, 123);
		}

		[Test]
		public void ConfigurationEditing()
		{
			EditableObjectHolder<TestConfig> editable = new EditableObjectHolder<TestConfig>(new TestConfig());
			editable.BeginEdit();
			editable.Object.AccInputMethod = TestEnum.One;
			editable.Object.AccMaxCmdDelay = 5;
			editable.Object.ConnectionAddress = IPAddress.Parse("192.168.1.1");
			editable.CancelEdit();
			Assert.AreEqual(editable.Object.AccInputMethod, TestEnum.Two);
			Assert.AreEqual(editable.Object.ConnectionAddress, IPAddress.Loopback);
		}

		[Test]
		public void ReadConfigurationFromFile()
		{
			string filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Configuration", "TestConfig.json");
			TestConfig config = ConfigurationBase.ReadConfig<TestConfig>(filePath);
			Assert.AreEqual(config.AccInputMethod, TestEnum.Three);
			Assert.AreEqual(config.ConnectionAddress, IPAddress.Parse("192.168.1.1"));
		}

		private class TestConfig : ConfigurationBase
		{
			public IPAddress ConnectionAddress {get; set; } = IPAddress.Loopback;
			public TestEnum AccInputMethod {get; set; } = TestEnum.Two;
			public int AccMaxCmdDelay { get; set; } = 7;
		}

		private class TestConfigExt: TestConfig
		{
			static TestConfigExt()
			{
				JsonSettings.Converters.Add(new RoTypeJsonConverter());
			}

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
