using PortSniffer.Model.Config;
using PortSniffer.Model.Scanner;
using PortSniffer.View.Interface;
using PortSniffer.View.ScanResults;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortSniffer.Presenter
{
    /// <summary>
    /// Manages the interaction between the port scanner and the scan results view.
    /// </summary>
    public class ScanResultsPresenter
    {
        private readonly PortScanner portScanner;
        private readonly IScanResultsView scanResultsView;
        private readonly IConsoleLogger logger;
        private readonly Settings settings;

        //this should be in model..
        private List<ScanResult> scanResults = new List<ScanResult>();

        public ScanResultsPresenter(PortScanner portScanner, IScanResultsView scanResultsView, IConsoleLogger logger ,Settings settings)
        {
            this.portScanner = portScanner;
            this.scanResultsView = scanResultsView;
            this.logger = logger;
            this.settings = settings;

            this.portScanner.OpenPortsFoundEvent += OnOpenPortsFoundEvent;
            this.portScanner.ScanProgressEvent += OnScanProgressUpdate;

            scanResultsView.ExportButton.Click += ExportButton_Click;
        }

        private void ExportButton_Click(object? sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "JSON Files (*.json)|*.json";
                saveDialog.Title = "Save Scan Results";
                saveDialog.DefaultExt = "json";
                saveDialog.FileName = "scan_results.json";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string json = JsonSerializer.Serialize(scanResults, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(saveDialog.FileName, json);
                        logger.Log("Scan results saved successfully!");
                    }
                    catch (Exception ex)
                    {
                        logger.Warn($"Error saving file: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// When scanner finds open ports on the address it gets added to view.
        /// </summary>
        /// <param name="result"></param>
        private void OnOpenPortsFoundEvent(ScanResult result)
        {
            scanResults.Add(result);
            ScanResultProperty scanResultProperty = new ScanResultProperty(settings, result);
            scanResultProperty.ScanResultSelected += ScanResultProperty_ScanResultSelected;

            scanResultsView.AddScanResult(scanResultProperty);
        }

        /// <summary>
        /// When scan result is selected, view displays the details of the scan result.
        /// </summary>
        /// <param name="result"></param>
        private void ScanResultProperty_ScanResultSelected(ScanResult result)
        {
            scanResultsView.ViewScanResult(result);
        }

        /// <summary>
        /// Updates the scan progress in the view as the scan progresses.
        /// </summary>
        /// <param name="progress"></param>
        private void OnScanProgressUpdate(ScanProgress progress)
        {
            scanResultsView.UpdateScanProgress(progress);
        }
    }
}
