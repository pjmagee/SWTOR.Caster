namespace SwtorCaster.Core.Factories
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Media;
    using Domain;
    using Services.Images;
    using Services.Settings;
    using SwtorCaster.Core.Extensions;

    public class LogLineFactory : ILogLineFactory
    {
        private readonly Random _random = new Random();
        private readonly Regex _regex = new Regex(@"\[(.*)\] \[(.*)\] \[(.*)\] \[((.*)\s{(\d*)}?)?\] \[(.*)\s{(\d*)}:\s(.*)\s{(\d*)}\]", RegexOptions.Compiled);

        private readonly IImageService _imageService;
        private readonly ISettingsService _settingsService;

        private string _currentPlayer;

        private void UpdatePlayer(EventDetailType type, string source)
        {
            if (type == EventDetailType.AbilityActivate)
            {
                _currentPlayer = source;
            }
        }

        public LogLineFactory(IImageService imageService, ISettingsService settingsService)
        {
            _imageService = imageService;
            _settingsService = settingsService;
        }

        public LogLine Create(string line)
        {
            var match = _regex.Match(line);

            var settings = _settingsService.Settings;

            if (match.Success)
            {
                var id = match.Groups[6].Value;
                var abilitySetting = settings.AbilitySettings.FirstOrDefault(s => s.AbilityId == id && s.Enabled);
                var imageUrl = _imageService.GetImageById(id);
                var angle = _random.Next(-settings.Rotate, settings.Rotate);
                var enableAbilityName = settings.EnableAbilityText ? Visibility.Visible : Visibility.Hidden;
                var source = match.Groups[2].Value;
                var eventType = match.Groups[7].Value;
                var abilityName = match.Groups[5].Value;
                var eventDetail = match.Groups[9].Value;
                var border = Colors.Transparent;

                if (abilitySetting != null)
                {
                    if (abilitySetting.Image != null && File.Exists(abilitySetting.Image))
                        imageUrl = abilitySetting.Image;

                    if (!string.IsNullOrEmpty(abilitySetting.BorderColor))
                        border = abilitySetting.BorderColor.FromHexToColor();

                    if (abilitySetting.Aliases.Any())
                        abilityName = abilitySetting.Aliases.Concat(new[] { abilityName }).ToList()[_random.Next(0, abilitySetting.Aliases.Count)];
                }

                EventDetailType detailType;
                Enum.TryParse(eventDetail, true, out detailType);
                SourceTargetType targetType = SourceTargetType.Self;

                if (detailType == EventDetailType.AbilityActivate)
                {
                    targetType = source.Contains(":") ? SourceTargetType.Companion : SourceTargetType.Self;
                }

                UpdatePlayer(detailType, source);

                var logLine = new LogLine
                {
                    Id = id,
                    Action = abilityName,
                    ActionVisibility = enableAbilityName,
                    ImageUrl = imageUrl,
                    Angle = angle,
                    SourceType = targetType,
                    EventType = (EventType)Enum.Parse(typeof(EventType), eventType, true),
                    EventDetailType = detailType,
                    ImageBorderColor = new SolidColorBrush(border),
                    IsUnknown = _imageService.IsUnknown(id),
                    IsCurrentPlayer = _currentPlayer == source
                };

                return logLine;
            }

            // Bad
            return null;
        }
    }
}