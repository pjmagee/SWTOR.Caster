namespace SwtorCaster
{
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
            var vm = DataContext as AbilitiesViewModel;
            vm?.Start();
        }
    }
}
