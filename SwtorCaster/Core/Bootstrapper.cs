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
    using Services.Logging;
    using Services.Parsing;
    using Services.Settings;
    using Services.Events;
    using Services.Factory;
    using Services.Providers;
    using Services.Guide;
    using ViewModels;

    public sealed class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;

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
            return _container.GetAllInstances(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = _container.GetInstance(service, key);
            if (instance != null) return instance;

            throw new Exception("Could not locate any instances.");
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();
            BindServices();
            BindViewModels();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
                typeof (Timeline),
                new FrameworkPropertyMetadata { DefaultValue = 30 });
            
            await InitializeImages();
            DisplayRootViewFor<MainViewModel>();
        }

        private async Task InitializeImages()
        {
            var splashViewModel = _container.GetInstance<SplashViewModel>();
            var imageService = _container.GetInstance<IImageService>();

            splashViewModel.Start();
            await Task.Run(() => imageService.Initialize());
            splashViewModel.TryClose();
        }

        private void BindViewModels()
        {
            // Start up
            _container.Singleton<SplashViewModel>();
            _container.Singleton<MainViewModel>();

            // Ability logger
            _container.PerRequest<AbilityViewModel>();
            _container.Singleton<AbilityOverlayViewModel>();
            _container.Singleton<WindowedViewModel>();

            // Settings
            _container.Singleton<MainSettingsViewModel>();
            _container.Singleton<AbilitySettingsViewModel>();
            _container.Singleton<EventSettingsViewModel>();
            _container.Singleton<SettingsViewModel>();

            // Guide 
            _container.Singleton<RotationViewModel>();
            _container.Singleton<GuideOverlayViewModel>();

            // Guide settings
            _container.Singleton<GuideSettingsViewModel>();
            _container.Singleton<CreateGuideViewModel>();
            _container.Singleton<LoadGuideViewModel>();
            
            // About
            _container.Singleton<AboutViewModel>();
        }

        private void BindServices()
        {
            // Combat services
            _container.Singleton<ICombatLogProvider, CombatLogProvider>();
            _container.Singleton<ICombatLogService, CombatLogService>("CombatLogParser");
            _container.Singleton<ICombatLogService, DemoCombatLogService>("DemoParser");
            _container.Singleton<ICombatLogParser, CombatLogParser>();
            _container.Singleton<ICombatLogViewModelFactory, CombatLogViewModelFactory>();

            // Caliburn services
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IWindowManager, WindowManager>();
            
            // Settings services
            _container.Singleton<ISettingsService, SettingsService>();
            _container.Singleton<IEventService, EventService>();
            _container.Singleton<IAudioService, AudioService>();
            _container.Singleton<IRotationService, RotationService>();

            // Image services
            // _container.Singleton<IImageService, ImageService>();
            _container.Singleton<IImageService, MappedImageService>();

            // Helper services
            _container.Singleton<ILoggerService, LoggerService>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            var parser = _container.GetInstance<ICombatLogService>();
            parser?.Stop();                        
        }
    }
}