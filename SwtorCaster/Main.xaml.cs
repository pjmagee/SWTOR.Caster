namespace SwtorCaster
{
    using Windows;
    using System.Windows;

    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settings = new Settings();
            settings.Show();
        }

        private void AbilityLoggerButton_Click(object sender, RoutedEventArgs e)
        {
            var logger = new AbilityLogger();
            logger.Show();
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            var log = new Log();
            log.Show();
        }
    }
}
