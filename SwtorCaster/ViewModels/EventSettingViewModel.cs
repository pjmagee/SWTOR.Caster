namespace SwtorCaster.ViewModels
{
    using Core.Domain;
    using Core.Services.Audio;

    public class EventSettingViewModel
    {
        private readonly EventSetting _eventSetting;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly IAudioService _audioService;

        public EventSetting EventSetting => _eventSetting;

        public EventSettingViewModel(EventSetting eventSetting, SettingsViewModel settingsViewModel, IAudioService audioService)
        {
            _eventSetting = eventSetting;
            _settingsViewModel = settingsViewModel;
            _audioService = audioService;
        }

        public string AbilityId
        {
            get { return _eventSetting.AbilityId; }
            set { _eventSetting.AbilityId = value; }
        }

        public string SoundFile
        {
            get { return _eventSetting.Sound; }
            set { _eventSetting.Sound = value; }
        }

        public bool Enabled
        {
            get { return _eventSetting.Enabled; }
            set { _eventSetting.Enabled = value; }
        }

        public EventDetailType Type
        {
            get { return _eventSetting.EventType; }
            set { _eventSetting.EventType = value; }
        }

        public void Play()
        {
            _audioService.Play(SoundFile, _settingsViewModel.Volume);
        }

        public void Stop()
        {
            _audioService.Stop();
        }
    }
}