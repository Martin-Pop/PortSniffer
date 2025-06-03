using PortSniffer.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortSniffer.Model.Config
{
    /// <summary>
    /// Manages the currect settings of the application, reading it and saving it to a file.
    /// </summary>
    public class SettingsManager : ISettingsManager
    {
        private readonly string PATH;
        public Settings Settings { get; set; }

        public SettingsManager(string filePath)
        {
            PATH = filePath;
            Settings = new Settings();
        }

        /// <summary>
        /// Reads the settings from a file and applies them to the Settings property.
        /// </summary>
        /// <param name="message">message whenever it was a success or nah + error msg</param>
        /// <returns>True if setting was read and applied, otherwise false</returns>
        public bool ReadSettings(out string message)
        {
            try
            {
                string json = File.ReadAllText(PATH);
                Settings? s = JsonSerializer.Deserialize<Settings>(json);

                if (s != null)
                {
                    Settings = s;
                    message = "\"Successfully read config file. Correctly written entries were used.";
                    return true;
                }
                else // if file is null or isnt object {} 
                {
                    message = "Failed to deserialize settings. Default settings were applied.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Error has occured while reading the settings, default settings were applied: ERROR MESSAGE:[ "+ ex.Message +" ]";
                return false;
            }
        }

        //save settings is not used because i didnt make the settings form, configuration must be done from config file

        /// <summary>
        /// Saves current settings to a file.
        /// </summary>
        /// <param name="message">message whenever it was a success or nah + error msg</param>
        /// <returns>True if setting was saved correctly, otherwise false</returns>
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
