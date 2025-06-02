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
        void AddScanResult(ScanResultProperty scanResultProperty);
        void ViewScanResult(ScanResult result);
        void UpdateScanProgress(ScanProgress progress);
        void ClearResults();
    }
}
