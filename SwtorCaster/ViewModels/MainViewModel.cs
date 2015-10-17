namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;

    public class MainViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly SettingsViewModel _settingsViewModel;

        private readonly AbilityViewModel _abilityViewModel;
        private readonly LogViewModel _logViewModel;
        private readonly AboutViewModel _aboutViewModel;
        
        public MainViewModel(
            IWindowManager windowManager,
            SettingsViewModel settingsViewModel, 
            AbilityViewModel abilityViewModel, 
            LogViewModel logViewModel, 
            AboutViewModel aboutViewModel)
        {
            _windowManager = windowManager;
            _settingsViewModel = settingsViewModel;
            _abilityViewModel = abilityViewModel;
            _logViewModel = logViewModel;
            _aboutViewModel = aboutViewModel;
        }

        public override string DisplayName { get; set; } = "SWTOR Caster";

        public void OpenSettingsView()
        {
            _windowManager.ShowWindow(_settingsViewModel);
        }

        public void OpenAbilityView()
        {
            _windowManager.ShowWindow(_abilityViewModel);
        }

        public void OpenLogView()
        {
            _windowManager.ShowWindow(_logViewModel);
        }

        public void OpenAboutView()
        {
            _windowManager.ShowWindow(_aboutViewModel);
        }
    }
}