using PortSniffer.Core.Interface;
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
        
        private readonly Color warnColor = Color.FromArgb(255, 0, 0);
        private readonly Color infoColor = Color.Black;

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
    }
}
