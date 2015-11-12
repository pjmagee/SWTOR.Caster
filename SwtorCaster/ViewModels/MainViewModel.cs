namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;
    using Screens;
    using Core.Domain.Settings;
    using Core.Services.Combat;
    using Core.Services.Providers;

    public class MainViewModel : Screen, IHandle<Settings>, IHandle<ICombatLogService>
    {
        private readonly IWindowManager _windowManager;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly AbilityViewModel _abilityViewModel;
        private readonly OnTopViewModel _onTopViewModel;
        private readonly LogViewModel _logViewModel;
        private readonly AboutViewModel _aboutViewModel;
        private readonly ICombatLogProvider _combatLogProvider;

        public MainViewModel(
            IWindowManager windowManager,
            SettingsViewModel settingsViewModel, 
            AbilityViewModel abilityViewModel, 
            OnTopViewModel onTopViewModel,
            LogViewModel logViewModel, 
            AboutViewModel aboutViewModel, 
            IEventAggregator eventAggregator, 
            ICombatLogProvider combatLogProvider)
        {
            _windowManager = windowManager;
            _settingsViewModel = settingsViewModel;
            _abilityViewModel = abilityViewModel;
            _onTopViewModel = onTopViewModel;
            _logViewModel = logViewModel;
            _aboutViewModel = aboutViewModel;
            _combatLogProvider = combatLogProvider;

            eventAggregator.Subscribe(this);
            Initialized();
        }

        private void Initialized()
        {
            var parser = _combatLogProvider.GetCombatLogService();
            parser.Start();
        }

        public override string DisplayName { get; set; } = "SWTOR Caster";

        public void OpenSettingsView()
        {
            OpenOrReactivate(_settingsViewModel);
        }

        public void OpenAbilityTransparentView()
        {
            OpenOrReactivate(_onTopViewModel);
        }

        public void OpenAbilityView()
        {
            OpenOrReactivate(_abilityViewModel);
        }

        public void OpenLogView()
        {
            OpenOrReactivate(_logViewModel);
        }

        public void OpenAboutView()
        {
            OpenOrReactivate(_aboutViewModel);
        }

        private void OpenOrReactivate(FocusableScreen focusableScreen)
        {
            if (!focusableScreen.IsActive)
            {
                _windowManager.ShowWindow(focusableScreen);
            }
            else
            {
                focusableScreen.Focus();
            }
        }

        public void Handle(Settings message)
        {
            Refresh();
        }

        public void Handle(ICombatLogService combatLogService)
        {
            combatLogService.Start();
        }
    }
}