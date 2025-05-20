using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Model
{
    public static class IPValidator
    {
        //IP

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

        //SUBNET MASK

        //validating by flipping bits is too complex so im using this list.
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
