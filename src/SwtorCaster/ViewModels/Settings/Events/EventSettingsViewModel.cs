namespace SwtorCaster.ViewModels
{
    using System.Linq;
    using Caliburn.Micro;
    using Core.Domain.Settings;
    using Core.Services.Audio;
    using Core.Services.Settings;
    using System.Windows.Controls;
    using MahApps.Metro.Controls;
    using System.Collections.Generic;
    using System;

    public class EventSettingsViewModel : Screen
    {
        public BindableCollection<EventSettingItem> EventSettingViewModels { get; set; } = new BindableCollection<EventSettingItem>();

        private readonly ISettingsService settingsService;
        private readonly IAudioService audioService;

        public MetroWindow Window => (GetView() as UserControl).TryFindParent<MetroWindow>();

        public IEnumerable<KeyValuePair<string, Guid>> SoundDevices => audioService.GetAudioDevices();

        public bool EnableSound
        {
            get { return settingsService.Settings.EnableSound; }
            set { settingsService.Settings.EnableSound = value; }
        }

        public int Volume
        {
            get { return settingsService.Settings.Volume; }
            set { settingsService.Settings.Volume = value; }
        }

        public KeyValuePair<string, Guid> SelectedSoundDevice
        {
            set {  settingsService.Settings.AudioDeviceId = value.Value; }
            get
            {
                return SoundDevices.First(x => x.Value == settingsService.Settings.AudioDeviceId);
            }
        }

        public EventSettingsViewModel(ISettingsService settingsService, IAudioService audioService)
        {
            this.settingsService = settingsService;
            this.audioService = audioService;
            InitializeEventViewModels();
        }

        private void InitializeEventViewModels()
        {
            foreach (var item in settingsService.Settings.EventSettings)
            {
                var eventViewModel = new EventSettingItem(this, item, audioService, settingsService);
                eventViewModel.EventSetting.PropertyChanged += (o, args) => UpdateEvents();
                EventSettingViewModels.Add(eventViewModel);
            }

            EventSettingViewModels.CollectionChanged += (o, args) => UpdateEvents();
        }

        public void AddEvent()
        {
            var eventSettingViewModel = new EventSettingItem(this, new EventSetting(), audioService, settingsService);
            eventSettingViewModel.EventSetting.PropertyChanged += (sender, args) => UpdateEvents();
            EventSettingViewModels.Add(eventSettingViewModel);
        }

        private void UpdateEvents()
        {
            settingsService.Settings.EventSettings = EventSettingViewModels.Select(x => x.EventSetting).ToList();
            settingsService.Save();
        }
    }
}