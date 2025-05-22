using PortSniffer.Core.Interface;
using PortSniffer.Model;
using PortSniffer.View.Interface;
using PortSniffer.View.ScanProperties;
using PortSniffer.View.Sections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace PortSniffer.Presenter
{
    public class ScanControlsPresenter
    {
        private readonly IControlPanelView controlPanelView;
        private readonly IConsoleLogger logger;
        public ScanControlsPresenter(IControlPanelView view, IConsoleLogger logger)
        {
            this.logger = logger;
            controlPanelView = view;

            //events
            controlPanelView.TargetIP.ValidationEvent += OnTargetIPValidation;
            controlPanelView.TargetIPRangeEnd.ValidationEvent += OnTargetIPRangeEndValidation;
            controlPanelView.SubnetMask.ValidationEvent += OnSubnetMaskValidation;
            controlPanelView.PortRangeStart.ValidationEvent += OnPortRangeStartValidation;
            controlPanelView.PortRangeEnd.ValidationEvent += OnPortRangeEndValidation;
        }

        /// <summary>
        /// Validates the IP address in the 'target IP range end' control field.
        /// </summary>
        private void ValidateIPTargetRangeEnd()
        {
            if (IPValidator.ValidateIP(controlPanelView.TargetIPRangeEnd.Input.Text, out IPAddress? ip))
            {
                if (IPValidator.IsGreaterThan(ip!, controlPanelView.TargetIP.IpAddress))
                {
                    controlPanelView.TargetIPRangeEnd.IpAddress = ip!;
                    controlPanelView.TargetIPRangeEnd.Input.Text = ip!.ToString();
                    controlPanelView.TargetIPRangeEnd.IsValid = true;
                    logger.Log($"Successfully set IP adress range-end to {controlPanelView.TargetIPRangeEnd.Input.Text}");
                    return;
                }
                else
                {
                    logger.Warn($"IP address range-end \"{ip}\" must be greater than \"{controlPanelView.TargetIP.Input.Text}\"");
                    controlPanelView.TargetIPRangeEnd.Input.Text = ip!.ToString();
                }
            }
            else
            {
                logger.Warn($"IP address range-end \"{controlPanelView.TargetIPRangeEnd.Input.Text}\" is not in the valid IPv4 format");
            }

            controlPanelView.HighlightValidationError(controlPanelView.TargetIPRangeEnd);
            controlPanelView.TargetIPRangeEnd.IsValid = false;
        }

        /// <summary>
        /// Validates the port range-end.
        /// </summary>
        private void ValidatePortRangeEnd()
        {
            if (int.TryParse(controlPanelView.PortRangeEnd.Input.Text, out int port) && port > 0 && port <= 65535)
            {
                if (controlPanelView.PortRangeStart.Port < port)
                {
                    controlPanelView.PortRangeEnd.Port = port;
                    controlPanelView.PortRangeEnd.IsValid = true;
                    logger.Log($"Successfully set port range-end to {controlPanelView.PortRangeEnd.Input.Text}");
                    return;
                }
                else
                {
                    logger.Warn($"Port range-end \"{controlPanelView.PortRangeEnd.Input.Text}\" must be greater than {controlPanelView.PortRangeStart.Port} (start)");
                }
            }
            else
            {
                logger.Warn($"Port range-end \"{controlPanelView.PortRangeEnd.Input.Text}\" must be a number within the range of 1 - 65535");
            }
            controlPanelView.HighlightValidationError(controlPanelView.PortRangeEnd);
            controlPanelView.PortRangeEnd.IsValid = false;
        }

        //EVENT HANDLERS

        /// <summary>
        /// Handler for the target IP validation event.
        /// if the input is empty, it resets the field. Otherwise, it validates the target IP
        /// and revalidates the end range if it was provided
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event arguments</param>
        private void OnTargetIPValidation(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(controlPanelView.TargetIP.Input.Text.Trim()))
            {
                controlPanelView.TargetIP.Reset();
                controlPanelView.TargetIPRangeEnd.Reset();
            }
            else
            {
                if (IPValidator.ValidateIP(controlPanelView.TargetIP.Input.Text, out IPAddress? ip))
                {
                    controlPanelView.TargetIP.IpAddress = ip!;
                    controlPanelView.TargetIP.Input.Text = ip!.ToString();
                    controlPanelView.TargetIP.IsValid = true;
                    logger.Log($"Successfully set IP adress target to {controlPanelView.TargetIP.Input.Text}");
                }
                else
                {
                    logger.Warn($"IP address \"{controlPanelView.TargetIP.Input.Text}\" is not in the valid IPv4 format");
                    controlPanelView.HighlightValidationError(controlPanelView.TargetIP);
                    controlPanelView.TargetIP.IsValid = false;
                }

                if (!string.IsNullOrWhiteSpace(controlPanelView.TargetIPRangeEnd.Input.Text)) //revalidate if there was something
                {
                    controlPanelView.RemoveHighlightValidationError(controlPanelView.TargetIPRangeEnd);
                    ValidateIPTargetRangeEnd();
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
            if (string.IsNullOrEmpty(controlPanelView.TargetIPRangeEnd.Input.Text.Trim()))
            {
                controlPanelView.TargetIPRangeEnd.Reset();
            }
            else
            {
                if (controlPanelView.TargetIP.IsValid)
                {
                    ValidateIPTargetRangeEnd();
                    controlPanelView.SubnetMask.Reset();
                }
                else
                {
                    logger.Warn($"Please set valid IP Target before setting the range-end");
                    controlPanelView.HighlightValidationError(controlPanelView.TargetIPRangeEnd);
                    controlPanelView.TargetIPRangeEnd.IsValid = false;
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
            if (string.IsNullOrEmpty(controlPanelView.SubnetMask.Input.Text.Trim()))
            {
                controlPanelView.SubnetMask.Reset();
            }
            else
            {
                if (IPValidator.ValidateSubnetMask(controlPanelView.SubnetMask.Input.Text, out IPAddress? mask))
                {
                    controlPanelView.SubnetMask.IpAddress = mask!;
                    controlPanelView.SubnetMask.Input.Text = mask!.ToString();
                    controlPanelView.SubnetMask.IsValid = true;
                    logger.Log($"Successfully set subnet mask to {controlPanelView.SubnetMask.Input.Text}");
                }
                else
                {
                    logger.Warn($"Subnet mask \"{controlPanelView.SubnetMask.Input.Text}\" is not valid IPv4 mask, try using CIDR format");
                    controlPanelView.HighlightValidationError(controlPanelView.SubnetMask);
                    controlPanelView.SubnetMask.IsValid = false;
                }

                controlPanelView.TargetIPRangeEnd.Reset();
            }
        }

        /// <summary>
        /// Handler for the port range-start validation event.
        /// If the input is empty, it resets the field. Otherwise, it validates the port range-start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPortRangeStartValidation(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(controlPanelView.PortRangeStart.Input.Text.Trim()))
            {
                controlPanelView.PortRangeStart.Reset();
            }
            else
            {
                if (int.TryParse(controlPanelView.PortRangeStart.Input.Text, out int port) && port > 0 && port <= 65535)
                {
                    controlPanelView.PortRangeStart.Port = port;
                    controlPanelView.PortRangeStart.IsValid = true;
                    logger.Log($"Successfully set port range-start to {controlPanelView.PortRangeStart.Input.Text}");
                }
                else
                {
                    logger.Warn($"Port range start \"{controlPanelView.PortRangeStart.Input.Text}\" must be a number within the range of 1 - 65535");
                    controlPanelView.HighlightValidationError(controlPanelView.PortRangeStart);
                    controlPanelView.PortRangeStart.IsValid = false;
                }

                if (!string.IsNullOrWhiteSpace(controlPanelView.PortRangeEnd.Input.Text)) //revalidate if there was something
                {
                    controlPanelView.RemoveHighlightValidationError(controlPanelView.PortRangeEnd);
                    ValidatePortRangeEnd();
                }
            }
        }

        /// <summary>
        /// Handler for the port range-end validation event.
        /// If the input is empty, it resets the field. Otherwise, it validates the port range-end
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPortRangeEndValidation(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(controlPanelView.PortRangeEnd.Input.Text.Trim()))
            {
                controlPanelView.PortRangeEnd.Reset();
            }
            else
            {
                if (controlPanelView.PortRangeStart.IsValid)
                {
                    ValidatePortRangeEnd();
                }
                else
                {
                    logger.Warn($"Please set valid port range-start before setting the range-end");
                    controlPanelView.HighlightValidationError(controlPanelView.PortRangeEnd);
                    controlPanelView.PortRangeEnd.IsValid = false;
                }
                
            }
        }

        //TODO: vladate maximum concurrent scans, timeout
        //TODO?: make more types of highilihts so it can indicate locked property (not used, overwritten by radio buttons)
        //TODO: add radio buttons for only well known ports. 
    }
}
