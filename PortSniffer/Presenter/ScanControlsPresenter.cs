using PortSniffer.Model;
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
        private readonly ControlPanelView controlPanelView;
        public ScanControlsPresenter(ControlPanelView view) 
        {
            controlPanelView = view;
            controlPanelView.TargetIP.ValidationEvent += OnTargetIPValidation;
            controlPanelView.TargetIPRangeEnd.ValidationEvent += OnTargetIPRangeEndValidation;
            controlPanelView.SubentMask.ValidationEvent += OnSubnetMaskValidation;
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
                Debug.WriteLine("IP is not valid");
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
            if(IPValidator.ValidateSubnetMask(controlPanelView.SubentMask.Input.Text, out IPAddress? mask))
            {
                controlPanelView.SubentMask.IpAddress = mask!;
                controlPanelView.SubentMask.Input.Text = mask!.ToString();
                controlPanelView.SubentMask.IsValid = true;
            }else
            {
                Debug.WriteLine("Subnet mask is not valid");
                controlPanelView.HighlightValidationError(controlPanelView.SubentMask);
                controlPanelView.SubentMask.IsValid = false;
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
                    controlPanelView.ResetValidationError(controlPanelView.TargetIPRangeEnd);
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
                controlPanelView.SubentMask.Reset();
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
            if (string.IsNullOrEmpty(controlPanelView.SubentMask.Input.Text.Trim()))
            {
                controlPanelView.SubentMask.Reset();
            }
            else
            {
                ValidateSubnetMask();
                controlPanelView.TargetIPRangeEnd.Reset();
            }
        }
    }
}
