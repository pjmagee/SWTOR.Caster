namespace SwtorCaster.ViewModels
{
    using System;
    using System.Windows.Media;
    using Core;
    using Core.Services.Settings;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Core.Services.Combat;

    public class MainSettingsViewModel : Caliburn.Micro.Screen
    {
        private readonly ISettingsService settingsService;
        private readonly IFontService fontService;

        public MainSettingsViewModel(ISettingsService settingsService, IFontService fontService)
        {
            this.settingsService = settingsService;
            this.fontService = fontService;
        }

        public IEnumerable<FontFamily> SelectableFonts => fontService.GetSelectableFonts();

        public string FontsPath => fontService.FontsPath;

        public double TopWindowOpacity
        {
            get { return settingsService.Settings.Opacity; }
            set { settingsService.Settings.Opacity = Math.Round(value, 3); }
        }

        public bool OpenLoggerWindowOnStartup
        {
            get { return settingsService.Settings.OpenLoggerWindowOnStartup; }
            set { settingsService.Settings.OpenLoggerWindowOnStartup = value; }
        }

        public string CustomCombatLogDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(settingsService.Settings.CustomCombatLogDirectory)) return RealTimeLogService.SwtorCombatLogPath;
                return settingsService.Settings.CustomCombatLogDirectory;
            }
            set
            {
                settingsService.Settings.CustomCombatLogDirectory = value;                
            }
        }

        public void AddCustomCombatLogDirectory()
        {
            using (var folderSelector = new FolderBrowserDialog())
            {
                folderSelector.ShowNewFolderButton = false;
                var result = folderSelector.ShowDialog();

                if (result == DialogResult.OK)
                {
                    CustomCombatLogDirectory = folderSelector.SelectedPath;
                    NotifyOfPropertyChange(() => CustomCombatLogDirectory);
                }                
            }
        }

        public void DefaultCustomCombatLogDirectory()
        {
            CustomCombatLogDirectory = null;
            NotifyOfPropertyChange(() => CustomCombatLogDirectory);
        }

        public int Items
        {
            get { return settingsService.Settings.Items; }
            set { settingsService.Settings.Items = value; }
        }

        public bool EnableCompanionAbilities
        {
            get { return settingsService.Settings.EnableCompanionAbilities; }
            set { settingsService.Settings.EnableCompanionAbilities = value; }
        }

        public bool EnableCombatClear
        {
            get { return settingsService.Settings.EnableCombatClear; }
            set { settingsService.Settings.EnableCombatClear = value; }
        }

        public bool EnableAbilityText
        {
            get { return settingsService.Settings.EnableAbilityText; }
            set { settingsService.Settings.EnableAbilityText = value; }
        }

        public FontFamily TextFont
        {
            get { return fontService.GetFontFromString(settingsService.Settings.TextFont); }
            set { settingsService.Settings.TextFont = fontService.GetStringFromFont(value); }
        }

        public bool EnableInactivityClear
        {
            get { return settingsService.Settings.EnableClearInactivity; }
            set { settingsService.Settings.EnableClearInactivity = value; }
        }

        public int ClearAfterInactivity
        {
            get { return settingsService.Settings.ClearAfterInactivity; }
            set { settingsService.Settings.ClearAfterInactivity = value; }
        }

        public bool EnableLogging
        {
            get { return settingsService.Settings.EnableLogging; }
            set { settingsService.Settings.EnableLogging = value; }
        }

        public int FontSize
        {
            get { return settingsService.Settings.FontSize; }
            set { settingsService.Settings.FontSize = value; }
        }

        public Color SelectedAbilityBackgroundColor
        {
            get { return settingsService.Settings.AbilityLoggerBackgroundColor.FromHexToColor(); }
            set { settingsService.Settings.AbilityLoggerBackgroundColor = value.ToHex(); }
        }

        public Color SelectedAbilityTextColor
        {
            get { return settingsService.Settings.AbilityTextColor.FromHexToColor(); }
            set { settingsService.Settings.AbilityTextColor = value.ToHex(); }
        }

        public Color SelectedAbilityTextBorderColor
        {
            get { return settingsService.Settings.AbilityTextBorderColor.FromHexToColor(); }
            set { settingsService.Settings.AbilityTextBorderColor = value.ToHex(); }
        }

        public int FontBorderThickness
        {
            get { return settingsService.Settings.FontBorderThickness; }
            set { settingsService.Settings.FontBorderThickness = value; }
        }

        public Color SelectedCompanionAbilityBorderColor
        {
            get { return settingsService.Settings.CompanionAbilityBorderColor.FromHexToColor(); }
            set { settingsService.Settings.CompanionAbilityBorderColor = value.ToHex(); }
        }
    }
}