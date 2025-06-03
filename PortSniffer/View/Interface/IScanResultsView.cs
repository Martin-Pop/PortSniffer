using PortSniffer.Model.Scanner;
using PortSniffer.View.ScanResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Interface
{
    /// <summary>
    /// Interface for scan results view.
    /// </summary>
    public interface IScanResultsView
    {
        /// <summary>
        /// Adds a scan result to the view.
        /// </summary>
        /// <param name="scanResultProperty"></param>
        void AddScanResult(ScanResultProperty scanResultProperty);

        /// <summary>
        /// Views the scan result in detail.
        /// </summary>
        /// <param name="result"></param>
        void ViewScanResult(ScanResult result);

        /// <summary>
        /// Updates the scan progress in the view.
        /// </summary>
        /// <param name="progress"></param>
        void UpdateScanProgress(ScanProgress progress);

        /// <summary>
        /// Export button.
        /// </summary>
        public Button ExportButton { get; }

        /// <summary>
        /// Gets all the scan results.
        /// </summary>
        /// <returns></returns>
        List<ScanResult> GetScanResults();

        /// <summary>
        /// Clears all results.
        /// </summary>
        void ClearResults();
    }
}
