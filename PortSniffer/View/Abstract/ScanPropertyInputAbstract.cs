using PortSniffer.Core.Config;
using PortSniffer.View.Interface;
using PortSniffer.View.ScanProperties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Abstract
{
    public abstract class ScanPropertyInputAbstract : PanelAbstract
    {
        protected FlowLayoutPanel labelPanel;
        public Label Label { get; init; }
        public Label RequiredStart { get; init; }
        public PropertyTooltip Tooltip { get; init; }
        public TextBox Input { get; protected set; }
        public bool IsValid { get; set; }
        public bool IsRequired { get; init; }

        public event EventHandler? ValidationEvent;

        protected ScanPropertyInputAbstract(string label, string toolTipMessage, bool required,Settings settings ,string placeholder = ""): base (settings)
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
            Label.TextAlign = ContentAlignment.MiddleLeft;
            Label.AutoSize = true;

            //tooltip
            Tooltip = new PropertyTooltip(toolTipMessage);

            //input
            Input = new TextBox();
            Input.Multiline = false;
            Input.Dock = DockStyle.Top;

            labelPanel.Controls.Add(Label);
            labelPanel.Controls.Add(Tooltip);

            //required star
            IsRequired = required;
            
            RequiredStart = new Label();
            RequiredStart.Text = "*";
            RequiredStart.TextAlign = ContentAlignment.MiddleLeft;
            RequiredStart.ForeColor = Color.Red;
                
            if (required) labelPanel.Controls.Add(RequiredStart);
            
            ApplySettings();
        
            Controls.Add(Input);
            Controls.Add(labelPanel);

            IsValid = false;
            Input.KeyDown += Control_KeyDown;
            Input.LostFocus += (_, _) => ValidationEvent?.Invoke(this, EventArgs.Empty);
            Input.GotFocus += (_, _) => Input.BackColor = Color.White;
        }

        private void Control_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                Label.Focus(); //lose focus so that the event is triggered
            }
        }

        public override void ApplySettings()
        {
            Label.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
            Input.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
            RequiredStart.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
        }

        /// <summary>
        /// Reset the property to its default state.
        /// </summary>
        public abstract void Reset();
    }
}
