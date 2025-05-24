using PortSniffer.Core.Interface;
using PortSniffer.Model;
using PortSniffer.View.Interface;
using PortSniffer.View.Properties;
using PortSniffer.View.Sections;
using System.Net;

namespace PortSniffer.Presenter
{
    public class ScanPropertiesPresenter
    {
        private readonly IScanPropertiesView scanPropertiesView;
        private readonly IConsoleLogger logger;
        public ScanPropertiesPresenter(IScanPropertiesView view, IConsoleLogger logger)
        {
            this.logger = logger;
            scanPropertiesView = view;

            //events
            scanPropertiesView.TargetIP.ValidationEvent += OnTargetIPValidation;
            scanPropertiesView.TargetIPRangeEnd.ValidationEvent += OnTargetIPRangeEndValidation;
            scanPropertiesView.SubnetMask.ValidationEvent += OnSubnetMaskValidation;
            scanPropertiesView.PortRangeStart.ValidationEvent += OnPortRangeStartValidation;
            scanPropertiesView.PortRangeEnd.ValidationEvent += OnPortRangeEndValidation;
            scanPropertiesView.MaximumConcurrentScans.ValidationEvent += OnMaximumConcurrentScansValidation;
            scanPropertiesView.Timeout.ValidationEvent += OnTimeoutValidation;
            scanPropertiesView.OnlyWellKnownPorts.StateChangedEvent += OnPredefinedPortsChanged;
            scanPropertiesView.OnlyRegisteredPorts.StateChangedEvent += OnPredefinedPortsChanged;
            scanPropertiesView.OnlyPrivatePorts.StateChangedEvent += OnPredefinedPortsChanged;
            scanPropertiesView.AllPorts.StateChangedEvent += OnPredefinedPortsChanged;
        }

        /// <summary>
        /// Validates the IP address in the 'target IP range end' control field.
        /// </summary>
        private void ValidateIPTargetRangeEnd()
        {
            if (IPValidator.ValidateIP(scanPropertiesView.TargetIPRangeEnd.Input.Text, out IPAddress? ip))
            {
                if (IPValidator.IsGreaterThan(ip!, scanPropertiesView.TargetIP.IpAddress))
                {
                    scanPropertiesView.TargetIPRangeEnd.IpAddress = ip!;
                    scanPropertiesView.TargetIPRangeEnd.Input.Text = ip!.ToString();
                    scanPropertiesView.TargetIPRangeEnd.IsValid = true;
                    logger.Log($"Successfully set IP adress range-end to {scanPropertiesView.TargetIPRangeEnd.Input.Text}");
                    return;
                }
                else
                {
                    logger.Error($"IP address range-end \"{ip}\" must be greater than \"{scanPropertiesView.TargetIP.Input.Text}\"");
                    scanPropertiesView.TargetIPRangeEnd.Input.Text = ip!.ToString();
                }
            }
            else
            {
                logger.Error($"IP address range-end \"{scanPropertiesView.TargetIPRangeEnd.Input.Text}\" is not in the valid IPv4 format");
            }

            scanPropertiesView.HighlightValidationError(scanPropertiesView.TargetIPRangeEnd);
            scanPropertiesView.TargetIPRangeEnd.IsValid = false;
        }

        /// <summary>
        /// Validates the port range-end.
        /// </summary>
        private void ValidatePortRangeEnd()
        {
            if (int.TryParse(scanPropertiesView.PortRangeEnd.Input.Text, out int port) && port > 0 && port <= 65535)
            {
                if (scanPropertiesView.PortRangeStart.Port < port)
                {
                    scanPropertiesView.PortRangeEnd.Port = port;
                    scanPropertiesView.PortRangeEnd.IsValid = true;
                    logger.Log($"Successfully set port range-end to {scanPropertiesView.PortRangeEnd.Input.Text}");
                    return;
                }
                else
                {
                    logger.Error($"Port range-end \"{scanPropertiesView.PortRangeEnd.Input.Text}\" must be greater than {scanPropertiesView.PortRangeStart.Port} (start)");
                }
            }
            else
            {
                logger.Error($"Port range-end \"{scanPropertiesView.PortRangeEnd.Input.Text}\" must be a number within the range of 1 - 65535");
            }
            scanPropertiesView.HighlightValidationError(scanPropertiesView.PortRangeEnd);
            scanPropertiesView.PortRangeEnd.IsValid = false;
        }


