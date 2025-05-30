using PortSniffer.Model.Config;
using PortSniffer.View.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Controls
{
    public class TimeoutProperty : ScanPropertyInputAbstract
    {
        public int Timeout { get; set; }

        public TimeoutProperty(string label, string toolTipMessage, bool required, Settings settings, string placeholder = "") : base(label, toolTipMessage, required, settings, placeholder)
        {
            IsValid = true;
            Timeout = Settings.DefautTimeout;
            Input.Text = Timeout.ToString();
        }

        public override void Reset()
        {
            IsValid = true;
            Timeout = Settings.DefautTimeout;

            Input.Text = Timeout.ToString();
            Input.BackColor = Color.White;
        }
    }
}
