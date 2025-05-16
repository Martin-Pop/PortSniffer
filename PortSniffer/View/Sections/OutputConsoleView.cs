using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Sections
{
    public class OutputConsoleView : Panel
    {
        private RichTextBox ouputConsole;
        public OutputConsoleView()
        {
            Dock = DockStyle.Fill;

            ouputConsole = new RichTextBox();
            ouputConsole.Dock = DockStyle.Fill;

            Controls.Add(ouputConsole);
        }
    }
}
