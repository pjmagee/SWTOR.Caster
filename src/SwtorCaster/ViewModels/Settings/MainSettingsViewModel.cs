namespace SwtorCaster.ViewModels
{
    using System;
    using System.IO;
    using System.Windows.Media;
    using Caliburn.Micro;
    using Microsoft.Win32;
    using Core.Extensions;
    using Core.Services.Settings;

    public class MainSettingsViewModel : PropertyChangedBase
    {
        private readonly ISettingsService _settingsService;

        public MainSettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public double TopWindowOpacity
        {
            get { return _settingsService.Settings.Opacity; }
            set { _settingsService.Settings.Opacity = Math.Round(value, 3); }
        }

        public bool OpenLoggerWindowOnStartup
        {
            get { return _settingsService.Settings.OpenLoggerWindowOnStartup; }
            set { _settingsService.Settings.OpenLoggerWindowOnStartup = value; }
        }

        public bool EnableDemoMode
        {
            get { return _settingsService.Settings.EnableDemoMode; }
            set { _settingsService.Settings.EnableDemoMode = value; }
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

        public FontFamily TextFont
        {
            get { return _settingsService.Settings.TextFont.FromStringToFont(); }
            set { _settingsService.Settings.TextFont = value.FromFontToString(); }
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

        public int FontSize
        {
            get { return _settingsService.Settings.FontSize; }
            set { _settingsService.Settings.FontSize = value; }
        }

        public Color SelectedAbilityBackgroundColor
        {
            get { return _settingsService.Settings.AbilityTextColor.FromHexToColor(); }
            set { _settingsService.Settings.AbilityTextColor = value.ToHex(); }
        }

        public Color SelectedAbilityTextColor
        {
            get { return _settingsService.Settings.AbilityTextColor.FromHexToColor(); }
            set { _settingsService.Settings.AbilityTextColor = value.ToHex(); }
        }

        public Color SelectedCompanionAbilityBorderColor
        {
            get { return _settingsService.Settings.CompanionAbilityBorderColor.FromHexToColor(); }
            set { _settingsService.Settings.CompanionAbilityBorderColor = value.ToHex(); }
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
                    // ignored
                }

                return "No file selected.";
            }
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
    }
}