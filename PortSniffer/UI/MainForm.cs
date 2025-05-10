using PortSniffer.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortSniffer.UI
{
    public class MainForm : Form
    {
        private SplitContainer mainSplitContainer;
        private SplitContainer leftSplitContainer;
        private ScanControls scanControls;
        // TODO: make only two sides - output and controls on the right
        public MainForm()
        {
            this.Size = new Size(800,600);
            this.MinimumSize = new Size(480, 360);
            this.Text = "Port Sniffer";
            this.Padding = new Padding(5);

            //layout
            mainSplitContainer = new SplitContainer();
            mainSplitContainer.Dock = DockStyle.Fill;
            mainSplitContainer.SplitterDistance = 100;

            leftSplitContainer = new SplitContainer();
            leftSplitContainer.Dock = DockStyle.Fill;
            leftSplitContainer.Orientation = Orientation.Horizontal;

            mainSplitContainer.Panel1.Controls.Add(leftSplitContainer);

            //output
            OutputPanel ow = new OutputPanel();
            leftSplitContainer.Panel2.Controls.Add(ow);

            //scan control
            scanControls = new ScanControls();
            scanControls.Dock = DockStyle.Fill;



            //test
            Button b1 = new Button() { Dock = DockStyle.Fill };
             
            //Button b2 = new Button() { Dock = DockStyle.Fill };

            leftSplitContainer.Panel1.Controls.Add(b1);
            mainSplitContainer.Panel2.Controls.Add(scanControls);
            this.Controls.Add(mainSplitContainer);
            


        }

    }
}
