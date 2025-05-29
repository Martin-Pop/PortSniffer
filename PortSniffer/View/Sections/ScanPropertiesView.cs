using PortSniffer.Core.Config;
using PortSniffer.Core.Interface;
using PortSniffer.View.Abstract;
using PortSniffer.View.Controls;
using PortSniffer.View.Interface;
using PortSniffer.View.Properties;
using PortSniffer.View.ScanProperties;

namespace PortSniffer.View.Sections
{
    public class ScanPropertiesView : PanelAbstract, IScanProperties
    {
        private TableLayoutPanel scanProperties;
        public IPAddressProperty TargetIP { get; private set; }
        public IPAddressProperty TargetIPRangeEnd { get; private set; }
        public IPAddressProperty SubnetMask { get; private set; }
        public PortProperty PortRangeStart { get; private set; }
        public PortProperty PortRangeEnd { get; private set; }
        public MaxConcurrentProperty MaximumConcurrentScans { get; private set; }
        public TimeoutProperty Timeout { get; private set; }
        public PredefinedPortsProperty OnlyWellKnownPorts { get; private set; }
        public PredefinedPortsProperty OnlyRegisteredPorts { get; private set; }
        public PredefinedPortsProperty OnlyPrivatePorts { get; private set; }
        public PredefinedPortsProperty AllPorts { get; private set; }

        public ScanPropertiesView(Settings settings) : base(settings)
        {
            AutoSize = true;
            Dock = DockStyle.Fill;

            scanProperties = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                AutoScroll = true,
                ColumnCount = 1,
                RowCount = 20,
                Padding = new Padding(0, 0, 1, 0) //supresses horizontal scrollbar!!!! //inspired by this answer => https://stackoverflow.com/a/6555682
            };

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
                "Optional. Maximum number of concurrent scans (on a single IP) to run.",
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
            OnlyWellKnownPorts = new PredefinedPortsProperty(
                "Only well known ports:",
                "Optional. If selected, only well known ports (1 - 1023) will be scanned.",
                Settings,
                1,
                1023
            );

            //ONLY REGISTERED PORTS
            OnlyRegisteredPorts = new PredefinedPortsProperty(
                 "Only registered ports:",
                 "Optional. If selected, only registered ports (1024 - 49151) will be scanned.",
                 Settings,
                 1024,
                 49151
             );

            //ONLY PRIVATE PORTS
            OnlyPrivatePorts = new PredefinedPortsProperty(
                "Only private ports:",
                "Optional. If selected, only private ports (49152 - 65535) will be scanned.",
                Settings,
                49152,
                65535
            );

            //ALL PORTS
            AllPorts = new PredefinedPortsProperty(
                "All ports:",
                "Optional. If selected, all ports (1 - 65535) will be scanned.",
                Settings,
                1,
                65535
            );

            scanProperties.Controls.Add(TargetIP);
            scanProperties.Controls.Add(TargetIPRangeEnd);
            scanProperties.Controls.Add(SubnetMask);
            scanProperties.Controls.Add(PortRangeStart);
            scanProperties.Controls.Add(PortRangeEnd);
            scanProperties.Controls.Add(OnlyWellKnownPorts);
            scanProperties.Controls.Add(OnlyRegisteredPorts);
            scanProperties.Controls.Add(OnlyPrivatePorts);
            scanProperties.Controls.Add(AllPorts);
            scanProperties.Controls.Add(MaximumConcurrentScans);
            scanProperties.Controls.Add(Timeout);
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

        /// <summary>
        /// Resets all scan properties to their default state.
        /// </summary>
        public void ClearAll()
        {
            foreach (Control control in scanProperties.Controls)
            {
                if (control is ScanPropertyInputAbstract inputProperty)
                {
                    inputProperty.Reset();
                }

                if (control is ScanPropertyCheckBoxAbstract checkboxProperty)
                {
                    checkboxProperty.Input.Checked = false; //yes this will trigger the validation events .... ( ˘︹˘ )
                }
            }
        }
    }

}
