using PortSniffer.Core;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace PortSniffer.Presenter
{
    public class ControlPanelPresenter
    {
        private readonly IControlPanelView controlPanelView;
        private readonly IScanProperties scanProperties;
        private readonly IConsoleLogger logger;

        private readonly PortScanner portScanner = new PortScanner();

        public ScaningState scanState {  get; private set; }

        public ControlPanelPresenter(IControlPanelView controlPanelView, IScanProperties scanProperties, IConsoleLogger logger, PortScanner portScanner)
        {
            this.controlPanelView = controlPanelView;
            this.scanProperties = scanProperties;
            this.logger = logger;
            this.portScanner = portScanner;

            // events
            controlPanelView.Start.Click += Start_Click;
            controlPanelView.Stop.Click += Stop_Click;
            controlPanelView.PauseResume.Click += PauseResume_Click;
            controlPanelView.Clear.Click += Clear_Click;

            portScanner.OpenPortsFoundEvent += (result) =>
            {
                logger.Log($"Open ports found for {result.IPAddress}: {string.Join(", ", result.OpenPorts)}");
            };
        }


        private ScanConfiguration CreateScanConfiguration()
        {
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

            return scanConfig;
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

        private async void Start_Click(object? sender, EventArgs e)
        {
            if (portScanner.ScanState == ScaningState.IDLE)
            { 
                if (CanStartScanning())
                {
                    ScanConfiguration scanConfiguration = CreateScanConfiguration();

                    long ops =  (long) scanConfiguration.Ports.Count * scanConfiguration.IPAddresses.Count * scanConfiguration.Timeout / scanConfiguration.MaxThreads;
                    var estimatedTime = TimeSpan.FromMilliseconds(ops);

                    if (estimatedTime.TotalHours > 1)
                    {
                        logger.Warn("The estimated scan time is over an hour. To speed it up, try increasing the 'Max Concurrent Scans' value or lowering the 'Timeout' setting.");
                    }

                    if (estimatedTime.TotalDays > 1)
                    {
                        logger.Warn("Your scan will take more than a day! This is not recomended!!");
                    }

                    //chatgpt helped with this D2 formating, - it means decimal, always 2 digits
                    logger.Log($"Estimated time for scan: {(int)estimatedTime.TotalHours:D2}:{estimatedTime.Minutes:D2}:{estimatedTime.Seconds:D2} minutes");
                    logger.Log("Starting Scanning at " + Utils.TimeNow());

                    controlPanelView.Start.Enabled = false;
                    controlPanelView.Stop.Enabled = true;
                    controlPanelView.PauseResume.Enabled = true;

                    await portScanner.StartScanAsync(scanConfiguration);

                    if (portScanner.ScanState == ScaningState.IDLE)
                    {
                        logger.Log("Scanning finished successfully");
                    }
                    else
                    {
                        logger.Log("Scanning has ended");
                    }

                    controlPanelView.Start.Enabled = true;
                    portScanner.ScanState = ScaningState.IDLE;
                }
            }
        }

        private void Stop_Click(object? sender, EventArgs e)
        {
            portScanner.CancelScan();

            controlPanelView.Start.Enabled = true;
            controlPanelView.Stop.Enabled = false;
            controlPanelView.PauseResume.Enabled = false;
            controlPanelView.PauseResume.Text = "Pause";

            logger.Log("Scanning manually stopped by user at " + Utils.TimeNow());
        }

        private void PauseResume_Click(object? sender, EventArgs e)
        {
            if (portScanner.ScanState == ScaningState.RUNNING)
            {
                portScanner.PauseScan();
                controlPanelView.PauseResume.Text = "Resume";
                logger.Log("Scanning paused at "+Utils.TimeNow());
            }
            else if (portScanner.ScanState == ScaningState.SUSPENDED)
            {
                portScanner.ResumeScan();
                controlPanelView.PauseResume.Text = "Pause";
                logger.Log("Scanning resumed at" +Utils.TimeNow());
            }
        }

        private void Clear_Click(object? sender, EventArgs e)
        {
           //TODO: make this x_x
        }
    }
}
