using Caliburn.Micro;

namespace SwtorCaster.ViewModels
{
    using System.Windows.Media;
    using SwtorCaster.Core;
    using SwtorCaster.Core.Domain;

    public class AbilitySettingViewModel : PropertyChangedBase
    {
        private readonly AbilitySetting _abilitySetting;
        private readonly SettingsViewModel _settingsViewModel;

        public AbilitySetting AbilitySetting => _abilitySetting;

        public AbilitySettingViewModel(AbilitySetting abilitySetting, SettingsViewModel settingsViewModel)
        {
            _abilitySetting = abilitySetting;
            _settingsViewModel = settingsViewModel;
        }

        public string Id
        {
            get { return _abilitySetting.AbilityId; }
            set { _abilitySetting.AbilityId = value; }
        }

        public string Image
        {
            get { return _abilitySetting.Image; }
            set { _abilitySetting.Image = value; }
        }

        public Color Border
        {
            get { return _abilitySetting.BorderColor.ToColorFromRgb(); }
            set { _abilitySetting.BorderColor = value.ToRgbFromColor(); }
        }

        public void Delete()
        {
            _settingsViewModel.AbilitySettingViewModels.Remove(this);
        }
    }
}