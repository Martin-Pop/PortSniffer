using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Interface
{
    /// <summary>
    /// Interface for console logger
    /// </summary>
    public interface IConsoleLogger
    {
        void Warn(string msg);
        void Log(string msg);
        void Error(string msg);

        /// <summary>
        /// clears console
        /// </summary>
        void CLear();
    }
}
