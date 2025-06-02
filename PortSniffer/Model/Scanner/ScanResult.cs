using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Model.Scanner
{
    /// <summary>
    /// Represents the result of a port scan on a specific IP address.
    /// </summary>
    public class ScanResult
    {
        public string IPAddress { get; private set; }
        public List<OpenPort> OpenPorts { get; private set; }
        public int ScannedPortRangeStart { get; private set; }
        public int ScannedPortRangeEnd { get; private set; }
        public int TotalScannedPorts { get; private set; }
        public int TotalOpenPorts => OpenPorts.Count;
        public DateOnly ScanDate { get; private set; }

        public ScanResult(string iPAddress, List<OpenPort> openPorts, int scannedPortRangeStart, int scannedPortRangeEnd, int totalScannedPorts, DateOnly scanDate)
        {
            IPAddress = iPAddress;
            OpenPorts = openPorts;
            ScannedPortRangeStart = scannedPortRangeStart;
            ScannedPortRangeEnd = scannedPortRangeEnd;
            TotalScannedPorts = totalScannedPorts;
            ScanDate = scanDate;
        }

        public override string ToString()
        {
            return $"Date: {ScanDate.ToString("dd.MM.yyyy")} \n" +
                $"IP Address: {IPAddress} \n" +
                $"Scanned ports: {ScannedPortRangeStart}-{ScannedPortRangeEnd} ({TotalScannedPorts} total) \n" +
                $"Open ports found ({TotalOpenPorts}): \n" +
                $"{string.Join("\n", OpenPorts)}"; 
        }
    }
}
