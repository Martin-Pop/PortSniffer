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
    /// <summary>
    /// UI for the output console, displays messages (logs, errors, warnings)
    /// </summary>
    public class OutputConsoleView : PanelAbstract, IOutpuConsoleView
    {
        private readonly RichTextBox console;
        public OutputConsoleView(Settings settings): base(settings)
        {
            Dock = DockStyle.Fill;

            console = new RichTextBox()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BorderStyle = BorderStyle.Fixed3D
            };
            
            console.KeyDown += HandleConsoleInput;

            ApplySettings();
            Controls.Add(console);
        }

        /// <summary>
        /// Writes a message to the console output with specified color.
        /// </summary>
        /// <param name="msg">Message to write</param>
        /// <param name="color">Color the message</param>
        public void Write(string msg, Color color)
        {
            if (console.IsDisposed) return;

            if (console.InvokeRequired)
            {
                console.BeginInvoke(new Action(() => Write(msg, color)));
                return;
            }

            console.AppendText("\n>> ");
            console.SelectionStart = console.TextLength;
            console.SelectionLength = 0;

            console.SelectionColor = color; 
            console.AppendText(msg);
            console.SelectionColor = console.ForeColor;

            //hoefully fixes crash if user minimeses the window at before the invoke :pray:
            if (console.IsHandleCreated)
            {
                console.BeginInvoke(new Action(console.ScrollToCaret));
            }
        }

        /// <summary>
        /// Clears the console output
        /// </summary>
        public void Clear()
        {
            console.Clear();
        }

        /// <summary>
        /// Hnaldes key input to prevent beeping when user types something even though is ReadOnly
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event arguments</param>
        private void HandleConsoleInput(object? sender, KeyEventArgs e)
        {   
            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        /// <summary>
        /// Applies settings
        /// </summary>
        public override void ApplySettings()
        {
            console.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
            console.BackColor = ColorTranslator.FromHtml(Settings.ConsoleBackgroundColor);
        }
    }
}
