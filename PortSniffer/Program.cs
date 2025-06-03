using PortSniffer.Model.Config;
using PortSniffer.Model.Scanner;
using PortSniffer.Presenter;
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
            bool success = settingsManager.ReadSettings(out string message);
            //settingsManager.SaveSettings(out string message11);


            //port scanner
            PortScanner portScanner = new PortScanner();

            //main form
            MainForm mainForm = new MainForm();

            //output console
            OutputConsoleView outputConsoleView = new OutputConsoleView(settingsManager.Settings);

            //scan properties
            ScanPropertiesView scanPropertiesView = new ScanPropertiesView(settingsManager.Settings);
            ScanPropertiesPresenter scanPropertiesPresenter = new ScanPropertiesPresenter(scanPropertiesView, outputConsoleView);

            //scan results
            ScanResultsView scanResultsView = new ScanResultsView(settingsManager.Settings);
            ScanResultsPresenter scanResultsPresenter = new ScanResultsPresenter(portScanner, scanResultsView, settingsManager.Settings);

            //scan controls
            ControlPanelView controlPanelView = new ControlPanelView(settingsManager.Settings);
            ControlPanelPresenter controlPanelPresenter = new ControlPanelPresenter(controlPanelView, scanPropertiesView, outputConsoleView, scanResultsView, portScanner);

            mainForm.AddViews(scanPropertiesView, outputConsoleView, controlPanelView, scanResultsView);

            if (success)
            {
                outputConsoleView.Log(message);
            }
            else
            {
                outputConsoleView.Warn(message);
            }

            outputConsoleView.Log("Port Sniffer has successfully started. Welcome!");
            Application.Run(mainForm);
        }
    }
}