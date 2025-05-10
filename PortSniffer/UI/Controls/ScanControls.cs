using PortSniffer.Models;
using PortSniffer.UI.ScanProperties;
using System;

namespace PortSniffer.UI.Controls
{
    public class ScanControls : TableLayoutPanel
    {
        private TextBox addressBox = new TextBox();

        //List<IScanSetting> scanSettings = new List<IScanSetting>();
        public ScanControls()
        {

            this.Dock = DockStyle.Fill;
            this.ColumnCount = 1;
            this.RowCount = 10;
            this.AutoScroll = true;

           InitializeControls();
        }

        public void InitializeControls()
        {
            //IP ADDRESS TARGET
            PropertyLabel targetLabel = new PropertyLabel("Target IP:");
            PropertyTooltip targetHelp = new PropertyTooltip("Type in your target IPv4 address");
            PropertyTextInput targetTextbox = new PropertyTextInput("192.168.0.1");
            IPAddressProperty targetIP = new IPAddressProperty(targetLabel, targetHelp, targetTextbox);
            
            Controls.Add(targetIP);
        }

        public ScanConfig? ConstructConfigOrNull()
        {
           throw new NotFiniteNumberException();
        }
    }


}
