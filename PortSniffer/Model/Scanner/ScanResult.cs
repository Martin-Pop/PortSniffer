using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Model.Scanner
{
    public class ScanResult
    {
        public string IPAddress { get; private set; }
        public List<int> OpenPorts { get; private set; }

        public ScanResult(string iPAddress, List<int> openPorts)
        {
            IPAddress = iPAddress;
            OpenPorts = openPorts;
        }
    }
}
