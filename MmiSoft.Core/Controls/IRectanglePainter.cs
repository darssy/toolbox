using System.Drawing;

namespace MmiSoft.Core.Controls
{
	public interface IRectanglePainter
	{
		Color PaintColor { get; set; }
		void UpdateRectangle(Rectangle rect);
		void StartPainting(Rectangle rect);
		void CancelPainting();
	}
}
