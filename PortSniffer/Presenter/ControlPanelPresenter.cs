using PortSniffer.Core;
using PortSniffer.Model;
using PortSniffer.Model.Interface;
using PortSniffer.Model.Scanner;
using PortSniffer.View.Interface;
using System.Diagnostics;
using System.Net;

namespace PortSniffer.Presenter
{
    /// <summary>
    /// Presenter for the control panel
    /// </summary>
    public class ControlPanelPresenter
    {
        private readonly IControlPanelView controlPanelView;
        private readonly IScanProperties scanProperties;
        private readonly IConsoleLogger logger;
        private readonly IScanResultsView scanResultsView;
        private readonly PortScanner portScanner;
        public ScaningState scanState {  get; private set; }

        public ControlPanelPresenter(IControlPanelView controlPanelView, IScanProperties scanProperties, IConsoleLogger logger,IScanResultsView scanResultsView, PortScanner portScanner)
        {
            this.controlPanelView = controlPanelView;
            this.scanProperties = scanProperties;
            this.logger = logger;
            this.scanResultsView = scanResultsView;
            this.portScanner = portScanner;

            // events
            controlPanelView.Start.Click += Start_Click;
            controlPanelView.Stop.Click += Stop_Click;
            controlPanelView.PauseResume.Click += PauseResume_Click;
            controlPanelView.Clear.Click += Clear_Click;
        }

        /// <summary>
        /// Creates a scan configuration based on the properties set in the scanProperties object, should be called only if required properties are valid.
        /// </summary>
        /// <returns>The scan configuration</returns>
        private ScanConfiguration CreateScanConfiguration()
        {
            IPAddress[] range = new IPAddress[2];

            if (scanProperties.SubnetMask.IsValid)
            {
                range = Validation.MaskToRange(scanProperties.TargetIP.IpAddress, scanProperties.SubnetMask.IpAddress);
            }
            else
            {
                range[0] = scanProperties.TargetIP.IpAddress;
                range[1] = scanProperties.TargetIPRangeEnd.IpAddress;
            }

            List<IPAddress> ips = new List<IPAddress>();
            if (scanProperties.TargetIPRangeEnd.IsValid || scanProperties.SubnetMask.IsValid)
            {
                ips = Validation.GetIPAddressesFromRange(range[0], range[1]);
            }
            else
            {
                ips = new List<IPAddress>() { range[0] };
            }

            // Ports
            List<int> ports = new List<int>();

            if (scanProperties.PortRangeEnd.IsValid)
            {
                ports = Validation.RangeToPortList(scanProperties.PortRangeStart.Port, scanProperties.PortRangeEnd.Port);
            }
            else
            {
                ports = new List<int> { scanProperties.PortRangeStart.Port };
            }

            //resets to default if it was invalid
            if (!scanProperties.SubnetMask.IsValid) scanProperties.SubnetMask.Reset();
            if (!scanProperties.PortRangeEnd.IsValid) scanProperties.PortRangeEnd.Reset();
            if (!scanProperties.TargetIPRangeEnd.IsValid) scanProperties.TargetIPRangeEnd.Reset();
            if (!scanProperties.Timeout.IsValid) scanProperties.Timeout.Reset();
            if (!scanProperties.MaximumConcurrentScans.IsValid) scanProperties.MaximumConcurrentScans.Reset();

            ScanConfiguration scanConfig = new ScanConfiguration(
                ips,
                ports,
                scanProperties.Timeout.Timeout,
                scanProperties.MaximumConcurrentScans.MaxThreadCount
            );

            return scanConfig;
        }

        /// <summary>
        /// Checks if the required properties for starting a scan are valid.
        /// </summary>
        /// <returns>True if conditions are met, otherwise false</returns>
        private bool CanStartScanning()
        {
            if(scanProperties.TargetIP.IsValid && scanProperties.PortRangeStart.IsValid)
            {
                return true;
            }
            else
            {
                if (!scanProperties.TargetIP.IsValid) scanProperties.TargetIP.Error();
                if (!scanProperties.PortRangeStart.IsValid) scanProperties.PortRangeStart.Error();
                logger.Error("Can not start scaning: Required properties are invalid");
                return false;
            }
        }

