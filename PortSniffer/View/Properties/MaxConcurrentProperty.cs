using PortSniffer.Core.Config;
using PortSniffer.View.Abstract;

namespace PortSniffer.View.Controls
{
    public class MaxConcurrentProperty : ScanPropertyInputAbstract
    {
        public int MaxThreadCount { get; set; } = 100;

        public MaxConcurrentProperty(string label, string toolTipMessage, bool required, Settings settings, string placeholder = "") : base(label, toolTipMessage, required, settings, placeholder)
        {
            IsValid = true;
            Input.Text = MaxThreadCount.ToString();
        }

        public override void Reset()
        {
            IsValid = false;
            MaxThreadCount = 0;

            Input.Text = string.Empty;
            Input.BackColor = Color.White;
        }
    }
}
