namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;
    using Screens;
    using Core.Domain.Settings;
    using Core.Services.Combat;
    using Core.Services.Providers;

    public class MainViewModel : Screen, IHandle<Settings>, IHandle<ICombatLogService>
    {
        private readonly ICombatLogProvider _combatLogProvider;
        private readonly IWindowManager _windowManager;

        private readonly SettingsViewModel _settingsViewModel;
        private readonly AboutViewModel _aboutViewModel;

        // Ability Logger Views
        private readonly OverlayViewModel _overlayViewModel;
        private readonly ObsViewModel _obsViewModel;

        public MainViewModel(
            IWindowManager windowManager,
            IEventAggregator eventAggregator, 
            ObsViewModel obsViewModel,
            SettingsViewModel settingsViewModel, 
            OverlayViewModel overlayViewModel,
            AboutViewModel aboutViewModel, 
            ICombatLogProvider combatLogProvider)
        {
            _windowManager = windowManager;
            _settingsViewModel = settingsViewModel;
            _overlayViewModel = overlayViewModel;
            _aboutViewModel = aboutViewModel;
            _combatLogProvider = combatLogProvider;
            _obsViewModel = obsViewModel;

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

        public void OpenOverlayView()
        {
            OpenOrReactivate(_overlayViewModel);
        }

        public void OpenObsView()
        {
            OpenOrReactivate(_obsViewModel);
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