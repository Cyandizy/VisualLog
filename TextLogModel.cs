using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace VisualLog
{
    public class TextLogModel
    {
        public static List<string> logData = new List<string>();
        private static int logNumber = 0;
        public static string logNumberD3 = "";
        public static bool debugMode = false;
        private static bool customSaveLocation = true;

        private static string LoadFile()
        {
            string dataFile;
            if (customSaveLocation)
            {
                string? path = Environment.GetEnvironmentVariable("MyData");
                if (!string.IsNullOrEmpty(path))
                {
                    Log($"File loaded from {path}");
                    dataFile = Path.Combine(path, "TextLogData.json");
                    return dataFile;
                }
                else
                {
                    Log("Custom save location not set up, using the default setting");
                    dataFile = "LogData.json";
                    return dataFile;
                }
            }
            else
            {
                Log("File loaded");
                dataFile = "LogData.json";
                return dataFile;
            }
        }

        public static void LoadData()
        {
            string dataFile = LoadFile();
            if (File.Exists(dataFile))
            {
                try
                {
                    string dataJson = File.ReadAllText(dataFile);
                    if (dataJson != null)
                    {
                        logData = JsonSerializer.Deserialize<List<string>>(dataJson)!;
                        Log("Data loaded");

                        return;
                    }
                }
                catch (JsonException ex)
                {
                    Log($"Saved data not found or damaged : {ex.Message}");
                }
                catch (IOException ex)
                {
                    Log($"Error reading the file: {ex.Message}");
                }
            }
            else
            {
                Log("Data file does not exist.");
            }
        }

        public static void SaveData()
        {
            string dataFile = LoadFile();
            if (File.Exists(dataFile))
            {
                try
                {
                    using FileStream createStream = File.Create(dataFile);
                    JsonSerializer.Serialize(createStream, logData);
                    Log("Data saved to json");
                }
                catch (IOException ex)
                {
                    Log($"Error writing the file: {ex.Message}");
                }
            }
        }

        public static void AppendData(string userInput)
        {
            logData.Add(userInput);
            Log("Data appended to the list");
        }

        public static void DeleteData(string userInput)
        {
            logData.Remove(userInput);
            Log("Data deleted from the list");
        }

        public static void Log(string message)
        {
            if (debugMode)
            {
                DateTime localTime = DateTime.Now;
                var culture = new CultureInfo("en-US");
                logNumber += 1;
                logNumberD3 = logNumber.ToString("D3");

                Debug.WriteLine($"{localTime.ToString(culture)} |{logNumberD3}| {message}");
            }
            return;
        }
    }
}
