using PortSniffer.Core.Config;
using PortSniffer.View.Abstract;
using PortSniffer.View.Interface;

namespace PortSniffer.View.Sections
{
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

            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.RowCount = 3;
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.AutoScroll = false;
           
            Start = new Button();
            Start.Text = "Start";
            Start.Dock = DockStyle.Fill;
            Start.Height = 35;
            Start.TextAlign = ContentAlignment.MiddleCenter;

            Stop = new Button();
            Stop.Text = "Stop";
            Stop.Dock = DockStyle.Fill;
            Stop.Height = 35;
            Stop.TextAlign = ContentAlignment.MiddleCenter;

            PauseResume = new Button();
            PauseResume.Text = "Pause";
            PauseResume.Dock = DockStyle.Fill;
            PauseResume.Height = 35;
            PauseResume.TextAlign = ContentAlignment.MiddleCenter;
            PauseResume.Enabled = false;

            Clear = new Button();
            Clear.Text = "Clear";
            Clear.Dock = DockStyle.Fill;
            Clear.Height = 35;
            Clear.TextAlign = ContentAlignment.MiddleCenter;

            ApplySettings();

            tableLayoutPanel.Controls.Add(Start);
            tableLayoutPanel.Controls.Add(Stop);
            tableLayoutPanel.Controls.Add(PauseResume);
            tableLayoutPanel.Controls.Add(Clear);

            Controls.Add(tableLayoutPanel);
    
        }

        //private void UpdateButtonWidths()
        //{
        //    foreach (Control control in flowLayoutPanel.Controls)
        //    {
        //        if (control is Button b)
        //        {
        //            b.Width = flowLayoutPanel.ClientSize.Width;
        //        }
        //    }
        //}

        public override void ApplySettings()
        {
            Start.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
            Stop.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
            PauseResume.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
            Clear.Font = new Font(Settings.FontFamily, Settings.FontSize, FontStyle.Regular);
        }
    }
}
