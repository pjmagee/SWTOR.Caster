using System;
using System.Text.RegularExpressions;
using System.Windows;
using SwtorCaster.Core.Domain;
using SwtorCaster.Core.Parser;

namespace SwtorCaster.Core.Services
{
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

            if (match.Success)
            {
                var id = match.Groups[6].Value;
                var imageUrl = _imageService.GetImageById(id);
                var angle = _random.Next(-_settingsService.Settings.Rotate, _settingsService.Settings.Rotate);
                var enableAbilityName = _settingsService.Settings.EnableAbilityText ? Visibility.Visible : Visibility.Hidden;
                var eventType = match.Groups[7].Value;
                var abilityName = match.Groups[5].Value;
                var eventDetail = match.Groups[9].Value;

                var logLineEventArgs = new LogLineEventArgs
                {
                    Id = id,
                    Action = abilityName,
                    ActionVisibility = enableAbilityName,
                    ImageUrl = imageUrl,
                    Angle = angle,
                    EventType = (EventType) Enum.Parse(typeof (EventType), eventType, ignoreCase: true),
                    EventDetailType = (EventDetailType) Enum.Parse(typeof (EventDetailType), eventDetail, ignoreCase: true)
                };

                return logLineEventArgs;
            }

            // Bad
            return null;
        }
    }
}