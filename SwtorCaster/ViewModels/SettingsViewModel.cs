namespace SwtorCaster.ViewModels
{
    using Screens;

    /// <summary>
    /// We hook into the Settings Property Changed event and any time a value changes, we serialize the settings instantly.
    /// So the user does not have to press Save changes. Changes are instant.
    /// </summary>
    public class SettingsViewModel : FocusableScreen
    {
        public override string DisplayName { get; set; } = "SWTOR Caster - Settings";

        public EventSettingsViewModel EventSettingsViewModel { get; } 

        public MainSettingsViewModel MainSettingsViewModel { get; }

        public AbilitySettingsViewModel AbilitySettingsViewModel { get; }
        
        public SettingsViewModel(
            MainSettingsViewModel mainSettingsViewModel, 
            AbilitySettingsViewModel abilitySettingsViewModel, 
            EventSettingsViewModel eventSettingsViewModel)
        {
            EventSettingsViewModel = eventSettingsViewModel;
            MainSettingsViewModel = mainSettingsViewModel;
            AbilitySettingsViewModel = abilitySettingsViewModel;
        }
    }
}