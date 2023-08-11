namespace MmiSoft.Core.Text
{
	/// <summary>
	/// Describes a position (line, column and total index) within a text input
	/// </summary>
	public readonly struct Position
	{
		public Position(int column) : this(column, 0, column)
		{ }

		public Position(int index, int line, int column)
		{
			Index = index;
			Line = line;
			Column = column;
		}

		public int Index { get; }
		public int Line { get; }
		public int Column { get; }

		public override string ToString() => $"Ln:{Line}, Col: {Column})";
	}
}
