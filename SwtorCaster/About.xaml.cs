namespace SwtorCaster
{
    using System.Windows;
    using System.Diagnostics;

    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void Guild_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.awakenedgamers.com/");
        }

        private void TorCommunity_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://torcommunity.com/");
        }
    }
}
