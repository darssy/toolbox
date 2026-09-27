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

	/// <summary>
	/// Called before edit is closed via EndEdit(); can be used to facilitate housekeeping e.g. commit changes of edited nested object 
	/// </summary>
	public virtual void BeforeEndEdit() {}

	public void EndEdit()
	{
		if (!IsEdited) return;
		IsEdited = false;
		EditAccepted?.Invoke(this, EventArgs.Empty);
	}

	/// <summary>
	/// Called before the memento is restored which is before CancelEdit() call. It can be used to suppress event invocations
	/// or side effect application
	/// </summary>
	public virtual void BeforeCancelEdit() {}

	public void CancelEdit()
	{
		if (!IsEdited) return;
		IsEdited = false;
		EditRejected?.Invoke(this, EventArgs.Empty);
	}
}