        /// <summary>
        /// Handler for the target IP validation event.
        /// if the input is empty, it resets the field. Otherwise, it validates the target IP
        /// and revalidates the end range if it was provided
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event arguments</param>
        private void OnTargetIPValidation(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(scanPropertiesView.TargetIP.Input.Text.Trim()))
            {
                scanPropertiesView.TargetIP.Reset();
                scanPropertiesView.TargetIPRangeEnd.Reset();
            }
            else
            {
                if (IPValidator.ValidateIP(scanPropertiesView.TargetIP.Input.Text, out IPAddress? ip))
                {
                    scanPropertiesView.TargetIP.IpAddress = ip!;
                    scanPropertiesView.TargetIP.Input.Text = ip!.ToString();
                    scanPropertiesView.TargetIP.IsValid = true;
                    logger.Log($"Successfully set IP adress target to {scanPropertiesView.TargetIP.Input.Text}");

                    if (!string.IsNullOrWhiteSpace(scanPropertiesView.TargetIPRangeEnd.Input.Text)) //revalidate if there was something
                    {
                        scanPropertiesView.RemoveHighlightValidationError(scanPropertiesView.TargetIPRangeEnd);
                        ValidateIPTargetRangeEnd();
                    }
                }
                else
                {
                    logger.Error($"IP address \"{scanPropertiesView.TargetIP.Input.Text}\" is not in the valid IPv4 format");
                    scanPropertiesView.HighlightValidationError(scanPropertiesView.TargetIP);
                    scanPropertiesView.TargetIP.IsValid = false;

                    if (scanPropertiesView.TargetIPRangeEnd.IsValid)
                    {
                        scanPropertiesView.HighlightValidationError(scanPropertiesView.TargetIPRangeEnd);
                        scanPropertiesView.TargetIPRangeEnd.IsValid = false;
                    }
                }
            }
        }

