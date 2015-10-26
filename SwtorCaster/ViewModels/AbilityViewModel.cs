
namespace SwtorCaster.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media;
    using Caliburn.Micro;
    using Core.Domain;
    using Screens;
    using Core;
    using Core.Services.Logging;
    using Core.Services.Parsing;
    using Core.Services.Settings;

    public class AbilityViewModel : FocusableScreen, IHandle<Settings>, IHandle<ParserMessage>, IHandle<LogLine>
    {
        private readonly IParserService _parserService;
        private readonly ILoggerService _loggerService;
        private readonly ISettingsService _settingsService;

        public override string DisplayName { get; set; } = "SWTOR Caster - Abilities";

        public SolidColorBrush BackgroundColor => new SolidColorBrush(_settingsService.Settings.AbilityLoggerBackgroundColor.ToColorFromRgb());

        // public double FontSize => _settingsService.Settings.FontSize;

        public ObservableCollection<LogLine> LogLines { get; } = new ObservableCollection<LogLine>();

        public AbilityViewModel(
            IParserService parserService, 
            ILoggerService loggerService,
            ISettingsService settingsService, 
            IEventAggregator eventAggregator)
        {
            _parserService = parserService;
            _loggerService = loggerService;
            _settingsService = settingsService;
            eventAggregator.Subscribe(this);
        }

        private void TryAddItem(LogLine item)
        {
            if (LogLines.Count == _settingsService.Settings.Items) LogLines.RemoveAt(LogLines.Count - 1);
            LogLines.Insert(0, item);
        }

        public void CopyToClipBoard(LogLine e)
        {
            Clipboard.SetText(e.Id, TextDataFormat.Text);
        }

        protected override void OnActivate()
        {
            if (_parserService.IsRunning) return;
            _parserService.Start();
            _loggerService.Log($"Parser service started");
        }

        protected override void OnDeactivate(bool close)
        {
            _parserService.Stop();
            _loggerService.Log($"Parser service stopped");
        }

        public void Handle(Settings message)
        {
            Refresh();
        }

        public void Handle(ParserMessage message)
        {
            if (message.ClearLog)
            {
                LogLines.Clear();
            }
        }

        public void Handle(LogLine message)
        {
            if (message == null) return;

            var settings = _settingsService.Settings;

            if (settings.EnableCombatClear && message.EventDetailType == EventDetailType.ExitCombat) LogLines.Clear();
            if (!settings.EnableCompanionAbilities && message.SourceType == SourceTargetType.Companion) return;
            if (settings.IgnoreUnknownAbilities && message.IsUnknown) return;
            if (message.EventDetailType != EventDetailType.AbilityActivate || message.EventType != EventType.Event) return;
            if (message.IsEmpty) _loggerService.Log($"{message.Id} was empty.");

            TryAddItem(message);
        }
    }
}