using System.Drawing;

namespace MmiSoft.Core.Controls
{
	public interface IPointTranslator
	{
		Point PointToClient(Point screenPoint);
		Point PointToScreen(Point clientPoint);
	}
}
