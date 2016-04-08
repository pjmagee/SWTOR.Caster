namespace SwtorCaster.Core.Services.Events
{
    using System;
    using System.Linq;
    using Audio;
    using Domain.Log;
    using Domain.Settings;
    using Logging;
    using Settings;
    using Extensions;

    public class EventService : IEventService
    {
        private readonly IAudioService _audioService;
        private readonly ILoggerService _loggerService;
        private readonly ISettingsService _settingsService;

        public EventService(ISettingsService settingsService, IAudioService audioService, ILoggerService loggerService)
        {
            _settingsService = settingsService;
            _audioService = audioService;
            _loggerService = loggerService;
        }

        public void Handle(CombatLogEvent line)
        {
            if (!_settingsService.Settings.EnableSound) return;

            foreach (var eventSetting in _settingsService.Settings.EventSettings.Where(x => x.Enabled))
            {
                HandleEventLine(line, eventSetting);
            }
        }

        private void HandleEventLine(CombatLogEvent line, EventSetting setting)
        {
            try
            {
                if (setting.CanPlay(line) && !string.IsNullOrEmpty(setting.Sound))
                {
                    _audioService.Play(setting.Sound);
                }

                if (setting.CanPlay(line) && string.IsNullOrEmpty(setting.Sound))
                {
                    _audioService.Stop();
                }
            }
            catch (Exception e)
            {
                _loggerService.Log(e.Message);
            }
        }
    }
}