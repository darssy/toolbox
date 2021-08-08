namespace MmiSoft.Core
{
	/// <summary>
	/// A collection of some unicode symbols as <c>UpEmptyTriangle</c> might be more straightforward than <c>'\u25B3'</c>.
	/// The list is not exhaustive -yet.
	/// </summary>
	public static class UniChar
	{
		public const char Ellipsis = '\u2026';

		public const char RightFilledTriangle = '\u25B6';
		public const char LeftFilledTriangle = '\u25C0';
		public const char UpFilledTriangle = '\u25B2';
		public const char DownFilledTriangle = '\u25BC';

		public const char UpEmptyTriangle = '\u25B3';
		public const char DownEmptyTriangle = '\u25BD';

		public const char UpwardsArrow = '\u2191';
		public const char DownwardsArrow = '\u2193';
	}
}
