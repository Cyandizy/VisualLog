using WinForms = System.Windows.Forms;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using VisualLog.Models;
using VisualLog.ViewModels;
using System.Linq;
using System;
using System.Collections.Generic;

namespace VisualLog.Views
{
    public partial class VisualLogView : Page
    {

        private VisualLogViewModel viewModel;


        public VisualLogView()
        {
            viewModel = new VisualLogViewModel();
            InitializeComponent();

            //create a load button if previous save exists
            if (viewModel.HasData())
            {
                Button loadButton = CreateLoadButton();
                LogPanel.Children.Add(loadButton);
            }
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(InputTextBox.Text))
            {
                string userInput = GetUserInput();
                CreateLog(userInput);
            }
        }


        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.HasData())
            {
                DebugService.Log("LogPanel cleared");
                LogPanel.Children.Clear();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Button loadButton = CreateLoadButton();
                LogPanel.Children.Add(loadButton);
            }
        }


        private void Load_Click(object sender, RoutedEventArgs e)
        {
            LogPanel.Children.Clear();
            LoadExistingLogs();
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                var textGrid = btn.Parent as Grid;
                btn.Click -= Delete_Click;

                if (textGrid != null)
                {
                    var textBlock = textGrid.Children.OfType<TextBlock>().FirstOrDefault();
                    var textPanel = textGrid.Parent as StackPanel;

                    if (textBlock != null && textPanel != null && textBlock.Tag != null)
                    {
                        string tagValue = textBlock.Tag.ToString()!;
                        viewModel.DeleteData(tagValue);

                        textPanel.Children.Remove(textGrid);
                    }
                }
            }
        }


        private string GetUserInput()
        {
            //attaches date and time to user's input and empties the text box
            DateTime localTime = DateTime.Now;
            var culture = new CultureInfo("en-US");
            string userInput = $"{localTime.ToString("yyyy-MM-dd hh:mm:ss tt", culture)}: {InputTextBox.Text}";
            viewModel.AppendData(userInput);

            InputTextBox.Text = string.Empty;
            DebugService.Log("textBox emptied");

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
            DebugService.Log("CreateLog (1/6): Columns defined");

            //date and time portion of the text is exactly 23
            string time = text.Substring(0, 23);
            string content = text.Substring(23);

            //colors the date and time portion
            Run timePortion = new Run(time)
            {
                Foreground = new BrushConverter().ConvertFrom("#045de1") as SolidColorBrush,
                FontWeight = FontWeights.Bold
            };
            //colors the content portion
            Run contentPortion = new Run(content)
            {
                Foreground = new BrushConverter().ConvertFrom("#cccccc") as SolidColorBrush
            };
            //combines both portions
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
            DebugService.Log("CreateLog (2/6): Columns set");

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
            DebugService.Log("CreateLog (3/6): deleteButton created");

            textGrid.Children.Add(textBlock);
            DebugService.Log("CreateLog (4/6): textBlock added to textGrid");

            textGrid.Children.Add(deleteButton);
            DebugService.Log("CreateLog (5/6): deleteButton added to textGrid");

            LogPanel.Children.Add(textGrid);
            DebugService.Log("CreateLog (finished): textGrid added to textPanel");

        }


        private Button CreateLoadButton()
        {
            Button loadButton = new Button()
            {
                Content = "Load Existing Logs",
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Style = (Style)FindResource("HoverButtonStyle")
            };
            loadButton.Click += (s, e) => Load_Click(s, e);

            return loadButton;
        }


        private void LoadExistingLogs()
        {
            DebugService.Log("Loading existing logs");
            List<string> dataList = viewModel.GetDataList();
            if (dataList != null)
            {
                foreach (string log in dataList)
                {
                    CreateLog(log);
                }
                return;
            }
            DebugService.Log("No saved data.");
            return;
        }


        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            //allows users to use return key in a way equivalent to Add button
            if (e.Key == Key.Return)
            {
                string userInput = GetUserInput();
                CreateLog(userInput);
                return;
            }
            return;
        }

        private void SavePath_Click(object sender, EventArgs e)
        {
            viewModel.ChooseSavePath();
            if (viewModel.HasData())
            {
                LogPanel.Children.Clear();
                Button loadButton = CreateLoadButton();
                LogPanel.Children.Add(loadButton);
            }
        }
    }

}
