namespace SwtorCaster.Core
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using Caliburn.Micro;
    using Services;
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

            Initialize();
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
        
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var imageService = _container.GetInstance<IImageService>();

            var splash = new SplashScreen("Resources/splash.jpg");
            splash.Show(false);
            imageService.Initialize();
            splash.Close(TimeSpan.FromSeconds(2));

            DisplayRootViewFor<MainViewModel>();
        }

        private void BindViewModels()
        {
            _container.Singleton<MainViewModel>();
            _container.Singleton<AbilityViewModel>();
            _container.Singleton<LogViewModel>();
            _container.Singleton<AboutViewModel>();
            _container.Singleton<SettingsViewModel>();
        }

        private void BindServices()
        {
            _container.Singleton<IImageService, ImageService>();
            _container.Singleton<ISettingsService, SettingsService>();
            _container.Singleton<ILoggerService, LoggerService>();
            _container.Singleton<IParserService, ParserService>();
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<ILogLineEventArgFactory, LogLineEventArgFactory>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            var parser = _container.GetInstance<IParserService>();
            parser?.Stop();
        }
    }
}