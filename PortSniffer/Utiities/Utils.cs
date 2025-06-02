using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Core
{
    /// <summary>
    /// Utility class for common functions used across the application.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Returns the current time, Makes the code look better thats why I made this. 
        /// </summary>
        /// <returns>current time in HH:mm:ss format</returns>
        public static string TimeNow()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
    }

    // i thought i will add more to this class but i didnt so now it seems useless :/
}
