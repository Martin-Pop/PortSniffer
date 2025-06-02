using PortSniffer.View.Interface;
using PortSniffer.View.Sections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortSniffer.View
{
    public class MainForm : Form
    {
        private readonly SplitContainer mainSplitContainer;
        private readonly SplitContainer leftSplitContainer;

        public MainForm()
        {
            Size = new Size(1000, 600);
            MinimumSize = new Size(480, 360);
            Text = "Port Sniffer";
            Padding = new Padding(5);

            //layout
            mainSplitContainer = new SplitContainer();
            mainSplitContainer.Dock = DockStyle.Fill;
            mainSplitContainer.SplitterDistance = 100;

            leftSplitContainer = new SplitContainer();
            leftSplitContainer.Dock = DockStyle.Fill;
            leftSplitContainer.Orientation = Orientation.Horizontal;

            mainSplitContainer.Panel1.Controls.Add(leftSplitContainer);

            Controls.Add(mainSplitContainer);
        }

        //adds views
        public void AddViews(ScanPropertiesView scanPropertiesView, OutputConsoleView outputConsoleView,ControlPanelView controlPanelView, ScanResultsView scanResultsView) //add other later (made so they are added to the correct panels)
        {
            mainSplitContainer.Panel2.Controls.Add(scanPropertiesView);
            mainSplitContainer.Panel2.Controls.Add(controlPanelView);
            leftSplitContainer.Panel2.Controls.Add(outputConsoleView);
            leftSplitContainer.Panel1.Controls.Add(scanResultsView);
        }

    }
}
