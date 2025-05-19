using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortSniffer.View.ScanProperties
{
    public class PropertyTextInput : TextBox
    {
        private bool isValid;
        public bool IsValid { get => isValid; set => isValid = value; }
        public PropertyTextInput()
        {
            Initialize();
        }

        public PropertyTextInput(string placeholder)
        {
            PlaceholderText = placeholder;
            Initialize();
        }

        private void Initialize()
        {
            Font = new Font("Consolas", 11F, FontStyle.Regular);
            Multiline = false;
        }
    }
}
