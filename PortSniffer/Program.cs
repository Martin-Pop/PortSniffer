using PortSniffer.Core.Config;
using PortSniffer.Model;
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
            
            //settings
            SettingsManager settingsManager = new SettingsManager(Path.Combine(Application.StartupPath, "config.json"));
            settingsManager.SaveSettings(out string message);

            //port scanner
            PortScanner portScanner = new PortScanner();

            //main form
            MainForm mainForm = new MainForm();

            //output console
            OutputConsoleView outputConsoleView = new OutputConsoleView(settingsManager.Settings);
            OutputConsolePresenter outputConsolePresenter = new OutputConsolePresenter(outputConsoleView);

            //scan properties
            ScanPropertiesView scanPropertiesView = new ScanPropertiesView(settingsManager.Settings);
            ScanPropertiesPresenter scanPropertiesPresenter = new ScanPropertiesPresenter(scanPropertiesView, outputConsolePresenter);

            //scan controls
            ControlPanelView controlPanelView = new ControlPanelView(settingsManager.Settings);
            ControlPanelPresenter controlPanelPresenter = new ControlPanelPresenter(controlPanelView, scanPropertiesView, outputConsolePresenter, portScanner);


            //scan results
            //not yet :\

            mainForm.AddViews(scanPropertiesView, outputConsoleView, controlPanelView);
            Application.Run(mainForm);

            //TODO: make settings and new interface for reaplying settings? => settings form :weary: ?
            // first load setings if scuccess apply them else apply default, then in console log result
            
        }

       

    }
}