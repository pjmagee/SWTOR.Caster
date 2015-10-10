namespace SwtorCaster
{
    using System;
    using System.Windows;
    using ViewModel;

    public partial class AbilityWindow
    {
        public AbilityWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as AbilitiesViewModel;
            viewModel?.Start();
        }

        private void Window_OnClosed(object sender, EventArgs e)
        {
            var viewModel = DataContext as AbilitiesViewModel;
            viewModel?.Stop();
        }
    }
}