        /// <summary>
        /// Handles the click event for the start button, creates scan configuration and starts the scanning process if conditions are met.
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event arguments</param>
        private async void Start_Click(object? sender, EventArgs e)
        {
            if (portScanner.ScanState == ScaningState.FINISHED || portScanner.ScanState == ScaningState.CANCELED)
            {
                scanResultsView.ClearResults();
                portScanner.ScanState = ScaningState.IDLE; 
            }

            if (portScanner.ScanState == ScaningState.IDLE)
            { 
                if (CanStartScanning())
                {
                    ScanConfiguration scanConfiguration = CreateScanConfiguration();

                    double batch = Math.Ceiling((double)scanConfiguration.Ports.Count / scanConfiguration.MaxThreads);
                    var estimatedTime = TimeSpan.FromMilliseconds(batch  * scanConfiguration.Timeout * scanConfiguration.IPAddresses.Count);
                        
                    if (estimatedTime.TotalHours > 1)
                    {
                        logger.Warn("The estimated scan time is over an hour. To speed it up, try increasing the 'Max Concurrent Scans' value or lowering the 'Timeout' setting.");
                    }

                    if (estimatedTime.TotalDays > 1)
                    {
                        logger.Warn("Your scan will take more than a day! This is not recomended!!");
                    }

                    //chatgpt helped with this D2 formating, - it means decimal, always 2 digits
                    logger.Log($"Estimated time for scan: {(int)estimatedTime.TotalHours:D2}:{estimatedTime.Minutes:D2}:{estimatedTime.Seconds:D2}");
                    logger.Log("Starting Scanning at " + Utils.TimeNow());
                    
                    UpdateButtons(ScaningState.RUNNING);

                    await portScanner.StartScanAsync(scanConfiguration);

                    if (portScanner.ScanState == ScaningState.FINISHED)
                    {
                        logger.Log("Scanning finished successfully");
                    }
                    else if (portScanner.ScanState == ScaningState.CANCELED)
                    {
                        logger.Log("Scanning was canceled by user");
                    }

                    UpdateButtons(ScaningState.FINISHED);
                }
            }
        }

        /// <summary>
        /// Hanlder for clicking the stop button, cancels the ongoing scan immediately.
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event arguments</param>
        private void Stop_Click(object? sender, EventArgs e)
        {
            portScanner.CancelScan();
            UpdateButtons(ScaningState.IDLE);
            logger.Log("Scanning manually stopped by user at " + Utils.TimeNow());
        }

        /// <summary>
        /// Handler for clicking the pause/resume button, pauses or resumes the scanning process based on the current state.
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event arguments</param>
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
                logger.Log("Scanning resumed at " +Utils.TimeNow());
            }
        }

        /// <summary>
        /// Handler for clicking the clear button, clears the logger and resets scan properties.
        /// </summary>
        /// <param name="sender">Source of the evetn</param>
        /// <param name="e">Event arguments</param>
        private void Clear_Click(object? sender, EventArgs e)
        {
            scanProperties.ClearAll();
            logger.CLear();
            scanResultsView.ClearResults();
            UpdateButtons(ScaningState.IDLE);

            portScanner.ScanState = ScaningState.IDLE;
        }

        /// <summary>
        /// Updates buttons based on the current scanning state.
        /// </summary>
        /// <param name="state">State to use</param>
        private void UpdateButtons(ScaningState state)
        {
            if (state == ScaningState.RUNNING) 
            {
                controlPanelView.Start.Enabled = false;
                controlPanelView.Stop.Enabled = true;
                controlPanelView.PauseResume.Enabled = true;
                controlPanelView.PauseResume.Text = "Pause";
                controlPanelView.Clear.Enabled = false;
            }
            else if (state == ScaningState.IDLE || state == ScaningState.FINISHED)
            {
                controlPanelView.Start.Enabled = true;
                controlPanelView.Stop.Enabled = false;
                controlPanelView.PauseResume.Enabled = false;
                controlPanelView.PauseResume.Text = "Pause";
                controlPanelView.Clear.Enabled = true;
            }
        }
    }
}
