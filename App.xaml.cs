using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using VisualLog.Models;
using System;


namespace VisualLog
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private LogDataService _logDataService;

        public App()
        {
            _logDataService = new LogDataService();
            this.Exit += new ExitEventHandler(App_Exit);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            if (!String.IsNullOrEmpty(LogData.FilePath) && LogData.DataList.Count != 0)
            {
                _logDataService.SaveData();
            }
        }
    }

}
