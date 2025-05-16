using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortSniffer.View.ScanProperties
{
    public class PropertyTooltip : PictureBox
    {
        private const int OFFSET_Y = -30;

        private string text;
        private ToolTip toolTip;

        public PropertyTooltip(string text)
        {
            this.text = text;

            Size = new Size(14, 14);
            Margin = new Padding(0);
            Dock = DockStyle.Left;
            SizeMode = PictureBoxSizeMode.Zoom;
            Image = Resources.HelpIcon.ToBitmap();

            toolTip = new ToolTip();
            MouseEnter += ShowTooltip;
            MouseLeave += HideToolTip;
        }

        private void ShowTooltip(object? sender, EventArgs e)
        {
            Point mouse = PointToClient(Cursor.Position);
            toolTip.Show(text, this, new Point(mouse.X, mouse.Y + OFFSET_Y));
        }

        private void HideToolTip(object? sender, EventArgs e)
        {
            toolTip.Hide(this);
        }

    }
}
