using System.Collections.Specialized;
using System.ComponentModel;

namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;
    using SwtorCaster.Core;
    using SwtorCaster.Core.Domain;
    using System.Linq;
    using System.Windows.Media;
    using Core.Services;
    using Screens;

    /// <summary>
    /// We hook into the Settings Property Changed event and any time a value changes, we serialize the settings instantly.
    /// So the user does not have to press Save changes. Changes are instant.
    /// </summary>
    public class SettingsViewModel : FocusableScreen
    {
        public override string DisplayName { get; set; } = "SWTOR Caster - Settings";

        private readonly ISettingsService _settingsService;
        
        public SettingsViewModel()
        {
           
        }

        public SettingsViewModel(ISettingsService settingsService) : this()
        {
            _settingsService = settingsService;

            AbilitySettingViewModels = new BindableCollection<AbilitySettingViewModel>();

            foreach (var item in _settingsService.Settings.AbilitySettings)
            {
                var abilitySettingviewModel = new AbilitySettingViewModel(item, this);
                abilitySettingviewModel.AbilitySetting.PropertyChanged += OnAbilitySettingOnPropertyChanged;
                AbilitySettingViewModels.Add(abilitySettingviewModel);
            }

            AbilitySettingViewModels.CollectionChanged += OnAbilitySettingViewModelsOnCollectionChanged;
        }

        private void OnAbilitySettingViewModelsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            UpdateAbilities();
        }

        private void OnAbilitySettingOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateAbilities();
        }

        private void UpdateAbilities()
        {
            _settingsService.Settings.AbilitySettings = AbilitySettingViewModels.Select(x => x.AbilitySetting).ToList();
            _settingsService.Save();
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

        public Color SelectedAbilityBackgroundColor
        {
            get { return _settingsService.Settings.AbilityLoggerBackgroundColor.ToColorFromRgb(); }
            set { _settingsService.Settings.AbilityLoggerBackgroundColor = value.ToRgbFromColor(); }
        }

        public BindableCollection<AbilitySettingViewModel> AbilitySettingViewModels { get; set; }

        public void AddAbility()
        {
            var abilitySetting = new AbilitySetting();
            var newAbilityViewModel = new AbilitySettingViewModel(abilitySetting, this);
            newAbilityViewModel.AbilitySetting.PropertyChanged += (sender, arg) => UpdateAbilities();
            AbilitySettingViewModels.Add(newAbilityViewModel);
        }
    }
}