﻿using PortSniffer.Model.Config;
using PortSniffer.View.Interface;
using PortSniffer.View.ScanProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Abstract
{
    /// <summary>
    /// Abstract class for every scan property that is a checkbox.
    /// </summary>
    public abstract class ScanPropertyCheckBoxAbstract : PanelAbstract
    {
        protected FlowLayoutPanel labelPanel;
        public Label Label { get; init; }
        public PropertyTooltip Tooltip { get; init; }
        public CheckBox Input { get; protected set; }
        public ScanPropertyCheckBoxAbstract(string label, string toolTipMessage, Settings settings) : base(settings)
        {
            //this
            AutoSize = true;
            Dock = DockStyle.Fill;

            //panel
            labelPanel = new FlowLayoutPanel();
            labelPanel.FlowDirection = FlowDirection.LeftToRight;
            labelPanel.WrapContents = false; //important
            labelPanel.AutoSize = true;
            labelPanel.Dock = DockStyle.Top;

            //label
            Label = new Label();
            Label.Text = label;
            Label.TextAlign = ContentAlignment.MiddleLeft;
            Label.AutoSize = true;
            Label.Dock = DockStyle.Left;

            //tooltip
            Tooltip = new PropertyTooltip(toolTipMessage);
            Tooltip.Margin = new Padding(5, 0, 0, 0);

            //input
            Input = new CheckBox();
            Input.AutoSize = true;
            Input.Dock = DockStyle.Left;

            labelPanel.Controls.Add(Label);
            labelPanel.Controls.Add(Input);
            labelPanel.Controls.Add(Tooltip);

            ApplySettings();

            Controls.Add(labelPanel);
        }

        public override void ApplySettings()
        {
            Label.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
            Input.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
        }
    }
}
