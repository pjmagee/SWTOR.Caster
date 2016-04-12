namespace SwtorCaster.Core.Domain.Settings
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Log;
    using Newtonsoft.Json;

    public class EventSetting : INotifyPropertyChanged
    {
        private bool enabled;
        private SoundEvent effectName = SoundEvent.AbilityActivate;
        private string abilityId;
        private string sound;

        public event PropertyChangedEventHandler PropertyChanged;

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

        [JsonProperty("effectName")]
        public SoundEvent EffectName
        {
            get { return effectName; }
            set
            {
                if (value == effectName) return;
                effectName = value;
                OnPropertyChanged();
            }
        }

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

        [JsonProperty("soundFile")]
        public string Sound
        {
            get { return sound; }
            set
            {
                if (value == sound) return;
                sound = value;
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