namespace MmiSoft.Core.ComponentModel
{
	/// <summary>
	/// Abstraction for objects that can conveniently be identified (aka designated) by a string, usually its name
	/// </summary>
	public interface IDesignatedItem
	{
		/// <summary>
		/// The designation of the object, ideally immutable and unique among the items of the same category
		/// </summary>
		string Designation { get; }
	}
}
