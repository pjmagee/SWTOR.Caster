namespace SwtorCaster.Core.Services.Parsing
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Media;
    using Domain;
    using Extensions;
    using Images;
    using Settings;

    public class LogLineParser : ILogLineParser
    {
        private readonly Random _random = new Random();
        private readonly Regex _regex = new Regex(@"\[(.*)\] \[(.*)\] \[(.*)\] \[((.*)\s{(\d*)}?)?\] \[(.*)\s{(\d*)}:\s(.*)\s{(\d*)}\]", RegexOptions.Compiled);

        private readonly IImageService _imageService;
        private readonly ISettingsService _settingsService;

        private string _currentPlayer;

        private void UpdatePlayer(string source)
        {
            _currentPlayer = source;
        }

        public LogLineParser(IImageService imageService, ISettingsService settingsService)
        {
            _imageService = imageService;
            _settingsService = settingsService;
        }

        public LogLine Parse(string line)
        {
            var match = _regex.Match(line);

            var settings = _settingsService.Settings;

            if (match.Success)
            {
                var source = match.Groups[2].Value;
                var target = match.Groups[3].Value;
                var abilityName = match.Groups[5].Value;
                var id = match.Groups[6].Value;
                var eventType = match.Groups[7].Value;
                var eventDetail = match.Groups[9].Value;

                var abilitySetting = settings.AbilitySettings.FirstOrDefault(s => s.AbilityId == id && s.Enabled);
                var imageUrl = _imageService.GetImageById(id);
                var angle = _random.Next(-settings.Rotate, settings.Rotate);
                var actionVisibility = settings.EnableAbilityText ? Visibility.Visible : Visibility.Hidden;
                var border = Colors.Transparent;

                if (abilitySetting != null)
                {
                    if (abilitySetting.Image != null && File.Exists(abilitySetting.Image))
                    {
                        imageUrl = abilitySetting.Image;
                    }

                    if (!string.IsNullOrEmpty(abilitySetting.BorderColor))
                    {
                        border = abilitySetting.BorderColor.FromHexToColor();
                    }

                    if (abilitySetting.Aliases.Any())
                    {
                        abilityName = abilitySetting.Aliases.Concat(new[] {abilityName}).ToList().PickRandom();
                    }
                }

                EventDetailType eventDetailType;
                Enum.TryParse(eventDetail, true, out eventDetailType);
                SourceTargetType targetType = SourceTargetType.Self;

                if (eventDetailType == EventDetailType.AbilityActivate)
                {
                    targetType = source.Contains(":") ? SourceTargetType.Companion : SourceTargetType.Self;
                }

                if (eventDetailType == EventDetailType.AbilityActivate)
                {
                    UpdatePlayer(source);
                }

                

                var logLine = new LogLine
                {
                    Id = id,
                    Action = abilityName,
                    ActionVisibility = actionVisibility,
                    ImageUrl = imageUrl,
                    Angle = angle,
                    SourceType = targetType,
                    EventType = (EventType)Enum.Parse(typeof(EventType), eventType, true),
                    EventDetailType = eventDetailType,
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