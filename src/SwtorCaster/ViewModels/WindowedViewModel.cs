namespace SwtorCaster.ViewModels
{
    using System.Windows.Media;
    using Core;
    using Core.Services.Settings;
    using Core.Domain.Settings;
    using Caliburn.Micro;
    using System.Windows;

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
            base.OnDeactivate(close);

            SaveWindowLocation();
        }

        private void SaveWindowLocation()
        {
            settingsService.Settings.LoggerWindowLocation = new Point(Window.Left, Window.Top);
        }

        protected override void OnInitialize()
        {
            SetWindowLocation();
        }

        private void SetWindowLocation()
        {
            if (settingsService.Settings.LoggerWindowLocation == default(Point)) return;
            Window.WindowStartupLocation = WindowStartupLocation.Manual;
            Window.Left = settingsService.Settings.LoggerWindowLocation.X;
            Window.Top = settingsService.Settings.LoggerWindowLocation.Y;
        }
    }
}