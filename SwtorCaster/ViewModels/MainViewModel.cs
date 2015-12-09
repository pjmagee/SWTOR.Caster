namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;
    using Core.Domain.Settings;
    using Core.Services.Combat;
    using Core.Services.Providers;

    public class MainViewModel : Screen, IHandle<Settings>, IHandle<ICombatLogService>
    {
        private readonly ICombatLogProvider _combatLogProvider;
        private readonly IWindowManager _windowManager;

        private readonly SettingsViewModel _settingsViewModel;
        private readonly AboutViewModel _aboutViewModel;

        private readonly AbilityOverlayViewModel _abilityOverlayViewModel;
        private readonly WindowedViewModel _windowedViewModel;

        public MainViewModel(
            IWindowManager windowManager,
            IEventAggregator eventAggregator,
            WindowedViewModel windowedViewModel,
            SettingsViewModel settingsViewModel,
            AbilityOverlayViewModel abilityOverlayViewModel,
            AboutViewModel aboutViewModel,
            ICombatLogProvider combatLogProvider)
        {
            _windowManager = windowManager;
            _settingsViewModel = settingsViewModel;
            _abilityOverlayViewModel = abilityOverlayViewModel;
            _aboutViewModel = aboutViewModel;
            _combatLogProvider = combatLogProvider;
            _windowedViewModel = windowedViewModel;

            eventAggregator.Subscribe(this);
            Initialized();
        }

        private void Initialized()
        {
            var parser = _combatLogProvider.GetCombatLogService();
            parser.Start();
        }

        public override string DisplayName { get; set; } = "SWTOR Caster";

        public void OpenAbilityWindowedView()
        {
            OpenOrReactivate(_windowedViewModel);
        }

        public void OpenAbilityOverlayView()
        {
            OpenOrReactivate(_abilityOverlayViewModel);
        }
        
        public void OpenAboutView()
        {
            OpenOrReactivate(_aboutViewModel);
        }

        public void OpenSettingsView()
        {
            OpenOrReactivate(_settingsViewModel);
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