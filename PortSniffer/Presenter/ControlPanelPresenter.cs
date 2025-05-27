using PortSniffer.Core.Interface;
using PortSniffer.Model;
using PortSniffer.View.Abstract;
using PortSniffer.View.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Presenter
{
    public class ControlPanelPresenter
    {
        private readonly IControlPanelView controlPanelView;
        private readonly IScanProperties scanProperties;
        private readonly IConsoleLogger logger;

        public ControlPanelPresenter(IControlPanelView controlPanelView, IScanProperties scanProperties, IConsoleLogger logger)
        {
            this.controlPanelView = controlPanelView;
            this.scanProperties = scanProperties;
            this.logger = logger;

            // events
            controlPanelView.Start.Click += Start_Click;
            controlPanelView.Stop.Click += Stop_Click;
            controlPanelView.PauseResume.Click += PauseResume_Click;
            controlPanelView.Clear.Click += Clear_Click;
        }


        private ScanConfiguration? CreateScanConfiguration()
        {
            // Example: Create a scan configuration based on user input
            // This method can be expanded to gather data from the view and create a ScanConfig object
            // var config = new ScanConfig(addresses, ports, timeOut, maxThreads);
            return null;
        }

        private bool CanStartScanning()
        {
            if(scanProperties.TargetIP.IsValid && scanProperties.PortRangeStart.IsValid)
            {
                return true;
            }
            else
            {
                scanProperties.TargetIP.Error();
                scanProperties.PortRangeStart.Error();
                logger.Error("Can not strat scaning: Required properties are invalid");
                return false;
            }
        }

        private void Start_Click(object? sender, EventArgs e)
        {
            if (CanStartScanning())
            {
                //IP
                IPAddress[] range = new IPAddress[2];

                if (scanProperties.SubnetMask.IsValid)
                {
                    range = IPUtilities.MaskToRange(scanProperties.TargetIP.IpAddress, scanProperties.SubnetMask.IpAddress);
                }
                else
                {
                    range[0] = scanProperties.TargetIP.IpAddress;
                    range[1] = scanProperties.TargetIPRangeEnd.IpAddress;
                }

                List<IPAddress> ips = new List<IPAddress>();
                if (scanProperties.TargetIPRangeEnd.IsValid || scanProperties.SubnetMask.IsValid)
                {
                    ips = IPUtilities.GetIPAddressesFromRange(range[0], range[1]);
                }
                else
                {
                    ips = new List<IPAddress>() { range[0] };
                }

                // Ports
                List<int> ports = new List<int>();

                if (scanProperties.PortRangeEnd.IsValid)
                {
                    ports = IPUtilities.RangeToPortList(scanProperties.PortRangeStart.Port, scanProperties.PortRangeEnd.Port);
                }
                else
                {
                    ports = new List<int> { scanProperties.PortRangeStart.Port };
                }

                ScanConfiguration scanConfig = new ScanConfiguration(
                    ips,
                    ports,
                    scanProperties.Timeout.Timeout,
                    scanProperties.MaximumConcurrentScans.MaxThreadCount
                );

                logger.Log("Successfully created scan configuration");


            }

            //bool result = CreateScanConfiguration();
        }

        private void Stop_Click(object? sender, EventArgs e)
        {
            // Handle stop button click
            // Example: Stop scanning or processing
        }

        private void PauseResume_Click(object? sender, EventArgs e)
        {
            // Handle pause/resume button click
            // Example: Pause or resume scanning or processing
        }

        private void Clear_Click(object? sender, EventArgs e)
        {
            // Handle clear button click
            // Example: Clear the console or results
        }
    }
}
