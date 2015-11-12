namespace SwtorCaster.ViewModels
{
    using System.Windows.Media;
    using Core.Extensions;
    using Core.Services.Settings;
    using Core.Domain.Settings;
    using Screens;
    using Caliburn.Micro;

    public class ObsViewModel : FocusableScreen, IHandle<Settings>
    {
        private readonly ISettingsService _settingsService;

        public override string DisplayName { get; set; } = "SWTOR Caster - Ability Logger";

        public ObsViewModel(AbilityListViewModel abilityListViewModel, ISettingsService settingsService)
        {
            _settingsService = settingsService;
            AbilityListViewModel = abilityListViewModel;
        }

        public SolidColorBrush BackgroundColor => new SolidColorBrush(_settingsService.Settings.AbilityLoggerBackgroundColor.FromHexToColor());

        public AbilityListViewModel AbilityListViewModel { get; }

        public void Handle(Settings message)
        {
            Refresh();
        }
    }
}