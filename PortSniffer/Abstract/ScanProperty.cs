using PortSniffer.UI.ScanProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Abstract
{
    public abstract class ScanProperty : Panel
    {
        protected FlowLayoutPanel labelPanel;
        protected PropertyLabel label;
        protected PropertyTooltip tooltip;
        protected Control control;
        public virtual bool IsValid { get; protected set; }
        
        protected ScanProperty(PropertyLabel pl, PropertyTooltip tp, Control c) 
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

            control = c;
            control.Dock = DockStyle.Top;

            Controls.Add(control);
            Controls.Add(labelPanel);
        }
    }
}
