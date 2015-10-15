namespace SwtorCaster
{
    using System.Windows;
    using System.Diagnostics;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : MetroWindow
    {
        public About()
        {
            InitializeComponent();
        }

        private void Guild_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.awakenedgamers.com/");
            this.Close();
        }

        private void TorCommunity_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://torcommunity.com/");
            this.Close();
        }
    }
}
