using System.Windows;

namespace SwtorCaster.Core.Domain.Settings
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;
    using Annotations;
    using Newtonsoft.Json;
    using System;

    public class Settings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IEnumerable<AbilitySetting> abilitySettings = new List<AbilitySetting>();
        private IEnumerable<EventSetting> eventSettings = new List<EventSetting>();

        private string abilityLoggerBackgroundColor = Colors.Transparent.ToHex();
        private string companionAbilityBorderColor = Colors.Purple.ToHex();
        private string abilityTextColor = Colors.Yellow.ToHex();
        private string abilityTextBorderColor = Colors.Black.ToHex();

        private string version;
        private string customCombatLogDirectory;
        private string combatLogFile;
        private string textFont = "SF Distant Galaxy"; // default font in Resources folder shipped with SWTORCaster
        private string mainWindowPlacementXml;
        private string windowedLoggerWindowPlacementXml;

        private int items = 5; // 5 items in the ability logger window
        private int clearAfterInactivity = 10; // 10 seconds
        private int fontSize = 32; // default text ability font size
        private int fontBorderThickness = 2;
        private int volume = 10; // the default volume of 100%
        private Guid audioDeviceId = Guid.Empty;
        private double opacity = 0.15; // the top window opacity over the game.
        
        private bool enableSound;
        private bool enableAbilitySettings;
        private bool enablePlaybackMode;

        private bool openLoggerWindowOnStartup = true;
        private bool enableAbilityText = true;
        private bool enableClearInactivity = true;
        private bool enableCombatClear = true;
        private bool enableCompanionAbilities = true;
        private bool enableLogging = true;
        private bool ignoreUnknownAbilities = true;

        private Point mainWindowLocation;
        private Point loggerWindowLocation;

        [JsonProperty("mainWindowLocation")]
        public Point MainWindowLocation
        {
            get { return mainWindowLocation; }
            set
            {
                if (value == mainWindowLocation) return;
                mainWindowLocation = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("loggerWindowLocation")]
        public Point LoggerWindowLocation
        {
            get { return loggerWindowLocation; }
            set
            {
                if (value == loggerWindowLocation) return;
                loggerWindowLocation = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("items")]
        public int Items
        {
            get { return items; }
            set
            {
                if (value == items) return;
                items = value;
                OnPropertyChanged();
            }
        }


        [JsonProperty("openLoggerWindowOnStartup")]
        public bool OpenLoggerWindowOnStartup
        {
            get { return openLoggerWindowOnStartup; }
            set
            {
                if (value == openLoggerWindowOnStartup) return;
                openLoggerWindowOnStartup = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableCompanionAbilities")]
        public bool EnableCompanionAbilities
        {
            get { return enableCompanionAbilities; }
            set
            {
                if (value == enableCompanionAbilities) return;
                enableCompanionAbilities = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("companionAbilityBorderColor")]
        public string CompanionAbilityBorderColor
        {
            get { return companionAbilityBorderColor; }
            set
            {
                companionAbilityBorderColor = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("customCombatLogDirectory")]
        public string CustomCombatLogDirectory
        {
            get { return customCombatLogDirectory; }
            set
            {
                customCombatLogDirectory = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableAbilityText")]
        public bool EnableAbilityText
        {
            get { return enableAbilityText; }
            set
            {
                if (value == enableAbilityText) return;
                enableAbilityText = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("textFont")]
        public string TextFont
        {
            get { return textFont; }
            set
            {
                if (value == textFont) return;
                textFont = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("mainWindowPlacementXml")]
        public string MainWindowPlacementXml
        {
            get { return mainWindowPlacementXml; }
            set
            {
                if (value == mainWindowPlacementXml) return;
                mainWindowPlacementXml = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("windowedLoggerWindowPlacementXml")]
        public string WindowedLoggerWindowPlacementXml
        {
            get { return windowedLoggerWindowPlacementXml; }
            set
            {
                if (value == windowedLoggerWindowPlacementXml) return;
                windowedLoggerWindowPlacementXml = value;
                OnPropertyChanged();
            }
        }        

        [JsonProperty("enableClearInactivity")]
        public bool EnableClearInactivity
        {
            get { return enableClearInactivity; }
            set
            {
                if (value == enableClearInactivity) return;
                enableClearInactivity = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("clearAfterInactivity")]
        public int ClearAfterInactivity
        {
            get { return clearAfterInactivity; }
            set
            {
                if (value == clearAfterInactivity) return;
                clearAfterInactivity = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableCombatClear")]
        public bool EnableCombatClear
        {
            get { return enableCombatClear; }
            set
            {
                if (value == enableCombatClear) return;
                enableCombatClear = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("version")]
        public string Version
        {
            get { return version; }
            set
            {
                if (value == version) return;
                version = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableLogging")]
        public bool EnableLogging
        {
            get { return enableLogging; }
            set
            {
                if (value == enableLogging) return;
                enableLogging = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("abilityLoggerBackgroundColour")]
        public string AbilityLoggerBackgroundColor
        {
            get { return abilityLoggerBackgroundColor; }
            set
            {
                if (value == abilityLoggerBackgroundColor) return;
                abilityLoggerBackgroundColor = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("abilityTextColor")]
        public string AbilityTextColor
        {
            get { return abilityTextColor; }
            set
            {
                if (value == abilityTextColor) return;
                abilityTextColor = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("abilityTextBorderColor")]
        public string AbilityTextBorderColor
        {
            get { return abilityTextBorderColor; }
            set
            {
                if (value == abilityTextBorderColor) return;
                abilityTextBorderColor = value;
                OnPropertyChanged();
            }
        }


        [JsonProperty("ignoreUnknownAbilities")]
        public bool IgnoreUnknownAbilities
        {
            get { return ignoreUnknownAbilities; }
            set
            {
                if (ignoreUnknownAbilities == value) return;
                ignoreUnknownAbilities = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("fontSize")]
        public int FontSize
        {
            get { return fontSize; }
            set
            {
                if (fontSize == value) return;
                fontSize = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("fontBorderThickness")]
        public int FontBorderThickness
        {
            get { return fontBorderThickness; }
            set
            {
                if (fontBorderThickness == value) return;
                fontBorderThickness = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("volume")]
        public int Volume
        {
            get { return volume; }
            set
            {
                if (value.Equals(volume)) return;
                volume = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableSound")]
        public bool EnableSound
        {
            get { return enableSound; }
            set
            {
                if (value == enableSound) return;
                enableSound = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableAbilitySettings")]
        public bool EnableAbilitySettings
        {
            get { return enableAbilitySettings; }
            set
            {
                if (value == enableAbilitySettings) return;
                enableAbilitySettings = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("abilitySettings")]
        public IEnumerable<AbilitySetting> AbilitySettings
        {
            get { return abilitySettings; }
            set
            {
                if (Equals(value, abilitySettings)) return;
                abilitySettings = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("eventSettings")]
        public IEnumerable<EventSetting> EventSettings
        {
            get { return eventSettings; }
            set
            {
                if (Equals(value, eventSettings)) return;
                eventSettings = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableDemoMode")]
        public bool EnablePlaybackMode
        {
            get { return enablePlaybackMode; }
            set
            {
                if (value == enablePlaybackMode) return;
                enablePlaybackMode = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("demoModeCombatLogFile")]
        public string CombatLogFile
        {
            get { return combatLogFile; }
            set
            {
                if (value == combatLogFile) return;
                combatLogFile = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("topWindowOpacity")]
        public double Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                if (opacity < 0.001) opacity = 0.001; // still allow enough transparancy for click-through...or do we make this another option?
                OnPropertyChanged();
            }
        }

        [JsonProperty("audioDeviceId")]
        public Guid AudioDeviceId
        {
            get { return audioDeviceId; }
            set
            {
                if (value.Equals(audioDeviceId)) return;
                audioDeviceId = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}