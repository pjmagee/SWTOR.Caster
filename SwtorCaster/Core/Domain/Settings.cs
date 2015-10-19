namespace SwtorCaster.Core.Domain
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Newtonsoft.Json;


    public class Settings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore] private string _version = string.Empty;

        [JsonIgnore] private int _items = 5;

        [JsonIgnore] private int _rotate = 5;

        [JsonIgnore] private int _clearAfterInactivity = 10;

        [JsonIgnore] private bool _enableAbilityText = true;

        [JsonIgnore] private bool _enableClearInactivity = true;

        [JsonIgnore] private bool _enableCombatClear = true;

        [JsonIgnore] private bool _enableCompanionAbilities = true;

        [JsonIgnore] private bool _enableLogging = true;

        [JsonProperty("maxAbilityList")]
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
                _version = value;
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}