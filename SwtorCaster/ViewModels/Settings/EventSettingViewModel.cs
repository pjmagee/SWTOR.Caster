namespace SwtorCaster.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Caliburn.Micro;
    using Core.Domain;
    using Core.Services.Audio;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;
    using Microsoft.Win32;

    public class EventSettingViewModel : PropertyChangedBase
    {
        private readonly EventSetting _eventSetting;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly IAudioService _audioService;

        public EventSetting EventSetting => _eventSetting;

        public IEnumerable<EventDetailType> EventDetailTypes => new[]
        {
            EventDetailType.EnterCombat,
            EventDetailType.ExitCombat,
            EventDetailType.AbilityActivate,
            EventDetailType.AbilityCancel,
            EventDetailType.Death,
            EventDetailType.Kill,
        };

        private MetroWindow Window => _settingsViewModel.GetView() as MetroWindow;

        public EventSettingViewModel(
            EventSetting eventSetting,
            SettingsViewModel settingsViewModel,
            IAudioService audioService)
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

        public string Sound
        {
            get { return Path.GetFileNameWithoutExtension(_eventSetting.Sound); }
            set
            {
                _eventSetting.Sound = value;
                NotifyOfPropertyChange(() => Sound);
            }
        }

        public bool Enabled
        {
            get { return _eventSetting.Enabled; }
            set { _eventSetting.Enabled = value; }
        }

        public EventDetailType SelectedEventDetailType
        {
            get { return _eventSetting.EventDetailType; }
            set { _eventSetting.EventDetailType = value; }
        }

        public void AddAudio()
        {
            FileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Audio files (mp3,wav,wma)|*.mp3;*.wav;*wma",
                Title = "Select audio to play",
                CheckPathExists = true,
                CheckFileExists = true
            };

            var result = fileDialog.ShowDialog();

            if (result.GetValueOrDefault())
            {
                Sound = fileDialog.FileName;
            }
        }

        public async void Play()
        {
            try
            {
                _audioService.Play(_eventSetting.Sound, _settingsViewModel.Volume);
            }
            catch (Exception e)
            {
                await Window.ShowMessageAsync("Error playing sound", e.Message);
            }
        }

        public void Stop()
        {
            try
            {
                _audioService.Stop();
            }
            catch
            {

            }
        }

        public async void Delete()
        {
            var result = await Window.ShowMessageAsync("Delete sound setting", "Are you sure?", MessageDialogStyle.AffirmativeAndNegative);

            if (result == MessageDialogResult.Affirmative)
            {
                _settingsViewModel.EventSettingViewModels.Remove(this);
            }
        }
    }
}