using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Interface
{
    /// <summary>
    /// Represents the output console view for displaying messages.
    /// </summary>
    public interface IOutpuConsoleView
    {
        /// <summary>
        /// Writes a message to the output console.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="color">Color of the message</param>
        void Write(string message, Color color);
        void Clear();
    }
}
