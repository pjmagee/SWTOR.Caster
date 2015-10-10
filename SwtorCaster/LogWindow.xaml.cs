namespace SwtorCaster
{
    using System;
    using System.IO;
    using System.Windows;

    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
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
            
            ScrollViewer.ScrollToBottom();
        }
    }
}
