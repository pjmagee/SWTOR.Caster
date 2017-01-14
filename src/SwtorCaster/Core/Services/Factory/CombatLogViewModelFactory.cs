namespace SwtorCaster.Core.Services.Factory
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Domain.Log;
    using Domain.Settings;
    using Images;
    using Settings;
    using ViewModels;

    public class CombatLogViewModelFactory : ICombatLogViewModelFactory
    {
        private readonly Random random = new Random();
        private readonly ISettingsService settingsService;
        private readonly IImageService imageService;
        private readonly IFontService fontService;

        public CombatLogViewModelFactory(ISettingsService settingsService, IImageService imageService, IFontService fontService)
        {
            this.settingsService = settingsService;
            this.imageService = imageService;
            this.fontService = fontService;
        }

        public CombatLogViewModel Create(CombatLogEvent @event)
        {
            var viewModel = new CombatLogViewModel(@event);
            var settings = settingsService.Settings;

            if (@event.IsAbilityActivate() || @event.IsApplyEffect())
            {
                ApplyLoggerSettings(@event, viewModel, settings);
                ApplyAbilitySettings(@event, viewModel, settings);
            }

            if (settings.EnableCompanionAbilities && @event.IsPlayerCompanion())
            {
                viewModel.ImageBorderColor = settings.CompanionAbilityBorderColor.FromHexToColor();
            }

            ApplyFontSettings(viewModel, settings);

            return viewModel;
        }

        private void ApplyFontSettings(CombatLogViewModel viewModel, AppSettings settings)
        {
            viewModel.FontColor = new SolidColorBrush(settings.AbilityTextColor.FromHexToColor());
            viewModel.FontFamily = fontService.GetFontFromString(settings.TextFont);
            viewModel.FontSize = settings.FontSize;
            viewModel.FontBorderColor = new SolidColorBrush(settings.AbilityTextBorderColor.FromHexToColor());
            viewModel.FontBorderThickness = settings.FontBorderThickness;
        }

        private void ApplyLoggerSettings(CombatLogEvent combatLogEvent, CombatLogViewModel viewModel, AppSettings settings)
        {
            var abilityId = combatLogEvent.Ability.EntityId;

            viewModel.ImageUrl = imageService.GetImageById(abilityId);
            viewModel.IsUnknown = imageService.IsUnknown(abilityId);
            viewModel.ImageBorderColor = Colors.Transparent;
            viewModel.Text = combatLogEvent.Ability.DisplayName;
            viewModel.TextVisibility = settings.EnableAbilityText ? Visibility.Visible : Visibility.Hidden;
            viewModel.TooltipText = $"{combatLogEvent.Ability.EntityId} (Click to copy Ability ID to Clipboard!)";
        }

        private static void ApplyAbilitySettings(CombatLogEvent @event, CombatLogViewModel viewModel, AppSettings settings)
        {
            if (!settings.EnableAbilitySettings) return;

            var abilitySetting = settings.AbilitySettings
                              .FirstOrDefault(s => s.AbilityId == @event.Ability.EntityId.ToString() && s.Enabled);

            if (abilitySetting != null)
            {
                if (abilitySetting.Image != null && File.Exists(abilitySetting.Image))
                {
                    viewModel.ImageUrl = abilitySetting.Image;
                }

                if (!string.IsNullOrEmpty(abilitySetting.BorderColor))
                {
                    viewModel.ImageBorderColor = abilitySetting.BorderColor.FromHexToColor();
                }

                if (abilitySetting.Aliases.Any())
                {
                    viewModel.Text = abilitySetting.Aliases.PickRandom();
                }
            }
        }
    }
}