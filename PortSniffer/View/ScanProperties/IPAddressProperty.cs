using PortSniffer.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortSniffer.View.ScanProperties
{
    public class IPAddressProperty : ScanPropertyAbstract
    {
        public IPAddress IpAddress { get; set; } = IPAddress.None;
        public event EventHandler? ValidationEvent;
        public IPAddressProperty(PropertyLabel pl, PropertyTooltip tp, Control c, bool required) : base(pl, tp, c, required)
        {
            Input = (PropertyTextInput) Input;

            IsValid = false;
            Input.KeyDown += Control_KeyDown;
            Input.LostFocus += (_, _) => ValidationEvent?.Invoke(this, EventArgs.Empty);
            Input.GotFocus += (_, _) => Input.BackColor = Color.White;
        }

        private void Control_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                

                Label.Focus(); //lose focus so that the event is triggered
            }
        }

        public override void Reset()
        {
            IsValid = false;
            IpAddress = IPAddress.None;
            Input.Text = string.Empty;
            Input.BackColor = Color.White;
        }
    }
}
