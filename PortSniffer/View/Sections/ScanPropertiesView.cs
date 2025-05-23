using PortSniffer.Core.Config;
using PortSniffer.View.Abstract;
using PortSniffer.View.Controls;
using PortSniffer.View.Interface;
using PortSniffer.View.ScanProperties;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace PortSniffer.View.Sections
{
    public class ScanPropertiesView : PanelAbstract, IScanPropertiesView
    {
        private TableLayoutPanel scanProperties;

        public IPAddressProperty TargetIP { get; private set; }
        public IPAddressProperty TargetIPRangeEnd { get; private set; }
        public IPAddressProperty SubnetMask { get; private set; }
        public PortProperty PortRangeStart { get; private set; }
        public PortProperty PortRangeEnd { get; private set; }
        public MaxConcurrentProperty MaximumConcurrentScans { get; private set; }
        public TimeoutProperty Timeout { get; private set; }
        public ScanPropertyCheckBox OnlyWellKnownPorts { get; private set; }

        public ScanPropertiesView(Settings settings): base(settings)
        {
            Dock = DockStyle.Fill;

            scanProperties = new TableLayoutPanel();
            scanProperties.Dock = DockStyle.Fill;         
            scanProperties.ColumnCount = 1;
            scanProperties.RowCount = 50;
            scanProperties.AutoScroll = true;
            scanProperties.AutoSize = false;

            InitializeControls();

            Controls.Add(scanProperties);
        }

        public void InitializeControls()
        {
            //TARGET IP
            TargetIP = new IPAddressProperty(
                "Target IP:",
                "Required. Acts as the single IP to scan, or the start of a range, or the base address for subnet scanning.",
                true,
               Settings
            );

            //TARGET IP RANGE END
            TargetIPRangeEnd = new IPAddressProperty(
                "Target IP range-end:",
                "Optional. Acts as the end of a range to scan.",
                false,
                Settings
            );

            //SUBNET MASK
            SubnetMask = new IPAddressProperty(
                "Subnet mask:",
                "Optional. Subnet mask for your target IP, can be set in CIDR format .",
                false,
                Settings
            );

            //PORT RANGE START/ SINGLE PORT
            PortRangeStart = new PortProperty(
                "Port / range-start:",
                "Optional. Acts as the start of a port range to scan.",
                true,
                Settings
            );

            //PORT RANGE END
            PortRangeEnd = new PortProperty(
                "Port range-end:",
                "Optional. Acts as the end of a port range to scan.",
                false,
                Settings
            );

            //MAXIMUM CONCURRENT SCANS
            MaximumConcurrentScans = new MaxConcurrentProperty(
                "Maximum Concurrent Scans:",
                "Optional. Maximum number of concurrent scans to run.",
                false,
                Settings
            );

            //TIMEOUT
            Timeout = new TimeoutProperty(
                "Timeout (ms):",
                "Optional. Timeout for each scan in miliseconds. ",
                false,
                Settings
            );

            //ONLY WELL KNOW PORTS
            OnlyWellKnownPorts = new ScanPropertyCheckBox(
                "Only well known ports:",
                "Optional. If selected, only well known ports (1 - 1023) will be scanned.",
                Settings
            );

            scanProperties.Controls.Add(TargetIP);
            scanProperties.Controls.Add(TargetIPRangeEnd);
            scanProperties.Controls.Add(SubnetMask);
            scanProperties.Controls.Add(PortRangeStart);
            scanProperties.Controls.Add(PortRangeEnd);
            scanProperties.Controls.Add(OnlyWellKnownPorts);
            scanProperties.Controls.Add(MaximumConcurrentScans);
            scanProperties.Controls.Add(Timeout);
        }

        /// <summary>
        /// Changes the color of the control to indicate a validation error.
        /// </summary>
        /// <param name="property">Control that gets visual changes</param>
        public void HighlightValidationError(ScanPropertyInputAbstract property)
        {
            //TODO: make this blink -yeah idk maybe not, just add these colors from config maybe?
            property.Input.BackColor = Color.FromArgb(255, 222, 222);
        }

        /// <summary>
        /// Removes the highlight from the control.
        /// </summary>
        /// <param name="property"></param>
        public void RemoveHighlightValidationError(ScanPropertyInputAbstract property)
        {
            property.Input.BackColor = Color.White;
        }

        /// <summary>
        /// Applies settings from the config to the controls.
        /// </summary>
        public override void ApplySettings()
        {
            foreach (Control control in scanProperties.Controls)
            {
                if (control is PanelAbstract input)
                {
                    input.ApplySettings();
                }
            }
        }
    }


}
