using PortSniffer.Core.Config;
using PortSniffer.View.Abstract;
using PortSniffer.View.Interface;

namespace PortSniffer.View.Sections
{
    /// <summary>
    /// UI for the control panel, which contains buttons to start, stop, pause/resume, and clear the scan.
    /// </summary>
    public class ControlPanelView : PanelAbstract, IControlPanelView
    {
        private readonly TableLayoutPanel tableLayoutPanel;
        public Button Start { get; private set; }
        public Button Stop { get; private set; }
        public Button PauseResume { get; private set; }
        public Button Clear { get; private set; }

        public ControlPanelView(Settings settings) : base(settings)
        {
            Dock = DockStyle.Bottom;
            AutoSize = true;

            tableLayoutPanel = new TableLayoutPanel()
            {
                RowCount = 4,
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoScroll = false
            };

            Start = new Button()
            {
                Text = "Start",
                Dock = DockStyle.Fill,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Stop = new Button()
            {
                Text = "Stop",
                Dock = DockStyle.Fill,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter,
                Enabled = false
            };

            PauseResume = new Button()
            {
                Text = "Pause",
                Dock = DockStyle.Fill,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter,
                Enabled = false
            };

            Clear = new Button()
            {
                Text = "Clear",
                Dock = DockStyle.Fill,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter
            };
           
            ApplySettings();

            tableLayoutPanel.Controls.Add(Start);
            tableLayoutPanel.Controls.Add(Stop);
            tableLayoutPanel.Controls.Add(PauseResume);
            tableLayoutPanel.Controls.Add(Clear);

            Controls.Add(tableLayoutPanel);
        }

        /// <summary>
        /// Applies settings to the controls (UI)
        /// </summary>
        public override void ApplySettings()
        {
            Start.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
            Stop.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
            PauseResume.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
            Clear.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
        }
    }
}
