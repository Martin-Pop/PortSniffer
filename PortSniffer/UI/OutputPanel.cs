using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.UI
{
    public class OutputPanel : Panel
    {
        private RichTextBox ouputConsole;
        public OutputPanel() 
        {
            this.Dock = DockStyle.Fill;

            ouputConsole = new RichTextBox();
            ouputConsole.Dock = DockStyle.Fill;
            
            this.Controls.Add(ouputConsole);
        }
    }
}
