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
            var window = new SettingsWindow();
            window.ShowDialog();
        }

        private void AbilityLoggerButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AbilityWindow();
            window.ShowDialog();
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new LogWindow();
            window.ShowDialog();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
