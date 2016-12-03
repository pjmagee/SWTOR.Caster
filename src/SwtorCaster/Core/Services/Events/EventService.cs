namespace SwtorCaster.Core.Services.Events
{
    using System;
    using System.Linq;
    using Audio;
    using Domain.Log;
    using Domain.Settings;
    using Logging;
    using Settings;    

    public class EventService : IEventService
    {
        private readonly IAudioService audioService;
        private readonly ILoggerService loggerService;
        private readonly ISettingsService settingsService;

        public EventService(ISettingsService settingsService, IAudioService audioService, ILoggerService loggerService)
        {
            this.settingsService = settingsService;
            this.audioService = audioService;
            this.loggerService = loggerService;
        }

        public void Handle(CombatLogEvent line)
        {
            var settings = settingsService.Settings;

            if (!settings.EnableSound) return;

            foreach (var eventSetting in settings.EventSettings.Where(x => x.Enabled))
            {
                HandleEventLine(line, eventSetting);
            }
        }

        private void HandleEventLine(CombatLogEvent line, EventSetting setting)
        {
            try
            {
                var canPlay = setting.CanPlay(line);
                var hasSoundFile = !string.IsNullOrEmpty(setting.Sound);

                if (canPlay && hasSoundFile)
                {
                    audioService.Play(setting.Sound);
                }
                else if (canPlay && !hasSoundFile)
                {
                    audioService.Stop();
                }
            }
            catch (Exception e)
            {
                loggerService.Log(e.Message);
            }
        }
    }
}