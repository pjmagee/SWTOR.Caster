namespace SwtorCaster.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Media;
    using Caliburn.Micro;
    using Core;
    using Core.Domain;
    using Core.Services.Audio;
    using Core.Services.Settings;
    using Screens;
    
    /// <summary>
    /// We hook into the Settings Property Changed event and any time a value changes, we serialize the settings instantly.
    /// So the user does not have to press Save changes. Changes are instant.
    /// </summary>
    public class SettingsViewModel : FocusableScreen
    {
        public override string DisplayName { get; set; } = "SWTOR Caster - Settings";

        private readonly ISettingsService _settingsService;
        private readonly IAudioService _audioService;

        public BindableCollection<AbilitySettingViewModel> AbilitySettingViewModels { get; set; }
        public BindableCollection<EventSettingViewModel> EventSettingViewModels { get; set; }

        public SettingsViewModel(ISettingsService settingsService, IAudioService audioService)
        {
            _settingsService = settingsService;
            _audioService = audioService;
            InitializeAbilityViewModels();
            InitializeEventSettingViewModels();
        }

        private void InitializeEventSettingViewModels()
        {
            EventSettingViewModels = new BindableCollection<EventSettingViewModel>();

            foreach (var item in _settingsService.Settings.EventSettings)
            {
                var eventViewModel = new EventSettingViewModel(item, this, _audioService);
                eventViewModel.EventSetting.PropertyChanged += (o, args) => UpdateAbilities();
                EventSettingViewModels.Add(eventViewModel);
            }

            EventSettingViewModels.CollectionChanged += (o, args) => UpdateAbilities();
        }

        private void InitializeAbilityViewModels()
        {
            AbilitySettingViewModels = new BindableCollection<AbilitySettingViewModel>();

            foreach (var item in _settingsService.Settings.AbilitySettings)
            {
                var abilityViewModel = new AbilitySettingViewModel(item, this);
                abilityViewModel.AbilitySetting.PropertyChanged += (o, args) => UpdateAbilities();
                AbilitySettingViewModels.Add(abilityViewModel);
            }

            AbilitySettingViewModels.CollectionChanged += (o, args) => UpdateAbilities();
        }

        private void UpdateAbilities()
        {
            _settingsService.Settings.AbilitySettings = AbilitySettingViewModels.Select(x => x.AbilitySetting).ToList();
            _settingsService.Save();
        }

        private void UpdateEvents()
        {
            _settingsService.Settings.EventSettings = EventSettingViewModels.Select(x => x.EventSetting).ToList();
            _settingsService.Save();
        }

        #region Main Settings

        public int Items
        {
            get { return _settingsService.Settings.Items; }
            set { _settingsService.Settings.Items = value; }
        }

        public bool EnableCompanionAbilities
        {
            get { return _settingsService.Settings.EnableCompanionAbilities; }
            set { _settingsService.Settings.EnableCompanionAbilities = value; }
        }

        public string SoundOnDeath
        {
            get { return _settingsService.Settings.SoundOnDeath; }
            set { _settingsService.Settings.SoundOnDeath = value; }
        }

        public bool EnableSound
        {
            get { return _settingsService.Settings.EnableSound; }
            set { _settingsService.Settings.EnableSound = value; }
        }

        public int Volume
        {
            get { return _settingsService.Settings.Volume; }
            set { _settingsService.Settings.Volume = value; }
        }

        public int Rotate
        {
            get { return _settingsService.Settings.Rotate; }
            set { _settingsService.Settings.Rotate = value; }
        }

        public bool EnableExitCombatClear
        {
            get { return _settingsService.Settings.EnableCombatClear; }
            set { _settingsService.Settings.EnableCombatClear = value; }
        }

        public bool EnableAbilityText
        {
            get { return _settingsService.Settings.EnableAbilityText; }
            set { _settingsService.Settings.EnableAbilityText = value; }
        }

        public bool EnableInactivityClear
        {
            get { return _settingsService.Settings.EnableClearInactivity; }
            set { _settingsService.Settings.EnableClearInactivity = value; }
        }

        public int ClearAfterInactivity
        {
            get { return _settingsService.Settings.ClearAfterInactivity; }
            set { _settingsService.Settings.ClearAfterInactivity = value; }
        }

        public bool EnableLogging
        {
            get { return _settingsService.Settings.EnableLogging; }
            set { _settingsService.Settings.EnableLogging = value; }
        }

        public bool IgnoreUnknownAbilities
        {
            get { return _settingsService.Settings.IgnoreUnknownAbilities; }
            set { _settingsService.Settings.IgnoreUnknownAbilities = value; }
        }

        public int FontSize
        {
            get { return _settingsService.Settings.FontSize; }
            set { _settingsService.Settings.FontSize = value; }
        }

        public Color SelectedAbilityBackgroundColor
        {
            get { return _settingsService.Settings.AbilityLoggerBackgroundColor.ToColorFromRgb(); }
            set { _settingsService.Settings.AbilityLoggerBackgroundColor = value.ToRgbFromColor(); }
        }

        #endregion

        public IEnumerable<SoundItem> Sounds => new List<SoundItem>()
        {
            new SoundItem("2SAD4ME", Path.Combine(Environment.CurrentDirectory, "Resources/Sounds", "2SAD4ME.mp3")),
            new SoundItem("2SED4AIRHORN", Path.Combine(Environment.CurrentDirectory, "Resources/Sounds", "2SED4AIRHORN.mp3")),
            new SoundItem("Darude DankStorm", Path.Combine(Environment.CurrentDirectory, "Resources/Sounds", "Darude - DarkStorm.mp3")),
            new SoundItem("NEVER DONE THAT", Path.Combine(Environment.CurrentDirectory, "Resources/Sounds", "NEVER DONE THAT.mp3")),
            new SoundItem("SKRILLEX Scary", Path.Combine(Environment.CurrentDirectory, "Resources/Sounds", "SKRILLEX Scary.mp3")),
            new SoundItem("SPOOKY", Path.Combine(Environment.CurrentDirectory, "Resources/Sounds", "SPOOKY.mp3")),
            new SoundItem("tactical nuke", Path.Combine(Environment.CurrentDirectory, "Resources/Sounds", "tactical nuke.mp3")),
            new SoundItem("wow-", Path.Combine(Environment.CurrentDirectory, "Resources/Sounds", "wow-.mp3")),
        };

        public void AddAbility()
        {
            var abilitySetting = new AbilitySetting();
            var abilityViewModel = new AbilitySettingViewModel(abilitySetting, this);
            abilityViewModel.AbilitySetting.PropertyChanged += (sender, arg) => UpdateAbilities();
            AbilitySettingViewModels.Add(abilityViewModel);
        }

        public void AddEvent()
        {
            var eventSetting = new EventSetting();
            var eventSettingViewModel = new EventSettingViewModel(eventSetting, this, _audioService);
            eventSettingViewModel.EventSetting.PropertyChanged += (sender, args) => UpdateEvents();
            EventSettingViewModels.Add(eventSettingViewModel);
        }
    }
}