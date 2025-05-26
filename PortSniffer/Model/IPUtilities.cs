using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Model
{
    public static class IPUtilities
    {
        //IP

        /// <summary>
        /// Validates if provided string is a valid IP address.
        /// </summary>
        /// <param name="input">input to validate</param>
        /// <param name="ip">out Parameter for IP Adress</param>
        /// <returns>Treu if ipnut is valid IP Adress, otherwise false</returns>
        public static bool ValidateIP(string input, out IPAddress? ip)
        {
            ip = null;
            input = input.Trim();

            if (string.IsNullOrWhiteSpace(input)) return false;//redundant?

            if (IPAddress.TryParse(input, out ip))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if first IP is greater than second IP.
        /// </summary>
        /// <param name="ip1">First IP</param>
        /// <param name="ip2">Second IP</param>
        /// <returns>True if f</returns>
        public static bool IsGreaterThan(IPAddress ip1, IPAddress ip2)
        {
            byte[] bytes1 = ip1.GetAddressBytes();
            byte[] bytes2 = ip2.GetAddressBytes();

            for (int i = 0; i < bytes1.Length; i++)
            {
                if (bytes1[i] > bytes2[i]) return true;
                if (bytes1[i] < bytes2[i]) return false;
            }

            return false;
        }

        /// <summary>
        /// Creates lsit of IP Adresses from provided range.
        /// </summary>
        /// <remarks>IP range 'start' and 'end' MUST be in correct order 'start' < 'end' </remarks>
        /// <param name="start">IP Adress representing the start of a range</param>
        /// <param name="end">IP Adress representing the end of a range</param>
        /// <returns></returns>
        public static List<IPAddress> GetIPAddressesFromRange(IPAddress start, IPAddress end)
        {
            List<IPAddress> addresses = new List<IPAddress>();

            uint s = IpToUint32(start);
            uint e = IpToUint32(end);

            for (uint i = s; i <= e; i++)
            {
                addresses.Add(Uint32ToIp(i));
            }

            return addresses;
        }

        /// <summary>
        /// Converts uint to IPAddress.
        /// Uses Big Endian format => can correctly add 1 to it (192.168.0.1 +1 => 192.168.0.2)
        /// </summary>
        /// <param name="ip">IP Adress to convert</param>
        /// <returns>IP Adress converted to uint32</returns>
        public static uint IpToUint32(IPAddress ip)
        {
            byte[] bytes = ip.GetAddressBytes();
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Converts uint to IPAddress.
        /// Uses Big Endian format.
        /// </summary>
        /// <param name="value">uint value</param>
        /// <returns>IP adress</returns>
        public static IPAddress Uint32ToIp(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return new IPAddress(bytes);
        }

        //SUBNET MASK
        //validating mask by flipping bits is too complex so im using this list 🙏😩😭🥀🥀.
        private static readonly string[] ValidMasks =
        [
            "128.0.0.0", "192.0.0.0", "224.0.0.0", "240.0.0.0",
            "248.0.0.0", "252.0.0.0", "254.0.0.0", "255.0.0.0",
            "255.128.0.0", "255.192.0.0", "255.224.0.0", "255.240.0.0",
            "255.248.0.0", "255.252.0.0", "255.254.0.0", "255.255.0.0",
            "255.255.128.0", "255.255.192.0", "255.255.224.0",
            "255.255.240.0", "255.255.248.0", "255.255.252.0",
            "255.255.254.0", "255.255.255.0", "255.255.255.128",
            "255.255.255.192", "255.255.255.224", "255.255.255.240",
            "255.255.255.248", "255.255.255.252", "255.255.255.254",
            "255.255.255.255"
        ];

        /// <summary>
        /// Validates subnet mask in CIDR or normal format.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static bool ValidateSubnetMask(string input, out IPAddress? mask)
        {
            mask = null;
            input = input.Trim();

            if (string.IsNullOrWhiteSpace(input)) return false;

            // CIDR
            if (input.Contains('/'))
            {
                string[] split = input.Split('/');
                if (split.Length != 2) return false;

                if (int.TryParse(split[1], out int prefix))
                {
                    if (prefix > 0 && prefix <= 32)
                    {
                        mask = IPAddress.Parse(ValidMasks[prefix - 1]);
                        return true;
                    }
                }
            }
            else //normal
            {
                if (IPAddress.TryParse(input, out IPAddress? parsedMask))
                {
                    if (ValidMasks.Contains(parsedMask.ToString()))
                    {
                        mask = parsedMask;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
