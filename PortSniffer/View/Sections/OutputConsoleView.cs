using PortSniffer.View.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.View.Sections
{
    public class OutputConsoleView : Panel, IOutpuConsoleView
    {
        private readonly RichTextBox console;
        private readonly Button clearButton;
        public OutputConsoleView()
        {
            Dock = DockStyle.Fill;

            console = new RichTextBox();
            console.Dock = DockStyle.Fill;
            console.ReadOnly = true;
            console.BorderStyle = BorderStyle.Fixed3D;
            console.Font = new Font("Consolas", 11F, FontStyle.Regular);

            console.KeyDown += HandleConsoleInput;


            clearButton = new Button();
            clearButton.Text = "R";
            clearButton.Width = 25;
            clearButton.Height = 25;
            //TODO: add icon + make this buttun iside controls and not here
            //Controls.Add(clearButton);
            Controls.Add(console);

            clearButton.BringToFront();
        }

        public void Write(string msg, Color color)
        {
            console.SelectionStart = console.TextLength;
            console.SelectionLength = 0;

            console.SelectionColor = color; 
            console.AppendText(msg + "\n");
            console.SelectionColor = console.ForeColor; 

            console.ScrollToCaret();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        private void HandleConsoleInput(object? sender, KeyEventArgs e)
        {
            
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
    }
}
