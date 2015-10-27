namespace SwtorCaster.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media.Animation;
    using Caliburn.Micro;
    using Factories;
    using Services.Audio;
    using Services.Images;
    using Services.Logging;
    using Services.Parsing;
    using Services.Settings;
    using Services;
    using Services.Events;
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
                new FrameworkPropertyMetadata { DefaultValue = 20 });
            
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
            _container.Singleton<SplashViewModel>();
            _container.Singleton<MainViewModel>();
            _container.Singleton<AbilityViewModel>();
            _container.Singleton<LogViewModel>();
            _container.Singleton<AboutViewModel>();
            _container.Singleton<SettingsViewModel>();
        }

        private void BindServices()
        {
            _container.Singleton<IParserProvider, ParserProvider>();
            _container.Singleton<IParserService, ParserService>("CombatLogParser");
            _container.Singleton<IParserService, DemoParserService>("DemoParser");

            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IAudioService, AudioService>();
            _container.Singleton<IImageService, ImageService>();
            _container.Singleton<ISettingsService, SettingsService>();
            _container.Singleton<ILoggerService, LoggerService>();
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<ILogLineFactory, LogLineFactory>();
            _container.Singleton<IEventService, EventService>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            var parser = _container.GetInstance<IParserService>();
            parser?.Stop();
        }
    }
}