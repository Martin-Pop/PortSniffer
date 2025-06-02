using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Interface
{
    /// <summary>
    /// Interface for constol panel view.
    /// </summary>
    public interface IControlPanelView
    {
        public Button Start { get; }
        public Button Stop { get; }
        public Button PauseResume { get; }
        public Button Clear { get; }
    }
}
