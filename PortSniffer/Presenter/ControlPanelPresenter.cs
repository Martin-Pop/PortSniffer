using PortSniffer.Core.Interface;
using PortSniffer.Model;
using PortSniffer.View.Abstract;
using PortSniffer.View.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private void Start_Click(object? sender, EventArgs e)
        {
            if (!scanProperties.TargetIP.IsValid || !scanProperties.PortRangeStart.IsValid)
            {
                scanProperties.TargetIP.Error();
                scanProperties.PortRangeStart.Error();
                logger.Error("Can not strat scaning: Required properties are invalid");
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
