﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Model.Scanner
{
    /// <summary>
    /// Represents the configuration for a port scanning.
    /// </summary>
    public class ScanConfiguration
    {
        private readonly List<IPAddress> iPAddresses;
        private readonly List<int> ports;
        private readonly int timeout;
        private readonly int maxThreads;
        public ScanConfiguration(List<IPAddress> ips, List<int> ports, int timeout, int maxThreads)
        {
            iPAddresses = ips;
            this.ports = ports;
            this.timeout = timeout;
            this.maxThreads = maxThreads;
        }

        public List<IPAddress> IPAddresses => iPAddresses;
        public List<int> Ports => ports;
        public int Timeout => timeout;
        public int MaxThreads => maxThreads;
    }
}
