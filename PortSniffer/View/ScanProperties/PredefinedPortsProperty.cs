using PortSniffer.Model.Config;
using PortSniffer.View.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Properties
{
    /// <summary>
    /// Property for predefined ports in the scan properties.
    /// </summary>
    public class PredefinedPortsProperty : ScanPropertyCheckBoxAbstract
    {
        public int PredefinedPortsStart { get; private set; }
        public int PredefinedPortsEnd { get; private set; }
        public event EventHandler? StateChangedEvent;
        public PredefinedPortsProperty(string label, string toolTipMessage, Settings settings, int portRangeStart, int portRangeEnd) : base(label, toolTipMessage, settings)
        {
            PredefinedPortsStart = portRangeStart;
            PredefinedPortsEnd = portRangeEnd;

            Input.CheckedChanged += HandleInput;
        }

        /// <summary>
        /// Triggers the state changed event.
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">event arguments</param>
        private void HandleInput(object? sender, EventArgs e)
        {
            StateChangedEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
