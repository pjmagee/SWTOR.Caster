using System.Windows;

namespace SwtorCaster.ViewModels
{
    using System;
    using Caliburn.Micro;
    using Core.Domain.Settings;
    using Core.Services.Combat;
    using Core.Services.Providers;
    using System.Deployment.Application;

    public class MainViewModel : Screen, IHandle<Settings>, IHandle<ICombatLogService>
    {
        private readonly ICombatLogProvider combatLogProvider;
        private readonly IWindowManager windowManager;

        private readonly SettingsViewModel settingsViewModel;
        private readonly AboutViewModel aboutViewModel;

        private readonly AbilityOverlayViewModel abilityOverlayViewModel;
        private readonly WindowedViewModel windowedViewModel;

        public MainViewModel(
            IWindowManager windowManager,
            IEventAggregator eventAggregator,
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

            eventAggregator.Subscribe(this);
            Initialized();
        }

        private void Initialized()
        {
            StartParserService();
            OpenDefaultWindows();
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

        public string Version => GetVersion();

        private string GetVersion()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var deployment = ApplicationDeployment.CurrentDeployment;
                return deployment.CurrentVersion.ToString();
            }

            return "Development";
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                Application.Current.Shutdown();
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