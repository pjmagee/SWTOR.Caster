namespace SwtorCaster.Core.Domain.Settings
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;
    using Annotations;
    using Extensions;
    using Newtonsoft.Json;

    public class Settings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IEnumerable<AbilitySetting> _abilitySettings = new List<AbilitySetting>();
        private IEnumerable<EventSetting> _eventSettings = new List<EventSetting>();

        private string _abilityLoggerBackgroundColor = Colors.Transparent.ToHex();
        private string _companionAbilityBorderColor = Colors.Purple.ToHex();

        private string _version;
        private string _combatLogFile;

        private int _items = 5; // 5 items in the ability logger window
        private int _rotate = 5; // +/- 5 degree angle rotation on image
        private int _clearAfterInactivity = 10; // 10 seconds
        private int _fontSize = 32; // default text ability font size
        private int _volume = 10; // the default volume of 100%
        private double _opacity = 0.15; // the top window opacity over the game.

        private bool _enableAbilityText = true;
        private bool _enableClearInactivity = true;
        private bool _enableCombatClear = true;
        private bool _enableCompanionAbilities = true;
        private bool _enableLogging = true;
        private bool _ignoreUnknownAbilities = true;

        private bool _enableSound;
        private bool _enableAbilitySettings;
        private bool _enableShowCrits;
        private bool _enableDemoMode;
        

        [JsonProperty("items")]
        public int Items
        {
            get { return _items; }
            set
            {
                if (value == _items) return;
                _items = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableShowCriticalHits")]
        public bool EnableShowCriticalHits
        {
            get { return _enableShowCrits; }
            set
            {
                if (value == _enableShowCrits) return;
                _enableShowCrits = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableCompanionAbilities")]
        public bool EnableCompanionAbilities
        {
            get { return _enableCompanionAbilities; }
            set
            {
                if (value == _enableCompanionAbilities) return;
                _enableCompanionAbilities = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("companionAbilityBorderColor")]
        public string CompanionAbilityBorderColor
        {
            get { return _companionAbilityBorderColor; }
            set
            {
                _companionAbilityBorderColor = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableAbilityText")]
        public bool EnableAbilityText
        {
            get { return _enableAbilityText; }
            set
            {
                if (value == _enableAbilityText) return;
                _enableAbilityText = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("rotate")]
        public int Rotate
        {
            get { return _rotate; }
            set
            {
                if (value == _rotate) return;
                _rotate = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableClearInactivity")]
        public bool EnableClearInactivity
        {
            get { return _enableClearInactivity; }
            set
            {
                if (value == _enableClearInactivity) return;
                _enableClearInactivity = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("clearAfterInactivity")]
        public int ClearAfterInactivity
        {
            get { return _clearAfterInactivity; }
            set
            {
                if (value == _clearAfterInactivity) return;
                _clearAfterInactivity = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableCombatClear")]
        public bool EnableCombatClear
        {
            get { return _enableCombatClear; }
            set
            {
                if (value == _enableCombatClear) return;
                _enableCombatClear = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("version")]
        public string Version
        {
            get { return _version; }
            set
            {
                if (value == _version) return;
                _version = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableLogging")]
        public bool EnableLogging
        {
            get { return _enableLogging; }
            set
            {
                if (value == _enableLogging) return;
                _enableLogging = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("abilityLoggerBackgroundColour")]
        public string AbilityLoggerBackgroundColor
        {
            get { return _abilityLoggerBackgroundColor; }
            set
            {
                if (value == _abilityLoggerBackgroundColor) return;
                _abilityLoggerBackgroundColor = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("ignoreUnknownAbilities")]
        public bool IgnoreUnknownAbilities
        {
            get { return _ignoreUnknownAbilities; }
            set
            {
                if (_ignoreUnknownAbilities == value) return;
                _ignoreUnknownAbilities = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("fontSize")]
        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize == value) return;
                _fontSize = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("volume")]
        public int Volume
        {
            get { return _volume; }
            set
            {
                if (value.Equals(_volume)) return;
                _volume = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableSound")]
        public bool EnableSound
        {
            get { return _enableSound; }
            set
            {
                if (value == _enableSound) return;
                _enableSound = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableAbilitySettings")]
        public bool EnableAbilitySettings
        {
            get { return _enableAbilitySettings; }
            set
            {
                if (value == _enableAbilitySettings) return;
                _enableAbilitySettings = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("abilitySettings")]
        public IEnumerable<AbilitySetting> AbilitySettings
        {
            get { return _abilitySettings; }
            set
            {
                if (Equals(value, _abilitySettings)) return;
                _abilitySettings = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("eventSettings")]
        public IEnumerable<EventSetting> EventSettings
        {
            get { return _eventSettings; }
            set
            {
                if (Equals(value, _eventSettings)) return;
                _eventSettings = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enableDemoMode")]
        public bool EnableDemoMode
        {
            get { return _enableDemoMode; }
            set
            {
                if (value == _enableDemoMode) return;
                _enableDemoMode = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("demoModeCombatLogFile")]
        public string CombatLogFile
        {
            get { return _combatLogFile; }
            set
            {
                if (value == _combatLogFile) return;
                _combatLogFile = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("topWindowOpacity")]
        public double Opacity
        {
            get { return _opacity; }
            set
            {
                if (value == _opacity) return;
                _opacity = value;
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