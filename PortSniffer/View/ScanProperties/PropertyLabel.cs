using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.ScanProperties
{
    public class PropertyLabel : Label
    {
        public PropertyLabel(string text)
        {
            Text = text;
            Font = new Font("Consolas", 12F, FontStyle.Regular);
            TextAlign = ContentAlignment.MiddleLeft;
        }
    }
}
