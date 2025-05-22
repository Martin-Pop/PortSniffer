using PortSniffer.View.Abstract;
using PortSniffer.View.ScanProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Interface
{
    /// <summary>
    /// Repesents the control panel view for managing properties.
    /// </summary>
    public interface IControlPanelView
    {
        /// <summary>
        /// Gets the target IP address property.
        /// </summary>
        IPAddressProperty TargetIP { get; }

        /// <summary>
        /// Gets the target IP range end property.
        /// </summary>
        IPAddressProperty TargetIPRangeEnd { get; }

        /// <summary>
        /// Gets the subnet mask property.
        /// </summary>
        IPAddressProperty SubnetMask { get; }

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
