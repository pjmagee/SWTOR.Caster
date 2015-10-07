namespace SwtorCaster
{
    using System;
    using System.IO;
    using System.Windows;

    /// <summary>
    /// Interaction logic for Log.xaml
    /// </summary>
    public partial class Log : Window
    {
        public Log()
        {
            InitializeComponent();
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            DebugTextBlock.Text = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"));
        }
    }
}
