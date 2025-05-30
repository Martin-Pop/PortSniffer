using PortSniffer.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Abstract
{
    /// <summary>
    /// Abstract class for every panel that needs to be customized from settings.
    /// </summary>
    public abstract class PanelAbstract : Panel
    {
        protected Settings Settings { get; set; }
        protected PanelAbstract(Settings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Apply settings to UI elements
        /// </summary>
        public abstract void ApplySettings();
    }
}
