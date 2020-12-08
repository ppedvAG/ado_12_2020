using System;
using System.Drawing;
using System.Windows.Forms;

namespace HalloLinq
{
    public class ImageDataGrid : DataGridView
    {
        public ImageDataGrid()
        {
            this.DoubleBuffered = true;
        }
        protected override void PaintBackground(Graphics graphics, Rectangle clipBounds, Rectangle gridBounds)
        {
            base.PaintBackground(graphics, clipBounds, gridBounds);

            graphics.DrawImage(Properties.Resources.VhjrRLR, gridBounds);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }
    }

}
