namespace SwtorCaster.Core.Services.Factory
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Domain.Log;
    using Domain.Settings;
    using Extensions;
    using Images;
    using Settings;
    using ViewModels;

    public class CombatLogViewModelFactory : ICombatLogViewModelFactory
    {
        private readonly Random _random = new Random();
        private readonly ISettingsService _settingsService;
        private readonly IImageService _imageService;

        public CombatLogViewModelFactory(ISettingsService settingsService, IImageService imageService)
        {
            _settingsService = settingsService;
            _imageService = imageService;
        }

        public CombatLogViewModel Create(CombatLogEvent @event)
        {
            var viewModel = new CombatLogViewModel(@event);
            var settings = _settingsService.Settings;

            if (@event.IsAbilityActivate() || @event.IsApplyEffect())
            {
                ApplyLoggerSettings(@event, viewModel, settings);
                ApplyAbilitySettings(@event, viewModel, settings);
            }

            if (@event.IsPlayerCompanion() && settings.EnableCompanionAbilities)
            {
                viewModel.ImageBorderColor = settings.CompanionAbilityBorderColor.FromHexToColor();
            }

            ApplyTextSettings(viewModel, settings);

            return viewModel;
        }

        private void ApplyTextSettings(CombatLogViewModel viewModel, Settings settings)
        {
            viewModel.FontColor = new SolidColorBrush(settings.AbilityTextColor.FromHexToColor());
            viewModel.FontFamily = settings.TextFont.FromStringToFont();
            viewModel.FontSize = settings.FontSize;
        }

        private void ApplyLoggerSettings(CombatLogEvent combatLogEvent, CombatLogViewModel viewModel, Settings settings)
        {
            viewModel.ImageUrl = _imageService.GetImageById(combatLogEvent.Ability.EntityId);
            viewModel.ImageBorderColor = Colors.Transparent;
            viewModel.ImageAngle = _random.Next(-settings.Rotate, settings.Rotate);

            viewModel.Text = combatLogEvent.Ability.DisplayName;
            viewModel.TextVisibility = settings.EnableAbilityText ? Visibility.Visible : Visibility.Hidden;
            viewModel.TooltipText = $"{combatLogEvent.Ability.EntityId} (Click to copy ability id to clipboard)";
        }

        private static void ApplyAbilitySettings(CombatLogEvent @event, CombatLogViewModel viewModel, Settings settings)
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
                    viewModel.Text = abilitySetting.Aliases.Concat(new[] { viewModel.Text }).ToList().PickRandom();
                }
            }
        }
    }
}