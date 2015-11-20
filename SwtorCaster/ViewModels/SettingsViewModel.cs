namespace SwtorCaster.ViewModels
{
    public class SettingsViewModel : FocusableScreen
    {
        public override string DisplayName { get; set; } = "SWTOR Caster - Settings";

        public EventSettingsViewModel EventSettingsViewModel { get; } 

        public MainSettingsViewModel MainSettingsViewModel { get; }

        public AbilitySettingsViewModel AbilitySettingsViewModel { get; }

        public GuideSettingsViewModel GuideSettingsViewModel { get; }

        public SettingsViewModel(
            MainSettingsViewModel mainSettingsViewModel, 
            AbilitySettingsViewModel abilitySettingsViewModel, 
            EventSettingsViewModel eventSettingsViewModel, 
            GuideSettingsViewModel guideSettingsViewModel)
        {
            EventSettingsViewModel = eventSettingsViewModel;
            GuideSettingsViewModel = guideSettingsViewModel;
            MainSettingsViewModel = mainSettingsViewModel;
            AbilitySettingsViewModel = abilitySettingsViewModel;
        }
    }
}