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
		if (IsEdited) return; // unfortunately a data grid view is calling BeginEdit just for kicks...
		IsEdited = true;
		EditStarted?.Invoke(this, EventArgs.Empty);
	}

	public void EndEdit()
	{
		if (!IsEdited) return;
		IsEdited = false;
		EditAccepted?.Invoke(this, EventArgs.Empty);
	}

	public void CancelEdit()
	{
		if (!IsEdited) return;
		IsEdited = false;
		EditRejected?.Invoke(this, EventArgs.Empty);
	}
}
