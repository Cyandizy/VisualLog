﻿using System.Collections.Generic;
using VisualLog.Models;


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
            logDataService.ChooseSavePath();
        }
    }
}
