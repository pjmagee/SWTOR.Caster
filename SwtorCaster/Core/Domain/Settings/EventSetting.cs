namespace SwtorCaster.Core.Domain.Settings
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Log;
    using Newtonsoft.Json;
    using Properties;

    public class EventSetting : INotifyPropertyChanged
    {
        private bool _enabled;
        private SoundEvent _effectName = SoundEvent.AbilityActivate;
        private string _abilityId;
        private string _sound;

        public event PropertyChangedEventHandler PropertyChanged;

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

        [JsonProperty("effectName")]
        public SoundEvent EffectName
        {
            get { return _effectName; }
            set
            {
                if (value == _effectName) return;
                _effectName = value;
                OnPropertyChanged();
            }
        }

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

        [JsonProperty("soundFile")]
        public string Sound
        {
            get { return _sound; }
            set
            {
                if (value == _sound) return;
                _sound = value;
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