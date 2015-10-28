namespace SwtorCaster.Core.Services.Providers
{
    using Caliburn.Micro;
    using Combat;
    using Domain;
    using Settings;

    public class CombatLogProvider : ICombatLogProvider, IHandle<Settings>
    {
        private readonly ISettingsService _settingsService;
        private readonly IEventAggregator _eventAggregator;

        public CombatLogProvider(ISettingsService settingsService, IEventAggregator eventAggregator)
        {
            _settingsService = settingsService;
            _eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }

        public void Handle(Settings message)
        {
            if (!message.EnableDemoMode)
            {
                var demoService = IoC.Get<ICombatLogService>("DemoParser");
                demoService.Stop();
            }

            _eventAggregator.PublishOnCurrentThread(message.EnableDemoMode
                ? IoC.Get<ICombatLogService>("DemoParser")
                : IoC.Get<ICombatLogService>("CombatLogParser"));
        }

        public ICombatLogService GetCombatLogService()
        {
            return _settingsService.Settings.EnableDemoMode
                ? IoC.Get<ICombatLogService>("DemoParser")
                : IoC.Get<ICombatLogService>("CombatLogParser");
        }
    }
}