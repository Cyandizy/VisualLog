using System.Collections.Generic;
using VisualLog.Models;
using WinForms = System.Windows.Forms;

namespace VisualLog.ViewModels
{
    public class VisualLogViewModel
    {
        private LogDataService logDataService;

        public VisualLogViewModel()
        {
            logDataService = new LogDataService();
            LoadDataFromFile();
        }

        private void LoadFile()
        {
            logDataService.LoadFile();
        }

        public void SaveData()
        {
            logDataService.SaveData();
        }

        public void LoadDataFromFile()
        {
            logDataService.LoadDataFromFile();
        }

        public List<string> GetDataList()
        {
            return logDataService.GetDataList(); 
        }

        public void AppendData(string userInput)
        {
            if (LogData.DataList != null)
            {
                logDataService.AppendData(userInput);
            }
        }

        public void DeleteData(string userInput)
        {
            if (LogData.DataList != null)
            {
                logDataService.DeleteData(userInput);
            }
        }

        public bool HasData()
        {
            return logDataService.HasData();
        }


        public void ChooseSavePath()
        {
            WinForms.FolderBrowserDialog folderBrowserDialog = new WinForms.FolderBrowserDialog();
            WinForms.OpenFileDialog openFileDialog = new WinForms.OpenFileDialog();
            WinForms.DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == WinForms.DialogResult.OK)
            {
                string folderName = folderBrowserDialog.SelectedPath;
                DebugService.Log(folderName);
            }

        }
    }
}
