using PortSniffer.View.ScanProperties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Core.Abstract
{
    public abstract class ScanPropertyAbstract : Panel
    {
        protected FlowLayoutPanel labelPanel;
        protected PropertyLabel label;
        protected PropertyTooltip tooltip;
        public Control Input { get; protected set; }
        public bool IsValid { get; set; }
        protected ScanPropertyAbstract(PropertyLabel pl, PropertyTooltip tp, Control c)
        {
            Dock = DockStyle.Fill;

            labelPanel = new FlowLayoutPanel();

            labelPanel.FlowDirection = FlowDirection.LeftToRight;
            labelPanel.AutoSize = true;
            labelPanel.Dock = DockStyle.Top;

            label = pl;
            label.AutoSize = true;
            label.Dock = DockStyle.Left;

            tooltip = tp;

            labelPanel.Controls.Add(label);
            labelPanel.Controls.Add(tooltip);

            Input = c;
            Input.Dock = DockStyle.Top;

            Controls.Add(Input);
            Controls.Add(labelPanel);
        }
    }
}
