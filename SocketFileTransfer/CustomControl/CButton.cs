using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SocketFileTransfer.CustomControl
{
    public class CButton : Button
    {
        public CButton()
        {
            BackColor = SystemColors.Menu;
            CResize();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CResize();
        }

        protected override void OnPaint(PaintEventArgs pEvent)
        {
            base.OnPaint(pEvent);

            var rect = ClientRectangle;
            var smallRect = new Rectangle(2, 2, rect.Width - 4, rect.Height - 4);
            using (var path = new GraphicsPath())
            using (var smallPath = new GraphicsPath())
            using (var brash = new SolidBrush(BackColor))
            using (var pen = new Pen(this.Parent.BackColor, 8))
            {
                path.StartFigure();
                path.AddArc(rect.X, rect.Y, Width, Width, 180, 90);
                path.AddArc(rect.Right - Width, rect.Y, Width, Width, 270, 90);
                path.AddArc(rect.Right - Width, rect.Bottom - Width, Width, Width, 0, 90);
                path.AddArc(rect.X, rect.Bottom - Width, Width, Width, 90, 90);
                path.CloseFigure();
                Region = new Region(path);

                smallPath.StartFigure();
                smallPath.AddArc(smallRect.X, smallRect.Y, Width, Width, 180, 90);
                smallPath.AddArc(smallRect.Right - Width, smallRect.Y, Width, Width, 270, 90);
                smallPath.AddArc(smallRect.Right - Width, smallRect.Bottom - Width, Width, Width, 0, 90);
                smallPath.AddArc(smallRect.X, smallRect.Bottom - Width, Width, Width, 90, 90);
                smallPath.CloseFigure();

                pEvent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                pEvent.Graphics.FillPath(brash, smallPath);
                pEvent.Graphics.DrawPath(pen, smallPath);
            }
            DrawText(pEvent);
        }

        private void DrawText(PaintEventArgs pEvent)
        {
            var text = TextRenderer.MeasureText(Text, Font, new Size(ClientRectangle.Width, ClientRectangle.Height), TextFormatFlags.WordBreak);

            var point = new Point(Width / 2 - (text.Width / 2), Height / 2 - (text.Height / 2));
            pEvent.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), point);
        }

        private void CResize()
        {
            if (Width != Height || Height != Width)
            {
                Height = Width;
                Width = Height;
            }
            else
            {
                return;
            }
            Invalidate();
        }
    }
}
