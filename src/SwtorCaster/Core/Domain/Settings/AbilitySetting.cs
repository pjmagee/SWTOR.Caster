namespace SwtorCaster.Core.Domain.Settings
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Newtonsoft.Json;

    public class AbilitySetting : INotifyPropertyChanged
    {
        private bool enabled = true;
        private string abilityId;
        private string image;
        private string abilityBorderColor;
        private List<string> aliases = new List<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("abilityId")]
        public string AbilityId
        {
            get { return abilityId; }
            set
            {
                if (value == abilityId) return;
                abilityId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("image")]
        public string Image
        {
            get { return image; }
            set
            {
                if (value == image) return;
                image = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("borderColor")]
        public string BorderColor
        {
            get { return abilityBorderColor; }
            set
            {
                if (value == abilityBorderColor) return;
                abilityBorderColor = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("aliases", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Aliases
        {
            get { return aliases; }
            set
            {
                if (Equals(value, aliases)) return;
                aliases = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("enabled")]
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (value == enabled) return;
                enabled = value;
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