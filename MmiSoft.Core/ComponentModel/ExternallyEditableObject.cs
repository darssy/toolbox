using System;

namespace MmiSoft.Core.ComponentModel;

public class ExternallyEditableObject : IEditableObjectWithEvents
{
	public event EventHandler EditStarted;
	public event EventHandler EditAccepted;
	public event EventHandler EditRejected;

	public bool IsEdited { get; private set; }
	public void BeginEdit()
	{
		IsEdited = true;
		EditStarted?.Invoke(this, EventArgs.Empty);
	}

	public void EndEdit()
	{
		IsEdited = false;
		EditAccepted?.Invoke(this, EventArgs.Empty);
	}

	public void CancelEdit()
	{
		IsEdited = false;
		EditRejected?.Invoke(this, EventArgs.Empty);
	}
}
