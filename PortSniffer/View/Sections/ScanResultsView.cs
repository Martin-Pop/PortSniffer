using PortSniffer.Model.Config;
using PortSniffer.Model.Scanner;
using PortSniffer.View.Abstract;
using PortSniffer.View.Interface;
using PortSniffer.View.ScanResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Sections
{
    /// <summary>
    /// Results view
    /// </summary>
    public class ScanResultsView : PanelAbstract, IScanResultsView
    {
        private readonly SplitContainer splitContainer;
        private readonly TableLayoutPanel selectionPanel;
        private readonly RichTextBox resultsTextBox;

        public ScanResultsView(Settings settings) : base(settings)
        {
            AutoSize = true;
            Dock = DockStyle.Fill;
            //Padding = new Padding(5, 0, 0, 0);

            splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 50,
            };

            selectionPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray,
                AutoSize = false,
                AutoScroll = true,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(0, 0, 5, 0)
            };

            resultsTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                ForeColor = Color.Black
            };

            resultsTextBox.KeyDown += HandleConsoleInput;

            splitContainer.Panel1.Controls.Add(selectionPanel);
            splitContainer.Panel2.Controls.Add(resultsTextBox);
            Controls.Add(splitContainer);
        }

        /// <summary>
        /// Handler for the results text box to prevent key presses from being processed, even though is readonly.
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">event arguments</param>
        private void HandleConsoleInput(object? sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        /// <summary>
        /// Adds a scan result to the selection panel.
        /// </summary>
        /// <param name="scanResultProperty"></param>
        public void AddScanResult(ScanResultProperty scanResultProperty)
        {
            selectionPanel.RowCount++;
            selectionPanel.Controls.Add(scanResultProperty);
        }

        /// <summary>
        /// Clears all scan results
        /// </summary>
        public void ClearResults()
        {
            selectionPanel.Controls.Clear();
            resultsTextBox.Clear();
        }

        /// <summary>
        /// Writes into the 'results text box' specific scan result.
        /// </summary>
        /// <param name="result">Result to show</param>
        public void ViewScanResult(ScanResult result)
        {
            //TODO: nake this look better
            resultsTextBox.Clear();
            resultsTextBox.AppendText($"Scan Result for {result.IPAddress}:\n");
            resultsTextBox.AppendText("Open Ports:\n");
            foreach (var port in result.OpenPorts)
            {
                resultsTextBox.AppendText($"Port {port} is open.\n");
            }
        }

        /// <summary>
        /// Applies settings
        /// </summary>
        public override void ApplySettings()
        {
            resultsTextBox.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
        }
    }
}
