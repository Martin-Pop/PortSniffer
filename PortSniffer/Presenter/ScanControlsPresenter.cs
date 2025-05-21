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
        }

        //TODO: write errors in the output console

        /// <summary>
        /// Validates the IP address in the target IP control field.
        /// </summary>
        private void ValidateIPTarget()
        {
            if (IPValidator.ValidateIP(controlPanelView.TargetIP.Input.Text, out IPAddress? ip))
            {
                controlPanelView.TargetIP.IpAddress = ip!;
                controlPanelView.TargetIP.Input.Text = ip!.ToString();
                controlPanelView.TargetIP.IsValid = true;
            }
            else
            {
                logger.Warn("Ip adress is invalid");
                controlPanelView.HighlightValidationError(controlPanelView.TargetIP);
                controlPanelView.TargetIP.IsValid = false;
            }
        }

        /// <summary>
        /// Validates the IP address in the 'target IP range end' control field.
        /// </summary>
        private void ValidateIPTargetRangeEnd()
        {
            if (IPValidator.ValidateIP(controlPanelView.TargetIPRangeEnd.Input.Text, out IPAddress? ip) && IPValidator.IsGreaterThan(ip!, controlPanelView.TargetIP.IpAddress))
            {
                controlPanelView.TargetIPRangeEnd.IpAddress = ip!;
                controlPanelView.TargetIPRangeEnd.Input.Text = ip!.ToString();
                controlPanelView.TargetIPRangeEnd.IsValid = true;
            }
            else
            {
                Debug.WriteLine("IP range end is not valid");
                controlPanelView.HighlightValidationError(controlPanelView.TargetIPRangeEnd);
                controlPanelView.TargetIPRangeEnd.IsValid = false;
            }
        }

        /// <summary>
        /// Validates the subnet mask in the subnet mask control field.
        /// </summary>
        private void ValidateSubnetMask()
        {
            if(IPValidator.ValidateSubnetMask(controlPanelView.SubnetMask.Input.Text, out IPAddress? mask))
            {
                controlPanelView.SubnetMask.IpAddress = mask!;
                controlPanelView.SubnetMask.Input.Text = mask!.ToString();
                controlPanelView.SubnetMask.IsValid = true;
            }else
            {
                Debug.WriteLine("Subnet mask is not valid");
                controlPanelView.HighlightValidationError(controlPanelView.SubnetMask);
                controlPanelView.SubnetMask.IsValid = false;
            }
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
            if (string.IsNullOrEmpty(controlPanelView.TargetIP.Input.Text.Trim()))
            {
                controlPanelView.TargetIP.Reset();
                controlPanelView.TargetIPRangeEnd.Reset();
            }
            else
            {
                ValidateIPTarget();
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
                ValidateIPTargetRangeEnd();
                controlPanelView.SubnetMask.Reset();
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
                ValidateSubnetMask();
                controlPanelView.TargetIPRangeEnd.Reset();
            }
        }
    }
}
