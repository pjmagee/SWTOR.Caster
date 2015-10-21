using System;

namespace SwtorCaster.ViewModels
{
    using System.Windows.Media;
    using SwtorCaster.Core;
    using SwtorCaster.Core.Domain;

    public class AbilitySettingViewModel
    {
        public readonly AbilitySetting _abilitySetting;

        public AbilitySetting AbilitySetting => _abilitySetting;

        public AbilitySettingViewModel(AbilitySetting abilitySetting)
        {
            if (abilitySetting == null)
                throw new ArgumentNullException(nameof(abilitySetting));

            _abilitySetting = abilitySetting;
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
    }
}