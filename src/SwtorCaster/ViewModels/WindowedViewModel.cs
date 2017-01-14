namespace SwtorCaster.ViewModels
{
    using System.Windows.Media;
    using Caliburn.Micro;
    using System.Windows;
    using Core.Services.Settings;
    using Core;
    using Core.Domain.Settings;

    public class WindowedViewModel : FocusableScreen, IHandle<AppSettings>
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

        public void Handle(AppSettings message)
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