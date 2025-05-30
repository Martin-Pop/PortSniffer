using PortSniffer.Model.Config;
using PortSniffer.View.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Controls
{
    public class PortProperty : ScanPropertyInputAbstract
    {
        public int Port { get; set; } = 0;

        public PortProperty(string label, string toolTipMessage, bool required, Settings settings, string placeholder = "") : base(label, toolTipMessage, required, settings, placeholder)
        {
        }

        public override void Reset()
        {
            IsValid = false;
            Port = 0;

            Input.Text = string.Empty;
            Input.BackColor = Color.White;
        }
    }
}
