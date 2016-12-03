namespace SwtorCaster.ViewModels
{
    using System.Windows.Media;
    using Core;
    using Core.Services.Settings;
    using Core.Domain.Settings;
    using Caliburn.Micro;

    public class WindowedViewModel : FocusableScreen, IHandle<Settings>
    {
        private readonly ISettingsService settingsService;

        public override string DisplayName { get; set; } = "SWTOR Caster - Ability Logger";

        public WindowedViewModel(AbilityViewModel abilityViewModel, ISettingsService settingsService) 
        {
            this.settingsService = settingsService;
            AbilityViewModel = abilityViewModel;
        }

        public SolidColorBrush BackgroundColor => new SolidColorBrush(settingsService.Settings.AbilityLoggerBackgroundColor.FromHexToColor());

        public AbilityViewModel AbilityViewModel { get; }

        public void Handle(Settings message)
        {
            Refresh();
        }

        protected override void OnDeactivate(bool close)
        {
            // this.settingsService.Settings.WindowedLoggerWindowPlacementXml = this.Window.GetPlacement();
        }

        protected override void OnInitialize()
        {
            if (IsInitialized)
            {
                // this.Window.SetPlacement(this.settingsService.Settings.WindowedLoggerWindowPlacementXml);
            }
        }
    }
}