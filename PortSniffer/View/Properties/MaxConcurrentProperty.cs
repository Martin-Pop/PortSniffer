using PortSniffer.Core.Config;
using PortSniffer.View.Abstract;

namespace PortSniffer.View.Controls
{
    public class MaxConcurrentProperty : ScanPropertyInputAbstract
    {
        public int MaxThreadCount { get; set; }

        public MaxConcurrentProperty(string label, string toolTipMessage, bool required, Settings settings, string placeholder = "") : base(label, toolTipMessage, required, settings, placeholder)
        {
            IsValid = true;
            MaxThreadCount = Settings.DefaultMaxThreads;
            Input.Text = MaxThreadCount.ToString();
        }

        public override void Reset()
        {
            IsValid = true;
            MaxThreadCount = Settings.DefaultMaxThreads;

            Input.Text = MaxThreadCount.ToString();
            Input.BackColor = Color.White;
        }
    }
}
