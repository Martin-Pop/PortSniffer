using PortSniffer.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortSniffer.Model.Config
{
    public class SettingsManager : ISettingsManager
    {
        private readonly string PATH;
        public Settings Settings { get; set; }

        public SettingsManager(string filePath)
        {
            PATH = filePath;
            Settings = new Settings();
        }

        public bool ReadSettings(out string message)
        {
            try
            {
                string json = File.ReadAllText(PATH);
                Settings? s = JsonSerializer.Deserialize<Settings>(json);

                if (s != null)
                {
                    Settings = s;
                    message = "Successfully loaded settings from file.";
                    return true;
                }
                else
                {
                    message = "Failed to deserialize settings. Default settings were applied.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Error has occured while reading the settings, default settings were applied: " + ex.Message;
                return false;
            }
        }

        public bool SaveSettings(out string message)
        {
            try
            {
                string json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(PATH, json);

                message = "Settings were successfully saved to file";
                return true;
            }
            catch (Exception ex)
            {
                message = "Error has occured while saving settings: " + ex.Message;
                return false;
            }
        }
    }
}
