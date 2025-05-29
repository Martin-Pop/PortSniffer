using PortSniffer.Core.Config;
using PortSniffer.Core.Interface;
using PortSniffer.Model;
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
                RowCount = 20,
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

        public void AddScanResult(ScanResultProperty scanResultProperty)
        {
            //selectionPanel.RowCount++;
            selectionPanel.Controls.Add(scanResultProperty);
        }

        public override void ApplySettings()
        {
            //
        }

        public void ViewScanResult(ScanResult result)
        {
            //
        }

        private void HandleConsoleInput(object? sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
    }
}