        /// <summary>
        /// Handler for the target 'IP range end' validation event.
        /// If the input is empty, it resets the field. Otherwise, it validates the 'range end' and resets mask
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event arguments</param>
        private void OnTargetIPRangeEndValidation(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(scanPropertiesView.TargetIPRangeEnd.Input.Text.Trim()))
            {
                scanPropertiesView.TargetIPRangeEnd.Reset();
            }
            else
            {
                if (scanPropertiesView.TargetIP.IsValid)
                {
                    ValidateIPTargetRangeEnd();
                    scanPropertiesView.SubnetMask.Reset();
                }
                else
                {
                    logger.Warn($"Please set valid IP Target before setting the range-end");
                    scanPropertiesView.HighlightValidationError(scanPropertiesView.TargetIPRangeEnd);
                    scanPropertiesView.TargetIPRangeEnd.IsValid = false;
                }
            }
        }

        /// <summary>
        /// Handler for the subnet mask validation event.
        /// If the input is empty, it resets the field. Otherwise, it validates the mask
        /// and resets the target IP range end field.
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event arguments</param>
        private void OnSubnetMaskValidation(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(scanPropertiesView.SubnetMask.Input.Text.Trim()))
            {
                scanPropertiesView.SubnetMask.Reset();
            }
            else
            {
                if (IPValidator.ValidateSubnetMask(scanPropertiesView.SubnetMask.Input.Text, out IPAddress? mask))
                {
                    scanPropertiesView.SubnetMask.IpAddress = mask!;
                    scanPropertiesView.SubnetMask.Input.Text = mask!.ToString();
                    scanPropertiesView.SubnetMask.IsValid = true;
                    logger.Log($"Successfully set subnet mask to {scanPropertiesView.SubnetMask.Input.Text}");
                }
                else
                {
                    logger.Error($"Subnet mask \"{scanPropertiesView.SubnetMask.Input.Text}\" is not valid IPv4 mask, try using CIDR format");
                    scanPropertiesView.HighlightValidationError(scanPropertiesView.SubnetMask);
                    scanPropertiesView.SubnetMask.IsValid = false;
                }
                scanPropertiesView.TargetIPRangeEnd.Reset();
            }
        }

        /// <summary>
        /// Handler for the port range-start validation event.
        /// If the input is empty, it resets the field. Otherwise, it validates the port range-start
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event arguments</param>
        private void OnPortRangeStartValidation(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(scanPropertiesView.PortRangeStart.Input.Text.Trim()))
            {
                scanPropertiesView.PortRangeStart.Reset();
            }
            else
            {
                if (int.TryParse(scanPropertiesView.PortRangeStart.Input.Text, out int port) && port > 0 && port <= 65535)
                {
                    scanPropertiesView.PortRangeStart.Port = port;
                    scanPropertiesView.PortRangeStart.IsValid = true;
                    logger.Log($"Successfully set port range-start to {scanPropertiesView.PortRangeStart.Input.Text}");

                    if (!string.IsNullOrWhiteSpace(scanPropertiesView.PortRangeEnd.Input.Text)) //revalidate if there was something
                    {
                        scanPropertiesView.RemoveHighlightValidationError(scanPropertiesView.PortRangeEnd);
                        ValidatePortRangeEnd();
                    }
                }
                else
                {
                    logger.Error($"Port range start \"{scanPropertiesView.PortRangeStart.Input.Text}\" must be a number within the range of 1 - 65535");
                    scanPropertiesView.HighlightValidationError(scanPropertiesView.PortRangeStart);
                    scanPropertiesView.PortRangeStart.IsValid = false;

                    if (scanPropertiesView.PortRangeEnd.IsValid)
                    {
                        scanPropertiesView.HighlightValidationError(scanPropertiesView.PortRangeEnd);
                        scanPropertiesView.PortRangeEnd.IsValid = false;
                    }
                }  
            }
        }

        /// <summary>
        /// Handler for the port range-end validation event.
        /// If the input is empty, it resets the field. Otherwise, it validates the port range-end
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event arguments</param>
        private void OnPortRangeEndValidation(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(scanPropertiesView.PortRangeEnd.Input.Text.Trim()))
            {
                scanPropertiesView.PortRangeEnd.Reset();
            }
            else
            {
                if (scanPropertiesView.PortRangeStart.IsValid)
                {
                    ValidatePortRangeEnd();
                }
                else
                {
                    logger.Warn($"Please set valid port range-start before setting the range-end");
                    scanPropertiesView.HighlightValidationError(scanPropertiesView.PortRangeEnd);
                    scanPropertiesView.PortRangeEnd.IsValid = false;
                }     
            }
        }

        /// <summary>
        /// Hanler for the maximum concurrent scans validation event.
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event arguments</param>
        private void OnMaximumConcurrentScansValidation(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(scanPropertiesView.MaximumConcurrentScans.Input.Text.Trim()))
            {
                scanPropertiesView.MaximumConcurrentScans.Reset();
            }
            else
            {
                if (int.TryParse(scanPropertiesView.MaximumConcurrentScans.Input.Text, out int max) && max > 0)
                {
                    scanPropertiesView.MaximumConcurrentScans.MaxThreadCount = max;
                    scanPropertiesView.MaximumConcurrentScans.IsValid = true;
                    logger.Log($"Successfully set maximum concurrent scans to {scanPropertiesView.MaximumConcurrentScans.Input.Text}");
                }
                else
                {
                    logger.Error($"Maximum concurrent scans \"{scanPropertiesView.MaximumConcurrentScans.Input.Text}\" must be a number greater than 0");
                    scanPropertiesView.HighlightValidationError(scanPropertiesView.MaximumConcurrentScans);
                    scanPropertiesView.MaximumConcurrentScans.IsValid = false;
                }
            }
        }

        /// <summary>
        /// Handler for the timeout validation event.
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event arguments</param>
        private void OnTimeoutValidation(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(scanPropertiesView.Timeout.Input.Text.Trim()))
            {
                scanPropertiesView.Timeout.Reset();
            }
            else
            {
                if (int.TryParse(scanPropertiesView.Timeout.Input.Text, out int timeout) && timeout > 49)
                {
                    scanPropertiesView.Timeout.Timeout = timeout;
                    scanPropertiesView.Timeout.IsValid = true;
                    logger.Log($"Successfully set timeout to {scanPropertiesView.Timeout.Input.Text}");
                }
                else
                {
                    logger.Error($"Timeout \"{scanPropertiesView.Timeout.Input.Text}\" must be a number (50+)");
                    scanPropertiesView.HighlightValidationError(scanPropertiesView.Timeout);
                    scanPropertiesView.Timeout.IsValid = false;
                }
            }
        }

        private void OnPredefinedPortsChanged(object? sender, EventArgs e)
        {
            if (sender is not PredefinedPortsProperty property) return;

            if (property.Input.Checked)
            {
                if (scanPropertiesView.OnlyWellKnownPorts.Input.Checked && !(scanPropertiesView.OnlyWellKnownPorts == property)) scanPropertiesView.OnlyWellKnownPorts.Input.Checked = false;
                if (scanPropertiesView.OnlyPrivatePorts.Input.Checked && !(scanPropertiesView.OnlyPrivatePorts == property)) scanPropertiesView.OnlyPrivatePorts.Input.Checked = false;
                if (scanPropertiesView.OnlyRegisteredPorts.Input.Checked && !(scanPropertiesView.OnlyRegisteredPorts == property)) scanPropertiesView.OnlyRegisteredPorts.Input.Checked = false;
                if (scanPropertiesView.AllPorts.Input.Checked && !(scanPropertiesView.AllPorts == property)) scanPropertiesView.AllPorts.Input.Checked = false;

                scanPropertiesView.PortRangeStart.Reset();
                scanPropertiesView.PortRangeEnd.Reset();
                scanPropertiesView.PortRangeStart.Input.Text = property.PredefinedPortsStart.ToString();
                scanPropertiesView.PortRangeEnd.Input.Text = property.PredefinedPortsEnd.ToString();
                scanPropertiesView.PortRangeStart.IsValid = true;
                scanPropertiesView.PortRangeEnd.IsValid = true;
                scanPropertiesView.PortRangeStart.Input.Enabled = false;
                scanPropertiesView.PortRangeEnd.Input.Enabled = false;
                logger.Log($"Enabled predefined ports: {property.PredefinedPortsStart} - {property.PredefinedPortsEnd}");
            }
            else
            {
                scanPropertiesView.PortRangeStart.Input.Enabled = true;
                scanPropertiesView.PortRangeEnd.Input.Enabled = true;
                scanPropertiesView.PortRangeStart.Reset();
                scanPropertiesView.PortRangeEnd.Reset();
                logger.Log($"Disabled predefined ports, custom range will be used");
            }
        }
    }
}
