namespace SwtorCaster.Core.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Newtonsoft.Json;

    public class AbilitySetting : INotifyPropertyChanged
    {
        private bool _enabled = true;
        private string _abilityId;
        private string _image;
        private string _abilityBorderColor;
        private List<string> _aliases = new List<string>();

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

        [JsonProperty("aliases", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Aliases
        {
            get { return _aliases; }
            set
            {
                if (Equals(value, _aliases)) return;
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