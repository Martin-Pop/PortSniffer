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
            Size = new Size(800, 600);
            MinimumSize = new Size(480, 360);
            Text = "Port Sniffer";
            Padding = new Padding(5);


            //main layout
            mainSplitContainer = new SplitContainer();
            mainSplitContainer.Dock = DockStyle.Fill;
            mainSplitContainer.SplitterDistance = 100;

            leftSplitContainer = new SplitContainer();
            leftSplitContainer.Dock = DockStyle.Fill;
            leftSplitContainer.Orientation = Orientation.Horizontal;
            leftSplitContainer.Panel1.BackColor = Color.LightBlue;
            leftSplitContainer.Panel2.BackColor = Color.Blue;

            mainSplitContainer.Panel1.Controls.Add(leftSplitContainer);

            Controls.Add(mainSplitContainer);

        }

        public void AddViews(ControlPanelView controlPanelView) //add other later (made so they are added to the correct panels)
        {
            mainSplitContainer.Panel2.Controls.Add(controlPanelView);
        }

    }
}
