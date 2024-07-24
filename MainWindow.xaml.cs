using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace VisualLog
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TextLogModel.LoadData();

            LoadData(TextLogModel.logData);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length > 0)
            {
                string userInput = GetUserInput();
                CreateLog(userInput);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text = string.Empty;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn) {
                var textGrid = btn.Parent as Grid;
                btn.Click -= Delete_Click;

                if (textGrid != null)
                {
                    var textBlock = textGrid.Children.OfType<TextBlock>().FirstOrDefault();
                    var textPanel = textGrid.Parent as StackPanel;

                    if (textBlock != null && textPanel != null && textBlock.Tag != null)
                    {
                        string tagValue = textBlock.Tag.ToString()!;
                        TextLogModel.DeleteData(tagValue);

                        textPanel.Children.Remove(textGrid);
                    }
                }
            }
        }
        private string GetUserInput()
        {
            DateTime localTime = DateTime.Now;
            var culture = new CultureInfo("en-US");
            string userInput = $"{localTime.ToString("yyyy-MM-dd hh:mm:ss tt", culture)}: {textBox.Text}";
            TextLogModel.AppendData(userInput);

            textBox.Text = string.Empty;
            TextLogModel.log("textBox emptied");

            return userInput;
        }

        private void CreateLog(string text)
        {
            Grid textGrid = new Grid();

            ColumnDefinition col1 = new ColumnDefinition()
            {
                Width = new GridLength(9, GridUnitType.Star)
            };
            ColumnDefinition col2 = new ColumnDefinition()
            {
                Width = new GridLength(1, GridUnitType.Star)
            };

            textGrid.ColumnDefinitions.Add(col1);
            textGrid.ColumnDefinitions.Add(col2);
            TextLogModel.log("CreateLog (1/6): Columns defined");

            string time = text.Substring(0, 23);
            string content = text.Substring(23);

            Run timePortion = new Run(time)
            {
                Foreground = new BrushConverter().ConvertFrom("#045de1") as SolidColorBrush,
                FontWeight = FontWeights.Bold
            };

            Run contentPortion = new Run(content)
            {
                Foreground = new BrushConverter().ConvertFrom("#cccccc") as SolidColorBrush
            };

            TextBlock textBlock = new TextBlock()
            {
                Inlines = {
                    timePortion,
                    contentPortion
                },
                FontSize = 15,
                TextWrapping = TextWrapping.Wrap,
                Padding = new Thickness(5, 5, 0, 0),
                Tag = text
            };

            Grid.SetColumn(textBlock, 0);
            TextLogModel.log("CreateLog (2/6): Columns set");

            Button deleteButton = new Button()
            {
                Content = "Del",
                Background = Brushes.Black,
                Foreground = new BrushConverter().ConvertFrom("#045de1") as SolidColorBrush,
                BorderBrush = Brushes.Black,
                FontWeight = FontWeights.Bold,
                FontSize = 15,
                Style = (Style)FindResource("HoverButtonStyle")
            };
            deleteButton.Click += (s, e) => Delete_Click(s, e);

            Grid.SetColumn(deleteButton, 1);
            TextLogModel.log("CreateLog (3/6): deleteButton created");

            textGrid.Children.Add(textBlock);
            TextLogModel.log("CreateLog (4/6): textBlock added to textGrid");

            textGrid.Children.Add(deleteButton);
            TextLogModel.log("CreateLog (5/6): deleteButton added to textGrid");

            textPanel.Children.Add(textGrid);
            TextLogModel.log("CreateLog (finished): textGrid added to textPanel");

        }

        private void LoadData(List<string> logData)
        {
            if (logData != null)
            {
                foreach (string log in logData)
                {
                    CreateLog(log);
                }
                return;
            }
            TextLogModel.log("No saved data.");
            return;
        }

        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string userInput = GetUserInput();
                CreateLog(userInput);
                return;
            }
            return;
        }
    }
}