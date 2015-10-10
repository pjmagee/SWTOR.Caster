namespace SwtorCaster
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settings = new SettingsWindow();
            settings.Show();
        }

        private void AbilityLoggerButton_Click(object sender, RoutedEventArgs e)
        {
            var logger = new AbilityWindow();
            logger.Show();
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            var log = new LogWindow();
            log.Show();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
