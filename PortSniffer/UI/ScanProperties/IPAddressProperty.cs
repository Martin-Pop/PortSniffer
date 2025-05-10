using PortSniffer.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortSniffer.UI.ScanProperties
{
    public class IPAddressProperty : ScanProperty
    {
        public IPAddress IpAddress { get; private set; } = IPAddress.None;
        public IPAddressProperty(PropertyLabel pl, PropertyTooltip tp, Control c) : base(pl, tp, c)
        {
            control = (PropertyTextInput) control;

            control.KeyDown += Control_KeyDown;
            control.GotFocus += (_,_) => control.BackColor = Color.White;
            control.LostFocus += (_,_) => Validate();  
        }

        private void Control_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                Validate();
            }
        }

        private void Validate()
        {
            string input = control.Text.Trim();
            if (!string.IsNullOrWhiteSpace(input))
            {
                IsValid = IPAddress.TryParse(input, out IPAddress? ip);
                if (!IsValid)
                {
                    control.BackColor = Color.FromArgb(255, 222, 222);
                }
                if (ip != null)
                {
                    IpAddress = ip;
                    control.Text = ip.ToString();
                }
            }

            label.Focus(); 
        }

    }
}
