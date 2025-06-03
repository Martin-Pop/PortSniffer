using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PortSniffer.Model.Config
{
    /// <summary>
    /// Settings for the app, mainly visuals
    /// </summary>
    public class Settings
    {
        [JsonPropertyName("font_family")]
        public string FontFamily { get; set; } = "Consolas";

        private float fontSize = 11f;
        [JsonPropertyName("font_size")]
        public float FontSize
        {
            get => fontSize;
            set { if (value > 0 && value < 100) fontSize = value; }
        }

        private string consoleBackgroundColor = "#fafafa";
        [JsonPropertyName("console_background_color")]
        public string ConsoleBackgroundColor
        {
            get => consoleBackgroundColor;
            set { if (IsValidHexColor(value)) consoleBackgroundColor = value; }
        }

        private string inputErrorHighlightColor = "#787474";
        [JsonPropertyName("input_error_highlight_color")]
        public string InputErrorHighlightColor
        {
            get => inputErrorHighlightColor;
            set { if (IsValidHexColor(value)) inputErrorHighlightColor = value; }
        }

        private string logColor = "#000000";
        [JsonPropertyName("log_color")]
        public string LogColor
        {
            get => logColor;
            set { if (IsValidHexColor(value)) logColor = value; }
        }

        private string errorColor = "#ff0000";
        [JsonPropertyName("error_color")]
        public string ErrorColor
        {
            get => errorColor;
            set { if (IsValidHexColor(value)) errorColor = value; }
        }

        private string warnColor = "#ff8c00";
        [JsonPropertyName("warn_color")]
        public string WarnColor
        {
            get => warnColor;
            set { if (IsValidHexColor(value)) warnColor = value; }
        }

        private int defaultTimeout = 500;
        [JsonPropertyName("default_timeout")]
        public int DefautTimeout
        {
            get => defaultTimeout;
            set { if (value > 0) defaultTimeout = value; }
        }

        private int defautlMaxThreads = 500;
        [JsonPropertyName("default_max_threads")]
        public int DefaultMaxThreads
        {
            get => defautlMaxThreads;
            set { if (value > 0) defautlMaxThreads = value; }
        }

        private bool IsValidHexColor(string? value)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                   Regex.IsMatch(value, "^#([0-9A-Fa-f]{6})$");
        }
    }
}
