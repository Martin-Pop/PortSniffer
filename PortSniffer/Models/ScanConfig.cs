using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Models
{
    public class ScanConfig
    {
        private readonly List<IPAddress> addresses;
        private readonly List<int> ports;
        private readonly int timeOut;
        private readonly int maxThreads;

        public ScanConfig(List<IPAddress> addresses, List<int> ports, int timeOut, int maxThreads)
        {
            this.addresses = addresses;
            this.ports = ports;
            this.timeOut = timeOut;
            this.maxThreads = maxThreads;
        }

        public List<IPAddress> Addresses => addresses;
        public List<int> Ports => ports;
        public int TimeOut => timeOut;
        public int MaxThreads => maxThreads;
    }
}
