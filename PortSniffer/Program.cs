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

            OutputConsoleView outputConsoleView = new OutputConsoleView();
            OutputConsolePresenter outputConsolePresenter = new OutputConsolePresenter(outputConsoleView);

            ControlPanelView controlPanelView = new ControlPanelView();
            ScanControlsPresenter scanControlsPresenter = new ScanControlsPresenter(controlPanelView, outputConsolePresenter);

            mainForm.AddViews(controlPanelView, outputConsoleView);

            Application.Run(mainForm);

            //TODO: make settings and new interface for reaplying settings? => settings form :weary: ?
            // first load setings if scuccess apply them else apply default, then in console log result
            
        }

       

    }
}