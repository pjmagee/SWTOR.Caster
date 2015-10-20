namespace SwtorCaster.Core.Services
{
    using System;
    using System.ComponentModel;
    using System.Deployment.Application;
    using System.IO;
    using System.Windows;
    using Domain;
    using Newtonsoft.Json;
    using static System.Environment;

    public class SettingsService : ISettingsService
    {
        private static readonly string SwtorCaster = Path.Combine(GetFolderPath(SpecialFolder.LocalApplicationData), "SwtorCaster");
        private static readonly string SettingsPath = Path.Combine(SwtorCaster, "settings.json");
        private readonly ILoggerService _loggerService;

        public Settings Settings { get; set; }

        public SettingsService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
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
                var json = JsonConvert.SerializeObject(Settings, Formatting.Indented);

                if (!Directory.Exists(SwtorCaster))
                {
                    Directory.CreateDirectory(SwtorCaster);
                }

                File.WriteAllText(SettingsPath, json);
            }
            catch (Exception e)
            {
                _loggerService.Log(e.Message);
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
                    Settings = JsonConvert.DeserializeObject<Settings>(json);

                    if (ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString() != Settings.Version)
                    {
                        Settings = new Settings();
                    }
                    else
                    {
                        Settings = new Settings();
                    }
                }
                catch (Exception e)
                {
                    Settings = new Settings();
                    _loggerService.Log(e.Message);
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Settings = new Settings();
            }

            // Hook into the property changed event, so that we instantly save changes.
            Settings.PropertyChanged -= SettingsOnPropertyChanged;
            Settings.PropertyChanged += SettingsOnPropertyChanged;
        }
    }
}