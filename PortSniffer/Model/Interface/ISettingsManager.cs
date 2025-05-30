using PortSniffer.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Model.Interface
{
    public interface ISettingsManager
    {
        Settings Settings { get; }
        bool SaveSettings(out string message);
        bool ReadSettings(out string message);
    }
}
