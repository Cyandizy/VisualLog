using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace VisualLog
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Exit += new ExitEventHandler(App_Exit);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            TextLogModel.SaveData();

        }
    }

}
