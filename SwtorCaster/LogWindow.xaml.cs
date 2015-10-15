namespace SwtorCaster
{
    using System;
    using System.IO;
    using System.Windows;
    using MahApps.Metro.Controls;
    using Parser;

    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : MetroWindow
    {
        public LogWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            if (File.Exists(Settings.LogPath))
            {
                ReadFile();
            }

            if (DebugTextBlock.Text == string.Empty)
            {
                DebugTextBlock.Text = $"[{DateTime.Now}] No errors found in debug log file";
            }

            ScrollViewer.ScrollToBottom();
        }

        private void ClearLog_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Settings.LogPath))
            {
                File.WriteAllText(Settings.LogPath, $"[{DateTime.Now}] Log file cleared.");
            }

            ReadFile();
        }

        private void ReadFile()
        {
            DebugTextBlock.Text = File.ReadAllText(Settings.LogPath);
        }
    }
}
