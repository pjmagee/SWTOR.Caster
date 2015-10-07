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
        public LogWindow()
        {
            InitializeComponent();
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            DebugTextBlock.Text = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"));
        }
    }
}
