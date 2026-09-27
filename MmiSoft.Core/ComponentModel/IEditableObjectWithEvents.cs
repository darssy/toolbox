using System;
using System.ComponentModel;

namespace MmiSoft.Core.ComponentModel;

public interface IEditableObjectWithEvents
{
	event EventHandler EditStarted;
	event EventHandler EditAccepted;
	event EventHandler EditRejected;

	bool IsEdited { get; }
	
	void BeginEdit();

	void BeforeEndEdit();
	void EndEdit();

	void BeforeCancelEdit();
	void CancelEdit();
}
