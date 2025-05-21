using PortSniffer.View.ScanProperties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Abstract
{
    public abstract class ScanPropertyInputAbstract : Panel
    {
        protected FlowLayoutPanel labelPanel;
        public Label Label { get; init; }
        public PropertyTooltip Tooltip { get; init; }
        public TextBox Input { get; protected set; }
        public bool IsValid { get; set; }
        public bool IsRequired { get; init; }
        protected ScanPropertyInputAbstract(string label, string toolTipMessage, bool required, string placeholder = "")
        {
            //this
            AutoSize = true;
            Dock = DockStyle.Fill;

            //panel
            labelPanel = new FlowLayoutPanel();
            labelPanel.FlowDirection = FlowDirection.LeftToRight;
            labelPanel.WrapContents = false; //important
            labelPanel.AutoSize = true;
            labelPanel.Dock = DockStyle.Top;

            //label
            Label = new Label();
            Label.Text = label;
            Label.Font = new Font("Consolas", 11F, FontStyle.Regular);
            Label.TextAlign = ContentAlignment.MiddleLeft;
            Label.AutoSize = true;
            Label.Dock = DockStyle.Left;

            //tooltip
            Tooltip = new PropertyTooltip(toolTipMessage);

            Input = new TextBox();
            Input.Font = new Font("Consolas", 11F, FontStyle.Regular);
            Input.Multiline = false;
            Input.Dock = DockStyle.Top;

            labelPanel.Controls.Add(Label);
            labelPanel.Controls.Add(Tooltip);

            //required star
            IsRequired = required;
            if (required)
            {
                Label l = new Label();
                l.Text = "*";
                l.Font = new Font("Consolas", 11F, FontStyle.Regular);
                l.TextAlign = ContentAlignment.MiddleLeft;
                l.ForeColor = Color.Red;
                labelPanel.Controls.Add(l);
            }
        
            Controls.Add(Input);
            Controls.Add(labelPanel);
        }

        /// <summary>
        /// Reset the property to its default state.
        /// </summary>
        public abstract void Reset();
    }
}
