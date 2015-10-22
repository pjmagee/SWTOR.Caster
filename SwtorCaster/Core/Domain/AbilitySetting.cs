using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SwtorCaster.Annotations;

namespace SwtorCaster.Core.Domain
{
    public class AbilitySetting : INotifyPropertyChanged
    {
        private Settings _settings;

        [JsonIgnore] private bool _enabled;

        [JsonIgnore] private string _abilityId;

        [JsonIgnore] private string _image;

        [JsonIgnore] private string _abilityBorderColor;

        [JsonIgnore] private string _aliases;

        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("abilityId")]
        public string AbilityId
        {
            get { return _abilityId; }
            set
            {
                if (value == _abilityId) return;
                _abilityId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("image")]
        public string Image
        {
            get { return _image; }
            set
            {
                if (value == _image) return;
                _image = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("borderColor")]
        public string BorderColor
        {
            get { return _abilityBorderColor; }
            set
            {
                if (value == _abilityBorderColor) return;
                _abilityBorderColor = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("aliases")]
        public string Aliases
        {
            get { return _aliases; }
            set
            {
                if (value == _aliases) return;
                _aliases = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enabled")]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value == _enabled) return;
                _enabled = value;
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