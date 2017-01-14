namespace SwtorCaster.Core.Services.Providers
{
    using Caliburn.Micro;
    using Combat;
    using Domain.Settings;
    using Settings;

    public class CombatLogProvider : ICombatLogProvider, IHandle<AppSettings>
    {
        private readonly ISettingsService _settingsService;
        private readonly IEventAggregator _eventAggregator;

        public CombatLogProvider(ISettingsService settingsService, IEventAggregator eventAggregator)
        {
            _settingsService = settingsService;
            _eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }

        private ICombatLogService PlayBackService => IoC.Get<ICombatLogService>("PlayBack");
        private ICombatLogService RealtimeService => IoC.Get<ICombatLogService>("RealTime");

        public void Handle(AppSettings message)
        {
            if(message.EnablePlaybackMode)
            {
                RealtimeService.Stop();
            }
            else
            {
                PlayBackService.Stop();
            }

            _eventAggregator.PublishOnCurrentThread(message.EnablePlaybackMode ? PlayBackService : RealtimeService);
        }

        public ICombatLogService GetCombatLogService()
        {
            return _settingsService.Settings.EnablePlaybackMode ? PlayBackService : RealtimeService;
        }
    }
}