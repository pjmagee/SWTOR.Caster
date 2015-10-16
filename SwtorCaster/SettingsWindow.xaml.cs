namespace SwtorCaster
{
    using System;
    using System.Linq;
    using System.Windows;
    using MahApps.Metro.Controls;
    using Parser;

    public partial class SettingsWindow : MetroWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Settings settings = new Settings
                {
                    MaxAbilityList = (int)MaxItems.Value,
                    MinimumAngle = (int)MiniumAngle.Value,
                    MaximumAngle = (int)MaximumAngle.Value,
                    EnableCombatClear = ExitCombatClearLog.IsChecked.GetValueOrDefault(),
                    EnableAbilityText = EnableAbilityName.IsChecked.GetValueOrDefault(),
                    EnableAliases = EnableAliases.IsChecked.GetValueOrDefault(),
                    EnableLogging = LogToFile.IsChecked.GetValueOrDefault(),
                    Abilities = Aliases.ItemsSource.Cast<Ability>(),
                    EnableClearInactivity = EnableInactivityClearLog.IsChecked.GetValueOrDefault(),
                    ClearAfterInactivity = (int)InactivitySecondsToClear.Value
                };

                settings.Save();
                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_OnLoaded(object sender, RoutedEventArgs e)
        {
            var settings = Settings.LoadSettings();

            MaxItems.Value = settings.MaxAbilityList;
            MiniumAngle.Value = settings.MinimumAngle;
            MaximumAngle.Value = settings.MaximumAngle;

            ExitCombatClearLog.IsChecked = settings.EnableCombatClear;
            EnableAliases.IsChecked = settings.EnableAliases;
            EnableAbilityName.IsChecked = settings.EnableAbilityText;
            LogToFile.IsChecked = settings.EnableLogging;
            Aliases.ItemsSource = settings.Abilities;

            EnableInactivityClearLog.IsChecked = settings.EnableClearInactivity;
            InactivitySecondsToClear.Value = settings.ClearAfterInactivity;
        }
    }
}
