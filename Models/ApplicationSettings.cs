using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System;

namespace VisualLog.Models
{
    public static class ApplicationSettings
    {
        public static bool DebugMode = false; //true allows debug messages
        public static string SaveFileName = "TextLogData.json";
        public static string SavePath = string.Empty;

        public static void LoadSettings()
        {
            string settingsFile = "ApplicationSettings.json";
            if (File.Exists(settingsFile))
            {
                try
                {
                    string settingsJson = File.ReadAllText(settingsFile);
                    if (!string.IsNullOrEmpty(settingsJson))
                    {
                        var settingsDict = JsonSerializer.Deserialize<Dictionary<string, string>>(settingsJson);
                        if (settingsDict != null)
                        {
                            DebugMode = Convert.ToBoolean(settingsDict["DebugMode"]);
                            LogData.FilePath = settingsDict["SavePath"];
                        }
                    }
                }
                catch (JsonException ex)
                {
                    DebugService.Log($"settings data not found or damaged : {ex.Message}");
                }
                catch (IOException ex)
                {
                    DebugService.Log($"Error reading settings file: {ex.Message}");
                }
            }
            else
            {
                DebugMode = false;
                SavePath = SaveFileName;
                ApplicationSettings.SaveSettings();
            }

        }

        public static void SaveSettings()
        {
            SavePath = LogData.FilePath;

            try
            {
                Dictionary<string, string> settingsDict = new Dictionary<string, string>()
                {
                    ["DebugMode"] = DebugMode.ToString(),
                    ["SavePath"] = SavePath,
                };

                using FileStream createStream = File.Create("ApplicationSettings.json");
                JsonSerializer.Serialize(createStream, settingsDict);
            }
            catch (IOException ex)
            {
                DebugService.Log($"Error writing the file: {ex.Message}");
            }
        }
    }
}
