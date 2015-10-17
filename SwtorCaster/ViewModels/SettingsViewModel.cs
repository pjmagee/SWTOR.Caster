namespace SwtorCaster.ViewModels
{
    using System.Windows;
    using Caliburn.Micro;
    using Core.Services;

    public class SettingsViewModel : Screen
    {
        public override string DisplayName { get; set; } = "SWTOR Caster - Settings";

        public int Items
        {
            get { return _settingsService.Settings.Items; }
            set { _settingsService.Settings.Items = value; }
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

        private readonly ISettingsService _settingsService;
        private readonly IWindowManager _windowManager;

        public SettingsViewModel(ISettingsService settingsService, IWindowManager windowManager)
        {
            _settingsService = settingsService;
            _windowManager = windowManager;
        }

        public void Focus()
        {
            var window = GetView() as Window;
            window?.Activate();
        }
    }
}