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
        public static string logNumberD3 = string.Empty;
        public static bool debugMode = true;

        public static void LoadData()
        {
            string dataFile = "LogData.json";
            if (File.Exists(dataFile))
            {
                try
                {
                    string dataJson = File.ReadAllText(dataFile);
                    if (dataJson != null)
                    {
                        logData = JsonSerializer.Deserialize<List<string>>(dataJson)!;
                        log("Data loaded");

                        return;
                    }
                }
                catch
                {
                    log("Saved data not found or damaged");
                }
            }
            return;
        }

        public static void SaveData()
        {
            string dataFile = "LogData.json";
            using FileStream createStream = File.Create(dataFile);
            JsonSerializer.SerializeAsync(createStream, logData);

            log("Data saved to json");
        }

        public static void AppendData(string userInput)
        {
            logData.Add(userInput);
            log("Data appended to the list");
        }

        public static void DeleteData(string userInput)
        {
            logData.Remove(userInput);
            log("Data deleted from the list");
        }

        public static void log(string message)
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
