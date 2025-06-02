using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Model.Scanner
{
    /// <summary>
    /// Represents open port with time it was scanned.
    /// </summary>
    public class OpenPort
    {
        private int portNumber;
        private TimeOnly scannedAt;

        public OpenPort(int portNumber, TimeOnly scannedAt)
        {
            this.portNumber = portNumber;
            this.scannedAt = scannedAt;
        }

        public override string? ToString()
        {
            return $"{portNumber} -- ({scannedAt.ToString("HH:mm:ss")})";
        }
    }
}
