using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace SwtorCaster.ViewModels
{
    using System.Linq;
    using Caliburn.Micro;
    using Core.Domain.Settings;
    using Core.Services.Audio;
    using Core.Services.Settings;

    public class EventSettingsViewModel : Screen
    {
        public BindableCollection<EventSettingItem> EventSettingViewModels { get; set; } = new BindableCollection<EventSettingItem>();

        private readonly ISettingsService _settingsService;
        private readonly IAudioService _audioService;

        public MetroWindow Window => (GetView() as UserControl).TryFindParent<MetroWindow>();

        public EventSettingsViewModel(ISettingsService settingsService, IAudioService audioService)
        {
            _settingsService = settingsService;
            _audioService = audioService;
            InitializeEventViewModels();
        }

        private void InitializeEventViewModels()
        {
            foreach (var item in _settingsService.Settings.EventSettings)
            {
                var eventViewModel = new EventSettingItem(this, item, _audioService, _settingsService);
                eventViewModel.EventSetting.PropertyChanged += (o, args) => UpdateEvents();
                EventSettingViewModels.Add(eventViewModel);
            }

            EventSettingViewModels.CollectionChanged += (o, args) => UpdateEvents();
        }

        public void AddEvent()
        {
            var eventSettingViewModel = new EventSettingItem(this, new EventSetting(), _audioService, _settingsService);
            eventSettingViewModel.EventSetting.PropertyChanged += (sender, args) => UpdateEvents();
            EventSettingViewModels.Add(eventSettingViewModel);
        }

        private void UpdateEvents()
        {
            _settingsService.Settings.EventSettings = EventSettingViewModels.Select(x => x.EventSetting).ToList();
            _settingsService.Save();
        }
    }
}