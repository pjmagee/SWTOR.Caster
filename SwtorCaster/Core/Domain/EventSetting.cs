namespace SwtorCaster.Core.Domain
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Newtonsoft.Json;

    public class EventSetting : INotifyPropertyChanged
    {
        private bool _enabled;
        private EventDetailType _eventType;
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

        [JsonProperty("eventType")]
        public EventDetailType EventType
        {
            get { return _eventType; }
            set
            {
                if (value == _eventType) return;
                _eventType = value;
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