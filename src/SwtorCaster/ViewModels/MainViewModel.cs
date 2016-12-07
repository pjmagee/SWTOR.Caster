namespace SwtorCaster.ViewModels
{
    using System.Windows;
    using Caliburn.Micro;
    using Core.Domain.Settings;
    using Core.Services.Combat;
    using Core.Services.Providers;
    using Core.Services.Settings;

    public class MainViewModel : FocusableScreen, IHandle<Settings>, IHandle<ICombatLogService>
    {
        private readonly ISettingsService settingsService;
        private readonly ICombatLogProvider combatLogProvider;
        private readonly IWindowManager windowManager;

        private readonly SettingsViewModel settingsViewModel;
        private readonly AboutViewModel aboutViewModel;

        private readonly AbilityOverlayViewModel abilityOverlayViewModel;
        private readonly WindowedViewModel windowedViewModel;

        public MainViewModel(
            IWindowManager windowManager,
            IEventAggregator eventAggregator,
            ISettingsService settingsService,
            WindowedViewModel windowedViewModel,
            SettingsViewModel settingsViewModel,
            AbilityOverlayViewModel abilityOverlayViewModel,
            AboutViewModel aboutViewModel,
            ICombatLogProvider combatLogProvider)
        {
            this.windowManager = windowManager;
            this.settingsViewModel = settingsViewModel;
            this.abilityOverlayViewModel = abilityOverlayViewModel;
            this.aboutViewModel = aboutViewModel;
            this.combatLogProvider = combatLogProvider;
            this.windowedViewModel = windowedViewModel;
            this.settingsService = settingsService;

            eventAggregator.Subscribe(this);
        }

        protected override void OnInitialize()
        {
            SetWindowLocation();

            StartParserService();            
            
            OpenDefaultWindows();
        }

        private void SetWindowLocation()
        {
            if (settingsService.Settings.MainWindowLocation == default(Point)) return;
            Window.Left = settingsService.Settings.MainWindowLocation.X;
            Window.Top = settingsService.Settings.MainWindowLocation.Y;
        }

        private void OpenDefaultWindows()
        {
            if (settingsViewModel.MainSettingsViewModel.OpenLoggerWindowOnStartup)
            {
                OpenAbilityWindowedView();
            }
        }

        private void StartParserService()
        {
            var parser = combatLogProvider.GetCombatLogService();
            parser.Start();
        }

        public override string DisplayName { get; set; } = "SWTOR Caster";

        public void OpenAbilityWindowedView()
        {
            OpenOrReactivate(windowedViewModel);
        }

        public void OpenAbilityOverlayView()
        {
            OpenOrReactivate(abilityOverlayViewModel);
        }
        
        public void OpenAboutView()
        {
            OpenOrReactivate(aboutViewModel);
        }

        public void OpenSettingsView()
        {
            OpenOrReactivate(settingsViewModel);
        }

        private void OpenOrReactivate(FocusableScreen focusableScreen)
        {
            if (!focusableScreen.IsActive)
            {
                windowManager.ShowWindow(focusableScreen);
            }
            else
            {
                focusableScreen.Focus();
            }
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            SaveWindowLocation();

            if (close)
            {                
                Application.Current.Shutdown();
            }            
        }

        private void SaveWindowLocation()
        {
            settingsService.Settings.MainWindowLocation = new Point(Window.Left, Window.Top);
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