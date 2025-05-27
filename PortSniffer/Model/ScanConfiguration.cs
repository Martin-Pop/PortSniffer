using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Model
{
    public class ScanConfiguration
    {
        private readonly List<IPAddress> iPAddresses;
        private readonly List<int> ports;
        private readonly int timeout;
        private readonly int maxThreads;
        public ScanConfiguration(List<IPAddress> ips, List<int> ports, int timeout, int maxThreads) 
        {
            this.iPAddresses = ips;
            this.ports = ports;
            this.timeout = timeout;
            this.maxThreads = maxThreads;
        }
    }
}
