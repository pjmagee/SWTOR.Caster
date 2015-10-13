namespace SwtorCaster
{
    using System;
    using System.IO;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : MetroWindow
    {
        private readonly string _logPath = Path.Combine(Environment.CurrentDirectory, "log.txt");

        public LogWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            if (File.Exists(_logPath))
            {
                DebugTextBlock.Text = File.ReadAllText(_logPath);
            }

            if (DebugTextBlock.Text == string.Empty)
            {
                DebugTextBlock.Text = "No errors found in debug log file";
            }

            ScrollViewer.ScrollToBottom();
        }
    }
}
