using System.ComponentModel;
using VisualLog.Models;

namespace VisualLog.ViewModels
{
    public class VisualLogViewModel
    {
        private LogDataService _logDataService;

        public VisualLogViewModel()
        {
            _logDataService = new LogDataService();
            LoadDataFromFile();
        }

        private void LoadFile()
        {
            _logDataService.LoadFile();
        }

        public void SaveData()
        {
            _logDataService.SaveData();
        }

        public void LoadDataFromFile()
        {
            _logDataService.LoadDataFromFile();
        }

        public List<string> GetDataList()
        {
            return _logDataService.GetDataList();
        }

        public void AppendData(string userInput)
        {
            if (LogData.DataList != null)
            {
                _logDataService.AppendData(userInput);
            }
        }

        public void DeleteData(string userInput)
        {
            if (LogData.DataList != null)
            {
                _logDataService.DeleteData(userInput);
            }
        }

        public bool HasData()
        {
            return _logDataService.HasData();
        }

    }
}
