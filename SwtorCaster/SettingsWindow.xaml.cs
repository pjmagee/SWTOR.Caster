namespace SwtorCaster
{
    using System.Linq;
    using System.Windows;
    using Parser;

    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();

            settings.MaxAbilityList = int.Parse(MaxItems.Text);
            settings.MinimumAngle = int.Parse(MinAngle.Text);
            settings.MaximumAngle = int.Parse(MaxAngle.Text);

            settings.EnableCombatClear = ExitCombatClearLog.IsChecked.GetValueOrDefault();
            settings.EnableAbilityText = EnableAbilityName.IsChecked.GetValueOrDefault();
            settings.EnableAliases = EnableAliases.IsChecked.GetValueOrDefault();
            settings.EnableLogging = LogToFile.IsChecked.GetValueOrDefault();

            settings.Abilities = Aliases.ItemsSource.Cast<Ability>();

            settings.Save();
        }

        private void Window_OnLoaded(object sender, RoutedEventArgs e)
        {
            var settings = Settings.LoadSettings();

            MaxItems.Text = settings.MaxAbilityList.ToString();
            MinAngle.Text = settings.MinimumAngle.ToString();
            MaxAngle.Text = settings.MaximumAngle.ToString();

            ExitCombatClearLog.IsChecked = settings.EnableCombatClear;
            EnableAliases.IsChecked = settings.EnableAliases;
            EnableAbilityName.IsChecked = settings.EnableAbilityText;
            LogToFile.IsChecked = settings.EnableLogging;

            Aliases.ItemsSource = settings.Abilities;
        }
    }
}
