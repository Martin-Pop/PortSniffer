using PortSniffer.Model.Config;
using PortSniffer.Model.Scanner;
using PortSniffer.View.Abstract;
using PortSniffer.View.Interface;
using PortSniffer.View.ScanResults;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        private readonly Label infoLabel;

        public ScanResultsView(Settings settings) : base(settings)
        {
            AutoSize = true;
            Dock = DockStyle.Fill;

            splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 50,
            };

            selectionPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                
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
                SelectionIndent = 10,
                SelectionRightIndent = 10,
                BackColor = Color.White,
                ForeColor = Color.Black
            };

            infoLabel = new Label
            {
                Text = "Scan results:",
                Dock = DockStyle.Top,
                
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = ColorTranslator.FromHtml("#b6b3b5"),
            };

            ApplySettings();
            ApplyDefautlText();

            resultsTextBox.KeyDown += HandleConsoleInput;

           
            splitContainer.Panel1.Controls.Add(selectionPanel);
            splitContainer.Panel2.Controls.Add(resultsTextBox);
            Controls.Add(splitContainer);
        }

        private void ApplyDefautlText()
        {
            splitContainer.Panel1.Controls.Add(infoLabel);
            resultsTextBox.Font = new Font(resultsTextBox.Font.FontFamily, resultsTextBox.Font.Size, FontStyle.Italic);
            resultsTextBox.Text = "No results to view";
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
            splitContainer.Panel1.Controls.Remove(infoLabel);
            infoLabel.Enabled = false;
            selectionPanel.RowCount++;
            selectionPanel.Controls.Add(scanResultProperty);
        }

        /// <summary>
        /// Clears all scan results
        /// </summary>
        public void ClearResults()
        {
            selectionPanel.Controls.Clear();
            selectionPanel.RowCount = 2;

            //only way i managed to reset the scollbar
            selectionPanel.AutoScroll = false;
            selectionPanel.AutoScroll = true;

            UpdateScanProgress(null);

            ApplyDefautlText();
        }

        /// <summary>
        /// Writes into the 'results text box' specific scan result.
        /// </summary>
        /// <param name="result">Result to show</param>
        public void ViewScanResult(ScanResult result)
        {
            resultsTextBox.Font = new Font(resultsTextBox.Font.FontFamily, resultsTextBox.Font.Size, FontStyle.Regular);

            resultsTextBox.Clear();
            resultsTextBox.AppendText(result.ToString());
            
        }

        /// <summary>
        /// Applies settings
        /// </summary>
        public override void ApplySettings()
        {
            resultsTextBox.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
            infoLabel.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Italic);
        }

        /// <summary>
        /// Updates the scan progress in the form title.
        /// </summary>
        /// <param name="progress">Progress to set</param>
        public void UpdateScanProgress(ScanProgress? progress)
        {
            Form? form = FindForm();
            if (form == null) return;

            if (form.InvokeRequired)
            {
                form.Invoke(new Action(() =>
                {
                    form.Text = progress != null ? $"Port Sniffer - {progress}" : "Port Sniffer";

                }));
            }
            else
            {
                form.Text = progress != null ? $"Port Sniffer - {progress}" : "Port Sniffer";
            }
        }
    }
}
