using System.Drawing;
using MmiSoft.Core.ComponentModel;
using NUnit.Framework;

namespace MmiSoft.Core
{
	[TestFixture]
	internal class Extensions
	{
		[Test]
		public void TestCopyObjectsPrimitiveTypes()
		{
			PrimitivesStub original = new PrimitivesStub();

			original.SetReadonlyProp(99);
			original.SetPrivateProp(4);
			original.SomeInt = 10;
			original.SomeString = "str";

			PrimitivesStub clone = new PrimitivesStub();
			original.Copy(clone);
			Assert.AreEqual(clone.SomeString, "str");
			Assert.AreEqual(clone.SomeInt, 10);
			Assert.AreEqual(clone.ReadonlyProp, 0);
			Assert.AreEqual(clone.GetPrivateProp(), 0);
		}

		[Test]
		public void TestCopyObjectsStructs()
		{
			StructStub original = new StructStub { SomeColor = Color.AliceBlue };
			StructStub clone = new StructStub();
			original.Copy(clone);
			Assert.AreEqual(clone.SomeColor, Color.AliceBlue);
		}

		[Test]
		public void TestWithInheritance()
		{
			EditableObjectHolder<InheritedStub> editable = new(new InheritedStub());
			editable.Object.SomeString = "string";
			Assert.False(editable.Object.IsEdited);
			editable.BeginEdit();
			Assert.True(editable.Object.IsEdited);
			editable.Object.SomeString = "other string";
			editable.CancelEdit();
			Assert.False(editable.Object.IsEdited);
			Assert.AreEqual(editable.Object.SomeString, "string");
		}

		private class PrimitivesStub : ExternallyEditableObject
		{
			public int SomeInt { get; set; }
			public string SomeString { get; set; }
			public float ReadonlyProp { get; private set; }
			private byte PrivateProp { get; set; }

			public void SetPrivateProp(byte val)
			{
				PrivateProp = val;
			}

			public byte GetPrivateProp()
			{
				return PrivateProp;
			}

			public void SetReadonlyProp(float val)
			{
				ReadonlyProp = val;
			}
		}

		private class StructStub
		{
			public Color SomeColor { get; set; }
		}

		private class InheritedStub : PrimitivesStub
		{

		}
	}
}
