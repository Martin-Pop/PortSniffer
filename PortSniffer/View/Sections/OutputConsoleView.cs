using PortSniffer.Core.Config;
using PortSniffer.View.Abstract;
using PortSniffer.View.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Sections
{
    public class OutputConsoleView : PanelAbstract, IOutpuConsoleView
    {
        private readonly RichTextBox console;
        public OutputConsoleView(Settings settings): base(settings)
        {
            Dock = DockStyle.Fill;

            console = new RichTextBox();
            console.Dock = DockStyle.Fill;
            console.ReadOnly = true;
            console.BorderStyle = BorderStyle.Fixed3D;
            ApplySettings();

            console.KeyDown += HandleConsoleInput;

            //TODO: add clear button to control panel
            Controls.Add(console);
        }

        public override void ApplySettings()
        {
            console.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
            console.BackColor = ColorTranslator.FromHtml(Settings.ConsoleBackgroundColor);
        }


        public void Write(string msg, Color color)
        {
            console.AppendText(">> ");
            console.SelectionStart = console.TextLength;
            console.SelectionLength = 0;

            console.SelectionColor = color; 
            console.AppendText(msg + "\n");
            console.SelectionColor = console.ForeColor;

            //hoefully fixes crash if user minimeses the window at before the invoke: pray:
            if (console.IsHandleCreated)
            {
                console.BeginInvoke(new Action(console.ScrollToCaret));
            }
        }

        public void Clear()
        {
            console.Clear();
        }

        private void HandleConsoleInput(object? sender, KeyEventArgs e)
        {   
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
    }
}
