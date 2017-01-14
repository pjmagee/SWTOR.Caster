namespace SwtorCaster.Core.Services.Settings
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using Caliburn.Micro;
    using Domain.Settings;
    using Logging;
    using Newtonsoft.Json;

    public class SettingsService : ISettingsService
    {
        private static readonly string SwtorCaster = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SwtorCaster");
        private static readonly string SettingsPath = Path.Combine(SwtorCaster, "settings.json");
        private readonly ILoggerService loggerService;
        private readonly IEventAggregator eventAggregator;

        public AppSettings Settings { get; set; }

        public SettingsService(ILoggerService loggerService, IEventAggregator eventAggregator)
        {
            this.loggerService = loggerService;
            this.eventAggregator = eventAggregator;
            Load();
        }

        private void SettingsOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Save();
        }

        public void Save()
        {
            try
            {
                var settings = Settings;

                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);

                if (!Directory.Exists(SwtorCaster))
                {
                    Directory.CreateDirectory(SwtorCaster);
                }

                File.WriteAllText(SettingsPath, json);

                eventAggregator.PublishOnUIThread(settings);
            }
            catch (Exception e)
            {
                loggerService.Log(e.Message);
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Load()
        {
            if (File.Exists(SettingsPath))
            {
                try
                {
                    var json = File.ReadAllText(SettingsPath);
                    Settings = JsonConvert.DeserializeObject<AppSettings>(json);
                }
                catch (Exception e)
                {
                    Settings = new AppSettings();
                    loggerService.Log(e.Message);
                    MessageBox.Show("Error reading saved settings, loading default settings.", "Settings", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Settings = new AppSettings();
            }

            WireEvents();
        }

        private void WireEvents()
        {
            Settings.PropertyChanged -= SettingsOnPropertyChanged;
            Settings.PropertyChanged += SettingsOnPropertyChanged;
        }
    }
}