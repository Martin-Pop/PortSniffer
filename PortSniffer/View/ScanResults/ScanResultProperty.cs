using PortSniffer.Model.Config;
using PortSniffer.Model.Scanner;
using PortSniffer.View.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.ScanResults
{
    /// <summary>
    /// Represents the UI for each scan result as a 'button' that will show more information about the scan result.
    /// </summary>
    public class ScanResultProperty : PanelAbstract
    {
        private readonly ScanResult results;
        private readonly Button selectButton;
        public event Action<ScanResult> ScanResultSelected;

        public ScanResult Results => results;

        public ScanResultProperty(Settings settings, ScanResult results) : base(settings) 
        {
            this.results = results;

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

            selectButton.Click += SelectButton_Click;

            ApplySettings();

            Controls.Add(selectButton);
        }

        /// <summary>
        /// Invokes the ScanResultSelected event when the button is clicked, passing the scan result.
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">event argumants</param>
        void SelectButton_Click(object? sender, EventArgs e)
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
