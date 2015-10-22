namespace SwtorCaster.Core.Services
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using SwtorCaster.Core.Domain;
    using SwtorCaster.Core.Parser;
    using System.IO;
    using System.Windows.Media;

    public class LogLineEventArgFactory : ILogLineEventArgFactory
    {
        private readonly Random _random = new Random();
        private readonly Regex _regex = new Regex(@"\[(.*)\] \[(.*)\] \[(.*)\] \[((.*)\s{(\d*)}?)?\] \[(.*)\s{(\d*)}:\s(.*)\s{(\d*)}\]", RegexOptions.Compiled);

        private readonly IImageService _imageService;
        private readonly ISettingsService _settingsService;

        public LogLineEventArgFactory(IImageService imageService, ISettingsService settingsService)
        {
            _imageService = imageService;
            _settingsService = settingsService;
        }

        public LogLineEventArgs Create(string line)
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
                var eventType = match.Groups[7].Value;
                var abilityName = match.Groups[5].Value;
                var eventDetail = match.Groups[9].Value;
                var border = Colors.Transparent;

                if (abilitySetting != null)
                {
                    if (abilitySetting.Image != null && File.Exists(abilitySetting.Image))
                        imageUrl = abilitySetting.Image;

                    if (!string.IsNullOrEmpty(abilitySetting.BorderColor))
                        border = abilitySetting.BorderColor.ToColorFromRgb();
                }

                EventDetailType detailType;
                Enum.TryParse(eventDetail, ignoreCase: true, result: out detailType);
                
                var logLineEventArgs = new LogLineEventArgs
                {
                    Id = id,
                    Action = abilityName,
                    ActionVisibility = enableAbilityName,
                    ImageUrl = imageUrl,
                    Angle = angle,
                    EventType = (EventType)Enum.Parse(typeof(EventType), eventType, ignoreCase: true),
                    EventDetailType = detailType,
                    ImageBorderColor = new SolidColorBrush(border)
                };

                return logLineEventArgs;
            }

            // Bad
            return null;
        }
    }
}