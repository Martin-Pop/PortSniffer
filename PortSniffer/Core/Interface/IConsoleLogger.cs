using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Core.Interface
{
    public interface IConsoleLogger
    {
        void Warn(string msg);
        void Log(string msg);
        void Error(string msg);

        void CLear();
    }
}
