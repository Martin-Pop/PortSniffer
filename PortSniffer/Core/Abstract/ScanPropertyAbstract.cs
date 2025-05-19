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
        public PropertyLabel Label { get; protected set; }
        public PropertyTooltip Tooltip { get; protected set; }
        public Control Input { get; protected set; }
        public bool IsValid { get; set; }
        public bool IsRequired { get; }
        protected ScanPropertyAbstract(PropertyLabel pl, PropertyTooltip tp, Control c, bool required)
        {
            IsRequired = required;

            AutoSize = true;
            Dock = DockStyle.Fill;

            labelPanel = new FlowLayoutPanel();
            labelPanel.FlowDirection = FlowDirection.LeftToRight;
            labelPanel.WrapContents = false; //important
            labelPanel.AutoSize = true;
            labelPanel.Dock = DockStyle.Top;

            Label = pl;
            Tooltip = tp;

            labelPanel.Controls.Add(Label);
            labelPanel.Controls.Add(Tooltip);

            if (required)
            {
                PropertyLabel l = new PropertyLabel("*");
                l.ForeColor = Color.Red;
                labelPanel.Controls.Add(l);
            }
          
            Input = c;
            Input.Dock = DockStyle.Top;

            Controls.Add(Input);
            Controls.Add(labelPanel);
        }
    }
}
