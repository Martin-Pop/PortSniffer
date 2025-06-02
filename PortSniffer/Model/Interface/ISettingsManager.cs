using PortSniffer.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Model.Interface
{
    /// <summary>
    /// Interface for managing application settings.
    /// </summary>
    public interface ISettingsManager
    {
        /// <summary>
        /// Current settings
        /// </summary>
        Settings Settings { get; }

        bool SaveSettings(out string message); //save settings
        bool ReadSettings(out string message); //read settings
    }
}
