﻿using PortSniffer.Model.Config;
using PortSniffer.View.Abstract;
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
    /// <summary>
    /// Represents IP address property for scan input.
    /// </summary>
    public class IPAddressProperty : ScanPropertyInputAbstract
    {
        public IPAddress IpAddress { get; set; } = IPAddress.None;
        
        public IPAddressProperty(string label, string toolTipMessage, bool required,Settings settings, string placeholder = "") : base(label, toolTipMessage, required, settings, placeholder)
        {
        }

        /// <summary>
        /// Resets the property
        /// </summary>
        public override void Reset()
        {
            IsValid = false;
            IpAddress = IPAddress.None;

            Input.Text = string.Empty;
            Input.BackColor = Color.White;
        }
    }
}
