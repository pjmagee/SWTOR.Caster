namespace SwtorCaster
{
    using System.Windows;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
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

        private void AboutButton_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new About();
            window.ShowDialog();
        }
    }
}
