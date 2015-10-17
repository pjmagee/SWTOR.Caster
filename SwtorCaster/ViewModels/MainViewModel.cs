namespace SwtorCaster.ViewModels
{
    using System.Windows;
    using Caliburn.Micro;
    using Core.Services;

    public class MainViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IParserService _parserService;
        private readonly SettingsViewModel _settingsViewModel;

        private readonly AbilityViewModel _abilityViewModel;
        private readonly LogViewModel _logViewModel;
        private readonly AboutViewModel _aboutViewModel;

        public bool IsParsing => _parserService.IsRunning;

        public MainViewModel(IWindowManager windowManager, IParserService parserService, SettingsViewModel settingsViewModel, AbilityViewModel abilityViewModel, LogViewModel logViewModel, AboutViewModel aboutViewModel)
        {
            _windowManager = windowManager;
            _parserService = parserService;
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

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _parserService.Stop();
            Application.Current.Shutdown();
        }
    }
}