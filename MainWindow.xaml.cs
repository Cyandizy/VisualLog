using System.Windows;
using VisualLog.Views;

namespace VisualLog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new VisualLogView());
        }
    }
}
