using PortSniffer.Core.Interface;
using PortSniffer.View.Interface;
using PortSniffer.View.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Presenter
{
    public class OutputConsolePresenter : IConsoleLogger
    {
        private readonly OutputConsoleView outputConsoleView;
        
        private readonly Color warnColor = Color.FromArgb(255, 110, 15);
        private readonly Color infoColor = Color.Black;
        private readonly Color errorColor = Color.FromArgb(225, 0, 0);

        public OutputConsolePresenter(OutputConsoleView outputConsoleView)
        {
            this.outputConsoleView = outputConsoleView;
        }

        public void Log(string msg)
        {
            outputConsoleView.Write(msg, infoColor);
        }

        public void Warn(string msg)
        {
            outputConsoleView.Write(msg, warnColor);
        }

        public void Error(string msg)
        {
            outputConsoleView.Write(msg, errorColor);
        }

        public void CLear()
        {
            outputConsoleView.Clear();
        }
    }
}
