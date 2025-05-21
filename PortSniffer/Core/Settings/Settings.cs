using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Core.Settings
{
    public class Settings
    {
        public string FontFamily { get; set; } = "Consolas";
        public float FontSize { get; set; } = 11f;
        public string TextColor { get; set; } = "#000000";
        public string BackgroundColor { get; set; } = "#ededed";
        public string InputErrorHighlightColor { get; set; } = "#787474";
    }
}
