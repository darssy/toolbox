using System;
using System.ComponentModel;

namespace MmiSoft.Core.ComponentModel;

public interface IEditableObjectWithEvents : IEditableObject
{
	public event EventHandler EditStarted;
	public event EventHandler EditAccepted;
	public event EventHandler EditRejected;

	bool IsEdited { get; }
}
