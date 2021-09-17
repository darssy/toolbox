using System.ComponentModel;
using MmiSoft.Core.ComponentModel;
using NUnit.Framework;

namespace MmiSoft.Core.Test.ComponentModel
{
	[TestFixture]
	public class EditableObjectHolderTest
	{
		[Test]
		public void TestCancelEditWithNotifyPropertyChanged()
		{
			PropertyNotifier stub = new PropertyNotifier();
			EditableObjectHolder<PropertyNotifier> objectHolder = new EditableObjectHolder<PropertyNotifier>(stub);
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

		private class PropertyNotifier : INotifyPropertyChanged, IExternallyEditable
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
				PropertyChangedEventHandler handler = PropertyChanged;
				if (handler != null) handler(this, e);
			}

			public bool IsEdited { get; set; }
		}
	}
}
