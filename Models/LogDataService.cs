using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System;
using System.Windows.Documents;
using WinForms = System.Windows.Forms;

namespace VisualLog.Models
{
    internal class LogDataService
    {
        public LogDataService() 
        {

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
                            DebugService.Log($"Data loaded from {LogData.FilePath}");
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

        public void ChooseSavePath()
        {
            WinForms.FolderBrowserDialog folderBrowserDialog = new WinForms.FolderBrowserDialog();
            WinForms.OpenFileDialog openFileDialog = new WinForms.OpenFileDialog();
            WinForms.DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == WinForms.DialogResult.OK)
            {
                string chosenPath = folderBrowserDialog.SelectedPath;
                chosenPath = $"{chosenPath}\\{ApplicationSettings.SaveFileName}";
                DebugService.Log($"Save path set to {chosenPath}");
                MoveSavePath(chosenPath);
            }
        }

        public void MoveSavePath(string newPath)
        {
            LogData.FilePath = newPath;

            if (File.Exists(LogData.FilePath))
            {
                LoadDataFromFile();
            }
            ApplicationSettings.SaveSettings();
        }

    }
}
