using PortSniffer.Model.Config;
using PortSniffer.View.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Controls
{
    /// <summary>
    /// Property for specifying a port number.
    /// </summary>
    public class PortProperty : ScanPropertyInputAbstract
    {
        public int Port { get; set; } = 0;

        public PortProperty(string label, string toolTipMessage, bool required, Settings settings, string placeholder = "") : base(label, toolTipMessage, required, settings, placeholder)
        {
        }

        /// <summary>
        /// Resets the property.
        /// </summary>
        public override void Reset()
        {
            IsValid = false;
            Port = 0;

            Input.Text = string.Empty;
            Input.BackColor = Color.White;
        }
    }
}
