namespace SwtorCaster.Parser
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using Annotations;
    using Newtonsoft.Json;

    public class Settings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        private static Settings _settings;

        [JsonIgnore]
        public static Settings Current
        {
            get
            {
                return _settings ?? (_settings = LoadSettings());
            }
            set
            {
                _settings = value;
                _settings.OnPropertyChanged();
            }
        }

        [JsonIgnore]
        private static readonly string SettingsPath = Path.Combine(Environment.CurrentDirectory, "settings.json");

        [JsonProperty("maxAbilityList")]
        public int MaxAbilityList { get; set; } = 5;

        [JsonProperty("enableAbilityText")]
        public bool EnableAbilityText { get; set; } = false;

        [JsonProperty("enableAliases")]
        public bool EnableAliases { get; set; } = false;

        [JsonProperty("enableLogging")]
        public bool EnableLogging { get; set; } = true;

        [JsonProperty("minimumAngle")]
        public int MinimumAngle { get; set; } = -5;

        [JsonProperty("maximumAngle")]
        public int MaximumAngle { get; set; } = 5;

        [JsonProperty("enableClearInactivity")]
        public bool EnableClearInactivity { get; set; } = true;

        [JsonProperty("secondsOfInactivity")]
        public int ClearAfterInactivity { get; set; } = 10;

        [JsonProperty("enableCombatClear")]
        public bool EnableCombatClear { get; set; } = true;

        [JsonProperty("abilities")]
        public IEnumerable<Ability> Abilities { get; set; } = new List<Ability>();
        
        public static Settings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var json = File.ReadAllText(SettingsPath);
                    return JsonConvert.DeserializeObject<Settings>(json);
                }

                return new Settings();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error loading settings: {e.Message}");
            }

            return new Settings();
        }

        public void Save()
        {
            try
            {
                Current = this;
                var json = JsonConvert.SerializeObject(Current);
                File.WriteAllText(SettingsPath, json);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error saving settings: {e.Message}");
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}