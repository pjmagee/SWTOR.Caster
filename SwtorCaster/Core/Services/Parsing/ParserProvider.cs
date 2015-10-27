namespace SwtorCaster.Core.Services.Parsing
{
    using Caliburn.Micro;
    using Settings;
    using Domain;

    public class ParserProvider : IParserProvider, IHandle<Settings>
    {
        private readonly ISettingsService _settingsService;
        private readonly IEventAggregator _eventAggregator;

        public ParserProvider(ISettingsService settingsService, IEventAggregator eventAggregator)
        {
            _settingsService = settingsService;
            _eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }

        public void Handle(Settings message)
        {
            if (!message.EnableDemoMode)
            {
                var demoService = IoC.Get<IParserService>("DemoParser");
                demoService.Stop();
            }

            _eventAggregator.PublishOnCurrentThread(message.EnableDemoMode
                ? IoC.Get<IParserService>("DemoParser")
                : IoC.Get<IParserService>("CombatLogParser"));
        }

        public IParserService GetParserService()
        {
            return _settingsService.Settings.EnableDemoMode
                ? IoC.Get<IParserService>("DemoParser")
                : IoC.Get<IParserService>("CombatLogParser");
        }
    }
}