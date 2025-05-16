using PortSniffer.Core.Abstract;
using PortSniffer.Models;
using PortSniffer.View.ScanProperties;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace PortSniffer.View.Sections
{
    public class ControlPanelView : Panel
    {
        private TableLayoutPanel scanProperties;
        private Panel bottomPanel;

        public IPAddressProperty TargetIP { get; private set; }
        public ControlPanelView()
        {

            Dock = DockStyle.Fill;
            
            scanProperties = new TableLayoutPanel();
            scanProperties.Dock = DockStyle.Top;
            scanProperties.ColumnCount = 1;
            scanProperties.RowCount = 10;
            scanProperties.AutoScroll = true;
            scanProperties.AutoSize = true;

            bottomPanel = new Panel();
            bottomPanel.Dock = DockStyle.Bottom;

            InitializeControls();

            Controls.Add(scanProperties);
            Controls.Add(bottomPanel);
        }

        public void InitializeControls()
        {
            //IP ADDRESS TARGET
            PropertyLabel targetLabel = new PropertyLabel("Target IP:");
            PropertyTooltip targetHelp = new PropertyTooltip("Type in your target IPv4 address");
            PropertyTextInput targetTextbox = new PropertyTextInput("192.168.0.1");
            //IPAddressProperty targetIP = new IPAddressProperty(targetLabel, targetHelp, targetTextbox);
            TargetIP = new IPAddressProperty(targetLabel, targetHelp, targetTextbox);

            Button startButton = new Button();
            startButton.Text = "Start Scan";
            startButton.Dock = DockStyle.Bottom;


            bottomPanel.Controls.Add(startButton);

            scanProperties.Controls.Add(TargetIP);

            Debug.WriteLine("Initialized Control panel  ");
        }

        /// <summary>
        /// Changes the color of the control to indicate a validation error.
        /// </summary>
        /// <param name="property">Control that gets visual changes</param>
        public void HighlightValidationError(ScanPropertyAbstract property)
        {
            //TODO: make this blink
            property.Input.BackColor = Color.FromArgb(255, 222, 222);
        }


        /*
        /// <summary>
        /// Resets the color of the control to indicate no validation error.
        /// </summary>
        /// <param name="property">Control that gets visual changes</param>
        public void ResetValidationError(ScanPropertyAbstract property)
        {
            property.BackColor = Color.White;
        }
        */

    }


}
