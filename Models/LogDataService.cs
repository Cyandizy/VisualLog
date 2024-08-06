using System.IO;
using System.Text.Json;

namespace VisualLog.Models
{
    internal class LogDataService
    {
        public LogDataService() 
        {
            LoadFile();
        }

        public void LoadFile()
        {
            string dataFile;

            string? path = Environment.GetEnvironmentVariable("MyData");
            if (!string.IsNullOrEmpty(path))
            {
                DebugService.Log($"File loaded from {path}");
                dataFile = Path.Combine(path, "TextLogData.json");
            }
            else
            {
                DebugService.Log("Custom save location not set up, using the default setting");
                dataFile = "LogData.json";
            }

            LogData.FilePath = dataFile;
        }

        public List<string> LoadDataFromFile()
        {
            if (File.Exists(LogData.FilePath))
            {
                try
                {
                    string dataJson = File.ReadAllText(LogData.FilePath);
                    if (!string.IsNullOrEmpty(dataJson))
                    {
                        var logData = JsonSerializer.Deserialize<List<string>>(dataJson);
                        if (logData != null)
                        {
                            DebugService.Log("Data loaded");
                            LogData.DataList = logData;
                            return logData;
                        }
                    }
                }
                catch (JsonException ex)
                {
                    DebugService.Log($"Saved data not found or damaged : {ex.Message}");
                }
                catch (IOException ex)
                {
                    DebugService.Log($"Error reading the file: {ex.Message}");
                }
            }
            else
            {
                DebugService.Log("Data file does not exist.");
            }

            LogData.DataList = new List<string>();
            return new List<string>();
        }

        public void SaveData()
        {
            if (File.Exists(LogData.FilePath))
            {
                DebugService.Log("File does not exist, the data will be written in a new file.");
            }
            try
            {
                using FileStream createStream = File.Create(LogData.FilePath);
                JsonSerializer.Serialize(createStream, LogData.DataList);
                DebugService.Log("Data saved to json");
            }
            catch (IOException ex)
            {
                DebugService.Log($"Error writing the file: {ex.Message}");
            }
        }

        public void AppendData(string userInput)
        {
            LogData.DataList.Add(userInput);
            DebugService.Log("Data appended to the list");
        }

        public void DeleteData(string userInput)
        {
            LogData.DataList.Remove(userInput);
            DebugService.Log("Data deleted from the list");
        }
        public bool HasData()
        {
            return LogData.DataList != null && LogData.DataList.Count != 0;
        }

        public List<string> GetDataList()
        {
            return LogData.DataList;
        }

    }
}
