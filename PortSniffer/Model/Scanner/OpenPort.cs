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
        public int PortNumber { get; private set; }
        public TimeOnly ScannedAt { get; private set; }

        public OpenPort(int portNumber, TimeOnly scannedAt)
        {
            PortNumber = portNumber;
            ScannedAt = scannedAt;
        }

        public override string? ToString()
        {
            return $"{PortNumber} -- ({ScannedAt.ToString("HH:mm:ss")})";
        }
    }
}
