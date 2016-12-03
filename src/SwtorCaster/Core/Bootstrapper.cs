namespace SwtorCaster.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media.Animation;
    using Caliburn.Micro;
    using Services.Audio;
    using Services.Combat;
    using Services.Images;
    using Services.Images.JediPedia;
    using Services.Logging;
    using Services.Parsing;
    using Services.Settings;
    using Services.Events;
    using Services.Factory;
    using Services.Providers;
    using ViewModels;
    using System.Threading;

    public sealed class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer container;

        public Bootstrapper()
        {
            //if (Execute.InDesignMode)
            //{
            //    StartDesignTime();
            //}
            //else 
            //{
            //    StartRuntime();
            //}

            base.Initialize();
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = container.GetInstance(service, key);
            if (instance != null) return instance;

            throw new Exception("Could not locate any instances.");
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void Configure()
        {
            container = new SimpleContainer();
            BindServices();
            BindViewModels();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof (Timeline), new FrameworkPropertyMetadata { DefaultValue = 30 });            
            await InitializeImages();
            DisplayRootViewFor<MainViewModel>();
        }

        private async Task InitializeImages()
        {
            var splashViewModel = container.GetInstance<SplashViewModel>();
            var imageService = container.GetInstance<IImageService>();
            var fontService = container.GetInstance<IFontService>();

            splashViewModel.Start();

            await Task.Run(() => imageService.Initialize());
            await Task.Run(() => fontService.Initialize());
            await Task.Run(() => Thread.Sleep(2));

            splashViewModel.TryClose();
        }

        private void BindViewModels()
        {
            // Start up
            container.Singleton<SplashViewModel>();
            container.Singleton<MainViewModel>();

            // Ability logger
            container.PerRequest<AbilityViewModel>();
            container.Singleton<AbilityOverlayViewModel>();
            container.Singleton<WindowedViewModel>();

            // Settings
            container.Singleton<MainSettingsViewModel>();
            container.Singleton<AbilitySettingsViewModel>();
            container.Singleton<EventSettingsViewModel>();
            container.Singleton<PlayBackSettingsViewModel>();
            container.Singleton<SettingsViewModel>();
            
            // About
            container.Singleton<AboutViewModel>();
        }

        private void BindServices()
        {
            // Combat services
            container.Singleton<ICombatLogParser, CombatLogParser>();
            container.Singleton<ICombatLogViewModelFactory, CombatLogViewModelFactory>();
            container.Singleton<ICombatLogProvider, CombatLogProvider>();
            container.Singleton<ICombatLogService, RealTimeLogService>("RealTime");
            container.Singleton<ICombatLogService, PlayBackLogService>("PlayBack");

            // Caliburn services
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<IWindowManager, WindowManager>();
            
            // Settings services
            container.Singleton<ISettingsService, SettingsService>();
            container.Singleton<IEventService, EventService>();
            container.Singleton<IAudioService, AudioService>();

            // Image services
            container.Singleton<IImageService, JediPediaImageService>();

            // Helper services
            container.Singleton<ILoggerService, LoggerService>();
            container.Singleton<IFontService, FontService>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            foreach(var combatLogService in container.GetAllInstances<ICombatLogService>())
            {
                combatLogService?.Stop();
            }            
        }
    }
}