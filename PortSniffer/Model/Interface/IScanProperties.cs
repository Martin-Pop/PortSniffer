using PortSniffer.View.Controls;
using PortSniffer.View.Properties;
using PortSniffer.View.ScanProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Model.Interface
{
    public interface IScanProperties
    {
        /// <summary>
        /// Property for Target IP.
        /// </summary>
        IPAddressProperty TargetIP { get; }

        /// <summary>
        /// Property for Target IP range end.
        /// </summary>
        IPAddressProperty TargetIPRangeEnd { get; }

        /// <summary>
        /// Property for Subnet mask
        /// </summary>
        IPAddressProperty SubnetMask { get; }

        /// <summary>
        /// Property for port range start.
        /// </summary>
        PortProperty PortRangeStart { get; }

        /// <summary>
        /// Property for port range end.
        /// </summary>
        PortProperty PortRangeEnd { get; }

        /// <summary>
        /// Property for maximum threads for scanning. 
        /// </summary>
        MaxConcurrentProperty MaximumConcurrentScans { get; }

        /// <summary>
        /// Timeout for each scan.
        /// </summary>
        TimeoutProperty Timeout { get; }

        /// <summary>
        /// Option for only well-known ports.
        /// </summary>
        PredefinedPortsProperty OnlyWellKnownPorts { get; }

        /// <summary>
        /// Option for only registered ports.
        /// </summary>
        PredefinedPortsProperty OnlyRegisteredPorts { get; }

        /// <summary>
        /// Option for only private ports.
        /// </summary>
        PredefinedPortsProperty OnlyPrivatePorts { get; }

        /// <summary>
        /// Option for all ports.
        /// </summary>
        PredefinedPortsProperty AllPorts { get; }

        /// <summary>
        /// Clears all scan properties to their default values.
        /// </summary>
        void ClearAll();
    }
}
