using PortSniffer.Presenter;
//using PortSniffer.UI;
using PortSniffer.View;
using PortSniffer.View.Sections;
using System.Diagnostics;
using System.Net;

namespace PortSniffer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            
            MainForm mainForm = new MainForm();
            
            ControlPanelView controlPanelView = new ControlPanelView();
            ScanControlsPresenter scanControlsPresenter = new ScanControlsPresenter(controlPanelView);

            Debug.WriteLine("Adding view?");
            mainForm.AddViews(controlPanelView);

            Application.Run(mainForm);

        }

       

    }
}