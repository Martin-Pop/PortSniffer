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
        private readonly IScanProperties scanProperties;
        private readonly IConsoleLogger logger;
        public ScanPropertiesPresenter(IScanProperties properties, IConsoleLogger logger)
        {
            scanProperties = properties;
            this.logger = logger;

            //events
            scanProperties.TargetIP.ValidationEvent += OnTargetIPValidation;
            scanProperties.TargetIPRangeEnd.ValidationEvent += OnTargetIPRangeEndValidation;
            scanProperties.SubnetMask.ValidationEvent += OnSubnetMaskValidation;
            scanProperties.PortRangeStart.ValidationEvent += OnPortRangeStartValidation;
            scanProperties.PortRangeEnd.ValidationEvent += OnPortRangeEndValidation;
            scanProperties.MaximumConcurrentScans.ValidationEvent += OnMaximumConcurrentScansValidation;
            scanProperties.Timeout.ValidationEvent += OnTimeoutValidation;
            scanProperties.OnlyWellKnownPorts.StateChangedEvent += OnPredefinedPortsChanged;
            scanProperties.OnlyRegisteredPorts.StateChangedEvent += OnPredefinedPortsChanged;
            scanProperties.OnlyPrivatePorts.StateChangedEvent += OnPredefinedPortsChanged;
            scanProperties.AllPorts.StateChangedEvent += OnPredefinedPortsChanged;
        }

        /// <summary>
        /// Validates the IP address in the 'target IP range end' control field.
        /// </summary>
        private void ValidateIPTargetRangeEnd()
        {
            if (IPUtilities.ValidateIP(scanProperties.TargetIPRangeEnd.Input.Text, out IPAddress? ip))
            {
                if (IPUtilities.IsGreaterThan(ip!, scanProperties.TargetIP.IpAddress))
                {
                    scanProperties.TargetIPRangeEnd.IpAddress = ip!;
                    scanProperties.TargetIPRangeEnd.Input.Text = ip!.ToString();
                    scanProperties.TargetIPRangeEnd.IsValid = true;
                    logger.Log($"Successfully set IP adress range-end to {scanProperties.TargetIPRangeEnd.Input.Text}");
                    return;
                }
                else
                {
                    logger.Error($"IP address range-end \"{ip}\" must be greater than \"{scanProperties.TargetIP.Input.Text}\"");
                    scanProperties.TargetIPRangeEnd.Input.Text = ip!.ToString();
                }
            }
            else
            {
                logger.Error($"IP address range-end \"{scanProperties.TargetIPRangeEnd.Input.Text}\" is not in the valid IPv4 format");
            }
            scanProperties.TargetIPRangeEnd.Error();
        }

        /// <summary>
        /// Validates the port range-end.
        /// </summary>
        private void ValidatePortRangeEnd()
        {
            if (int.TryParse(scanProperties.PortRangeEnd.Input.Text, out int port) && port > 0 && port <= 65535)
            {
                if (scanProperties.PortRangeStart.Port < port)
                {
                    scanProperties.PortRangeEnd.Port = port;
                    scanProperties.PortRangeEnd.IsValid = true;
                    logger.Log($"Successfully set port range-end to {scanProperties.PortRangeEnd.Input.Text}");
                    return;
                }
                else
                {
                    logger.Error($"Port range-end \"{scanProperties.PortRangeEnd.Input.Text}\" must be greater than {scanProperties.PortRangeStart.Port} (start)");
                }
            }
            else
            {
                logger.Error($"Port range-end \"{scanProperties.PortRangeEnd.Input.Text}\" must be a number within the range of 1 - 65535");
            }
            //TODO FIX THIS + make new method for 'valid' option
            scanProperties.PortRangeEnd.Error();
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
            if (string.IsNullOrEmpty(scanProperties.TargetIP.Input.Text.Trim()))
            {
                scanProperties.TargetIP.Reset();
                scanProperties.TargetIPRangeEnd.Reset();
            }
            else
            {
                if (IPUtilities.ValidateIP(scanProperties.TargetIP.Input.Text, out IPAddress? ip))
                {
                    scanProperties.TargetIP.IpAddress = ip!;
                    scanProperties.TargetIP.Input.Text = ip!.ToString();
                    scanProperties.TargetIP.IsValid = true;
                    logger.Log($"Successfully set IP adress target to {scanProperties.TargetIP.Input.Text}");

                    if (!string.IsNullOrWhiteSpace(scanProperties.TargetIPRangeEnd.Input.Text)) //revalidate if there was something
                    {
                        scanProperties.TargetIPRangeEnd.RemovetValidationError();
                        ValidateIPTargetRangeEnd();
                    }
                }
                else
                {
                    logger.Error($"IP address \"{scanProperties.TargetIP.Input.Text}\" is not in the valid IPv4 format");
                    scanProperties.TargetIP.Error();

                    if (scanProperties.TargetIPRangeEnd.IsValid)
                    {
                        scanProperties.TargetIPRangeEnd.Error();
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
            if (string.IsNullOrEmpty(scanProperties.TargetIPRangeEnd.Input.Text.Trim()))
            {
                scanProperties.TargetIPRangeEnd.Reset();
            }
            else
            {
                if (scanProperties.TargetIP.IsValid)
                {
                    ValidateIPTargetRangeEnd();
                    scanProperties.SubnetMask.Reset();
                }
                else
                {
                    logger.Warn($"Please set valid IP Target before setting the range-end");
                    scanProperties.TargetIPRangeEnd.Error();
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
            if (string.IsNullOrEmpty(scanProperties.SubnetMask.Input.Text.Trim()))
            {
                scanProperties.SubnetMask.Reset();
            }
            else
            {
                if (IPUtilities.ValidateSubnetMask(scanProperties.SubnetMask.Input.Text, out IPAddress? mask))
                {
                    scanProperties.SubnetMask.IpAddress = mask!;
                    scanProperties.SubnetMask.Input.Text = mask!.ToString();
                    scanProperties.SubnetMask.IsValid = true;
                    logger.Log($"Successfully set subnet mask to {scanProperties.SubnetMask.Input.Text}");
                }
                else
                {
                    logger.Error($"Subnet mask \"{scanProperties.SubnetMask.Input.Text}\" is not valid IPv4 mask, try using CIDR format");
                    scanProperties.SubnetMask.Error();
                }
                scanProperties.TargetIPRangeEnd.Reset();
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
            if (string.IsNullOrEmpty(scanProperties.PortRangeStart.Input.Text.Trim()))
            {
                scanProperties.PortRangeStart.Reset();
            }
            else
            {
                if (int.TryParse(scanProperties.PortRangeStart.Input.Text, out int port) && port > 0 && port <= 65535)
                {
                    scanProperties.PortRangeStart.Port = port;
                    scanProperties.PortRangeStart.IsValid = true;
                    logger.Log($"Successfully set port range-start to {scanProperties.PortRangeStart.Input.Text}");

                    if (!string.IsNullOrWhiteSpace(scanProperties.PortRangeEnd.Input.Text)) //revalidate if there was something
                    {
                        scanProperties.PortRangeEnd.RemovetValidationError();
                        ValidatePortRangeEnd();
                    }
                }
                else
                {
                    logger.Error($"Port range start \"{scanProperties.PortRangeStart.Input.Text}\" must be a number within the range of 1 - 65535");
                    scanProperties.PortRangeStart.Error();

                    if (scanProperties.PortRangeEnd.IsValid)
                    {
                        scanProperties.PortRangeEnd.Error();
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
            if (string.IsNullOrEmpty(scanProperties.PortRangeEnd.Input.Text.Trim()))
            {
                scanProperties.PortRangeEnd.Reset();
            }
            else
            {
                if (scanProperties.PortRangeStart.IsValid)
                {
                    ValidatePortRangeEnd();
                }
                else
                {
                    logger.Warn($"Please set valid port range-start before setting the range-end");
                    scanProperties.PortRangeEnd.Error();
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
            if (string.IsNullOrEmpty(scanProperties.MaximumConcurrentScans.Input.Text.Trim()))
            {
                scanProperties.MaximumConcurrentScans.Reset();
            }
            else
            {
                if (int.TryParse(scanProperties.MaximumConcurrentScans.Input.Text, out int max) && max > 0)
                {
                    scanProperties.MaximumConcurrentScans.MaxThreadCount = max;
                    scanProperties.MaximumConcurrentScans.IsValid = true;
                    logger.Log($"Successfully set maximum concurrent scans to {scanProperties.MaximumConcurrentScans.Input.Text}");
                }
                else
                {
                    logger.Error($"Maximum concurrent scans \"{scanProperties.MaximumConcurrentScans.Input.Text}\" must be a number greater than 0");
                    scanProperties.MaximumConcurrentScans.Error();
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
            if (string.IsNullOrEmpty(scanProperties.Timeout.Input.Text.Trim()))
            {
                scanProperties.Timeout.Reset();
            }
            else
            {
                if (int.TryParse(scanProperties.Timeout.Input.Text, out int timeout) && timeout > 49)
                {
                    scanProperties.Timeout.Timeout = timeout;
                    scanProperties.Timeout.IsValid = true;
                    logger.Log($"Successfully set timeout to {scanProperties.Timeout.Input.Text}");
                }
                else
                {
                    logger.Error($"Timeout \"{scanProperties.Timeout.Input.Text}\" must be a number (50+)");
                    scanProperties.Timeout.Error();
                }
            }
        }

        private void OnPredefinedPortsChanged(object? sender, EventArgs e)
        {
            if (sender is not PredefinedPortsProperty property) return;

            if (property.Input.Checked)
            {
                if (scanProperties.OnlyWellKnownPorts.Input.Checked && !(scanProperties.OnlyWellKnownPorts == property)) scanProperties.OnlyWellKnownPorts.Input.Checked = false;
                if (scanProperties.OnlyPrivatePorts.Input.Checked && !(scanProperties.OnlyPrivatePorts == property)) scanProperties.OnlyPrivatePorts.Input.Checked = false;
                if (scanProperties.OnlyRegisteredPorts.Input.Checked && !(scanProperties.OnlyRegisteredPorts == property)) scanProperties.OnlyRegisteredPorts.Input.Checked = false;
                if (scanProperties.AllPorts.Input.Checked && !(scanProperties.AllPorts == property)) scanProperties.AllPorts.Input.Checked = false;

                scanProperties.PortRangeStart.Reset();
                scanProperties.PortRangeEnd.Reset();
                scanProperties.PortRangeStart.Input.Text = property.PredefinedPortsStart.ToString();
                scanProperties.PortRangeEnd.Input.Text = property.PredefinedPortsEnd.ToString();
                scanProperties.PortRangeStart.IsValid = true;
                scanProperties.PortRangeEnd.IsValid = true;
                scanProperties.PortRangeStart.Input.Enabled = false;
                scanProperties.PortRangeEnd.Input.Enabled = false;
                scanProperties.PortRangeStart.Port = property.PredefinedPortsStart;
                scanProperties.PortRangeEnd.Port = property.PredefinedPortsEnd;
                logger.Log($"Enabled predefined ports: {property.PredefinedPortsStart} - {property.PredefinedPortsEnd}");
            }
            else
            {
                scanProperties.PortRangeStart.Input.Enabled = true;
                scanProperties.PortRangeEnd.Input.Enabled = true;
                scanProperties.PortRangeStart.Reset();
                scanProperties.PortRangeEnd.Reset();
                logger.Log($"Disabled predefined ports, custom range will be used");
            }
        }
    }
}
