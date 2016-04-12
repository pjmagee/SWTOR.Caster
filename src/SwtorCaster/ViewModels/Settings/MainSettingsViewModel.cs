using Microsoft.Win32;

namespace SwtorCaster.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Media;
    using System.IO;
    using Caliburn.Micro;
    using Core.Extensions;
    using Core.Services.Settings;
    using System.Collections.Generic;

    public class MainSettingsViewModel : PropertyChangedBase
    {
        private readonly ISettingsService settingsService;
        private readonly IFontService fontService;

        public MainSettingsViewModel(ISettingsService settingsService, IFontService fontService)
        {
            this.settingsService = settingsService;
            this.fontService = fontService;
        }

        public IEnumerable<FontFamily> SelectableFonts => fontService.GetSelectableFonts();

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

        public int Rotate
        {
            get { return settingsService.Settings.Rotate; }
            set { settingsService.Settings.Rotate = value; }
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

        public bool IgnoreUnknownAbilities
        {
            get { return settingsService.Settings.IgnoreUnknownAbilities; }
            set { settingsService.Settings.IgnoreUnknownAbilities = value; }
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

        public Color SelectedCompanionAbilityBorderColor
        {
            get { return settingsService.Settings.CompanionAbilityBorderColor.FromHexToColor(); }
            set { settingsService.Settings.CompanionAbilityBorderColor = value.ToHex(); }
        }
    }
}