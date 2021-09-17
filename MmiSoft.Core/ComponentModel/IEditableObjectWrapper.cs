using System.ComponentModel;

namespace MmiSoft.Core.ComponentModel
{
	public interface IEditableObjectWrapper : IEditableObject
	{
		object Object { get; }
	}
}
