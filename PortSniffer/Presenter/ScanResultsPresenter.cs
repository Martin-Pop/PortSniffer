using PortSniffer.Model.Config;
using PortSniffer.Model.Scanner;
using PortSniffer.View.Interface;
using PortSniffer.View.ScanResults;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Presenter
{
    public class ScanResultsPresenter
    {
        private readonly PortScanner portScanner;
        private readonly IScanResultsView scanResultsView;
        private readonly Settings settings;

        public ScanResultsPresenter(PortScanner portScanner, IScanResultsView scanResultsView, Settings settings)
        {
            this.portScanner = portScanner;
            this.scanResultsView = scanResultsView;
            this.settings = settings;

            this.portScanner.OpenPortsFoundEvent += OnOpenPortsFoundEvent;
            this.portScanner.ScanProgressEvent += OnScanProgressUpdate;
        }

        private void OnOpenPortsFoundEvent(ScanResult result)
        {
            //Debug.WriteLine("------------------------------------");
            //Debug.WriteLine(portScanner.Settings);
            ScanResultProperty scanResultProperty = new ScanResultProperty(settings, result);
            scanResultProperty.ScanResultSelected += ScanResultProperty_ScanResultSelected;

            scanResultsView.AddScanResult(scanResultProperty);
        }

        private void ScanResultProperty_ScanResultSelected(ScanResult result)
        {
            scanResultsView.ViewScanResult(result);
        }

        private void OnScanProgressUpdate(ScanProgress progress)
        {
            scanResultsView.UpdateScanProgress(progress);
        }
    }
}
