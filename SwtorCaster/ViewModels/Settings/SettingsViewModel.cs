namespace SwtorCaster.ViewModels
{
    using System.IO;
    using System.Linq;
    using System.Windows.Media;
    using Caliburn.Micro;
    using Core.Domain.Settings;
    using Core.Services.Audio;
    using Core.Services.Settings;
    using Screens;
    using Core.Extensions;
    using Microsoft.Win32;

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

        protected IWindowManager WindowManager { get; }

        public SettingsViewModel(ISettingsService settingsService, IAudioService audioService, IWindowManager windowManager)
        {
            _settingsService = settingsService;
            _audioService = audioService;
            WindowManager = windowManager;

            InitializeAbilityViewModels();
            InitializeEventViewModels();
        }

        #region Main Settings

        public bool EnableDemoMode
        {
            get { return _settingsService.Settings.EnableDemoMode; }
            set { _settingsService.Settings.EnableDemoMode = value; }
        }

        public bool EnableShowCriticalHits
        {
            get { return _settingsService.Settings.EnableShowCriticalHits; }
            set { _settingsService.Settings.EnableShowCriticalHits = value; }
        }

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

        public bool EnableAbilitySettings
        {
            get { return _settingsService.Settings.EnableAbilitySettings; }
            set { _settingsService.Settings.EnableAbilitySettings = value; }
        }

        public bool EnableCombatClear
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

        public string CombatLogFile
        {
            get { return _settingsService.Settings.CombatLogFile; }
            set { _settingsService.Settings.CombatLogFile = value; }
        }

        public string SelectedCombatLogFile
        {
            get
            {
                try
                {
                    return Path.GetFileNameWithoutExtension(_settingsService.Settings.CombatLogFile);
                }
                catch
                {

                }

                return "No file selected.";
            }
        }

        public int FontSize
        {
            get { return _settingsService.Settings.FontSize; }
            set { _settingsService.Settings.FontSize = value; }
        }

        public Color SelectedAbilityBackgroundColor
        {
            get { return _settingsService.Settings.AbilityLoggerBackgroundColor.FromHexToColor(); }
            set { _settingsService.Settings.AbilityLoggerBackgroundColor = value.ToHex(); }
        }

        public Color SelectedCompanionAbilityBorderColor
        {
            get { return _settingsService.Settings.CompanionAbilityBorderColor.FromHexToColor(); }
            set { _settingsService.Settings.CompanionAbilityBorderColor = value.ToHex(); }
        }

        public void PickFile()
        {
            FileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Combat log (*.txt)|*.txt;",
                Title = "Select combat log file",
                CheckPathExists = true,
                CheckFileExists = true
            };

            var result = fileDialog.ShowDialog();

            if (result.GetValueOrDefault())
            {
                CombatLogFile = fileDialog.FileName;
                NotifyOfPropertyChange(() => SelectedCombatLogFile);
            }
        }

        public void ClearFile()
        {
            CombatLogFile = string.Empty;
            NotifyOfPropertyChange(() => SelectedCombatLogFile);
        }

        #endregion

        #region Ability Settings

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

        public void AddAbility()
        {
            var abilitySetting = new AbilitySetting();
            var abilityViewModel = new AbilitySettingViewModel(abilitySetting, this);
            abilityViewModel.AbilitySetting.PropertyChanged += (sender, arg) => UpdateAbilities();
            AbilitySettingViewModels.Add(abilityViewModel);
        }

        private void UpdateAbilities()
        {
            _settingsService.Settings.AbilitySettings = AbilitySettingViewModels.Select(x => x.AbilitySetting).ToList();
            _settingsService.Save();
        }

        #endregion

        #region Event Settings

        private void InitializeEventViewModels()
        {
            EventSettingViewModels = new BindableCollection<EventSettingViewModel>();

            foreach (var item in _settingsService.Settings.EventSettings)
            {
                var eventViewModel = new EventSettingViewModel(item, this, _audioService);
                eventViewModel.EventSetting.PropertyChanged += (o, args) => UpdateEvents();
                EventSettingViewModels.Add(eventViewModel);
            }

            EventSettingViewModels.CollectionChanged += (o, args) => UpdateEvents();
        }

        public void AddEvent()
        {
            var eventSetting = new EventSetting();
            var eventSettingViewModel = new EventSettingViewModel(eventSetting, this, _audioService);
            eventSettingViewModel.EventSetting.PropertyChanged += (sender, args) => UpdateEvents();
            EventSettingViewModels.Add(eventSettingViewModel);
        }

        private void UpdateEvents()
        {
            _settingsService.Settings.EventSettings = EventSettingViewModels.Select(x => x.EventSetting).ToList();
            _settingsService.Save();
        }

        #endregion
    }
}