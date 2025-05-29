using PortSniffer.Core.Config;
using PortSniffer.Core.Interface;
using PortSniffer.Model;
using PortSniffer.View.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.ScanResults
{
    /// <summary>
    /// Represents the UI for each scan result as a 'button' that will show all the ports
    /// </summary>
    public class ScanResultProperty : PanelAbstract
    {
        private readonly ScanResult results;
        private readonly Button selectButton;
        public event Action<ScanResult> ScanResultSelected;

        public ScanResultProperty(Settings settings, ScanResult results) : base(settings) 
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Dock = DockStyle.Fill;


            selectButton = new Button
            {
                Text = results.IPAddress,
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            ApplySettings();

            Controls.Add(selectButton);
        }

        void SelectButton_Click(object sender, EventArgs e)
        {
            ScanResultSelected?.Invoke(results);
        }

        /// <summary>
        /// Applies settings
        /// </summary>
        public override void ApplySettings()
        {
            selectButton.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
        }
    }
}
