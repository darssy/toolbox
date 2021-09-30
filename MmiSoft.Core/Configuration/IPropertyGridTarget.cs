using System;

namespace MmiSoft.Core.Configuration
{
	public interface IPropertyGridTarget
	{
		string Name { get; }
		string Description { get; }
		string Category { get; }
		object Value { get; set; }
	}

	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ExpectedTypeAttribute : Attribute
	{
		public ExpectedTypeAttribute(Type valuePropertyType)
		{
			ValuePropertyType = valuePropertyType ?? throw new ArgumentNullException(nameof(valuePropertyType));
		}

		public Type ValuePropertyType { get; }
	}
}
