namespace SwtorCaster.Views
{
    using System.Diagnostics;
    using System.Windows;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : MetroWindow
    {
        public AboutView()
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
