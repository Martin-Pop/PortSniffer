using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortSniffer.View.ScanProperties
{
    /// <summary>
    /// Represents a little tooltip (? icon).
    /// </summary>
    public class PropertyTooltip : PictureBox
    {
        private const int OFFSET_Y = -30;

        private string text;
        private ToolTip toolTip;

        public PropertyTooltip(string text)
        {
            this.text = text;

            Size = new Size(14, 14);
            SizeMode = PictureBoxSizeMode.Zoom;
            Margin = new Padding(0);
            Image = Resources.HelpIcon.ToBitmap();
            Dock = DockStyle.Left;

            toolTip = new ToolTip();
            MouseEnter += ShowTooltip;
            MouseLeave += HideToolTip;
        }

        /// <summary>
        /// Event handler for showing the tooltip when the mouse enters the control.
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">event arguments</param>
        private void ShowTooltip(object? sender, EventArgs e)
        {
            Point mouse = PointToClient(Cursor.Position);
            toolTip.Show(text, this, new Point(mouse.X, mouse.Y + OFFSET_Y));
        }
        /// <summary>
        /// Event handler for hiding the tooltip when the mouse leaves the control.
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">event arguments</param>
        private void HideToolTip(object? sender, EventArgs e)
        {
            toolTip.Hide(this);
        }
    }
}
