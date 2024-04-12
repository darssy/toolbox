using System.ComponentModel;
using NUnit.Framework;

namespace MmiSoft.Core.ComponentModel
{
	[TestFixture]
	public class EditableObjectHolderTest
	{
		[Test]
		public void TestCancelEditWithNotifyPropertyChanged()
		{
			PropertyNotifier stub = new();
			EditableObjectHolder<PropertyNotifier> objectHolder = new(stub);
			objectHolder.BeginEdit();
			Assert.True(objectHolder.IsEditing);
			Assert.AreEqual(stub.TimesEventFired, 0);
			stub.Property = 20;
			Assert.AreEqual(stub.TimesEventFired, 1);
			objectHolder.CancelEdit();
			Assert.AreEqual(stub.TimesEventFired, 2);
			Assert.AreEqual(stub.Property, 0);
			Assert.False(objectHolder.IsEditing);
		}

		private class PropertyNotifier : ExternallyEditableObject, INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;

			private int timesEventFired;

			private int property;

			public int Property
			{
				get => property;
				set
				{
					if (property == value) return;
					property = value;
					OnPropertyChanged(new PropertyChangedEventArgs("Property"));
				}
			}

			public int TimesEventFired => timesEventFired;

			protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
			{
				timesEventFired++;
				PropertyChanged?.Invoke(this, e);
			}
		}
	}
}
