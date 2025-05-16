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

namespace PortSniffer.Presenter
{
    public class ScanControlsPresenter
    {
        private readonly ControlPanelView controlPanelView;
        public ScanControlsPresenter(ControlPanelView view) 
        {
            controlPanelView = view;
            controlPanelView.TargetIP.ValidationEvent += ValidateIP;
        }

        private void ValidateIP(object? sender, EventArgs e)
        {
            
            if (sender is IPAddressProperty property)
            {
                string input = property.Input.Text.Trim();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    property.IsValid = IPAddress.TryParse(input, out IPAddress? ip);
                    if (!property.IsValid)
                    {
                        controlPanelView.HighlightValidationError(property);
                    }

                    if (ip != null)
                    {
                        property.IpAddress = ip;
                        property.Input.Text = ip.ToString();
                    }
                }

                LoseFocus();
            }
        }

        /// <summary>
        /// Forces focus on controlPanelView, used after validation events
        /// </summary>
        private void LoseFocus()
        {
            controlPanelView.Focus();
        }
    }
}
