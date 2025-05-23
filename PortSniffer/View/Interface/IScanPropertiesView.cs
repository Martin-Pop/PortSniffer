using PortSniffer.View.Abstract;
using PortSniffer.View.Controls;
using PortSniffer.View.ScanProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Interface
{
    /// <summary>
    /// Repesents the view for managing scan properties.
    /// </summary>
    public interface IScanPropertiesView
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

        ScanPropertyCheckBox OnlyWellKnownPorts { get; }

        /// <summary>
        /// Visually highlights validation error.
        /// </summary>
        /// <param name="property">Property to highlight</param>
        void HighlightValidationError(ScanPropertyInputAbstract property);

        /// <summary>
        /// Visually resets the validation error highlight.
        /// </summary>
        /// <param name="property">Property to highlight</param>
        void RemoveHighlightValidationError(ScanPropertyInputAbstract property);
    }
}
