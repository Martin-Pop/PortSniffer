using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PortSniffer.Model.Config
{
    /// <summary>
    /// Settings for the app, mainly visuals
    /// </summary>
    public class Settings
    {
        public string FontFamily { get; set; } = "Consolas";
        public float FontSize { get; set; } = 11f;
        //public string TextColor { get; set; } = "#000000";
        public string ConsoleBackgroundColor { get; set; } = "#fafafa";
        public string BackgroundColor { get; set; } = "#ededed";
        public string InputErrorHighlightColor { get; set; } = "#787474";

        public int DefautTimeout { get; set; } = 500;
        public int DefaultMaxThreads { get; set; } = 500;
    }
}
