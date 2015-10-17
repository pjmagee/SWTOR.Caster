namespace SwtorCaster.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using Caliburn.Micro;
    using Core.Parser;
    using Core.Services;

    public class AbilityViewModel : Screen
    {
        private readonly IParserService _parserService;
        private readonly ILoggerService _loggerService;
        private readonly ISettingsService _settingsService;

        public override string DisplayName { get; set; } = "SWTOR Caster - Abilities";

        public ObservableCollection<LogLineEventArgs> LogLines { get; } = new ObservableCollection<LogLineEventArgs>();

        public AbilityViewModel(
            IParserService parserService, 
            ILoggerService loggerService,
            ISettingsService settingsService)
        {
            _parserService = parserService;
            _loggerService = loggerService;
            _settingsService = settingsService;
        }

        private void ParserServiceOnClear(object sender, EventArgs eventArgs)
        {
            Application.Current.Dispatcher.Invoke(() => LogLines.Clear());
        }

        private void ParserServiceOnItemAdded(object sender, LogLineEventArgs e)
        {
            if (_settingsService.Settings.EnableCombatClear && e.EventDetail == "ExitCombat")
            {
                Application.Current.Dispatcher.Invoke(() => LogLines.Clear());
                return;
            }

            if (e.Ability.Trim() == string.Empty)
            {
                _loggerService.Log($"{e.Id} was empty.");
            };

            Application.Current.Dispatcher.Invoke(() => AddItem(e));
        }

        private void AddItem(LogLineEventArgs item)
        {
            if (item.EventDetail != "AbilityActivate" || item.EventType != "Event") return;
            if (LogLines.Count == _settingsService.Settings.Items) LogLines.RemoveAt(LogLines.Count - 1);
            LogLines.Insert(0, item);
        }

        protected override void OnActivate()
        {
            _parserService.Clear -= ParserServiceOnClear;
            _parserService.ItemAdded -= ParserServiceOnItemAdded;

            _parserService.Clear += ParserServiceOnClear;
            _parserService.ItemAdded += ParserServiceOnItemAdded;


            if (_parserService.IsRunning) return;
            _parserService.Start();
        }

        protected override void OnDeactivate(bool close)
        {
            _parserService.Clear -= ParserServiceOnClear;
            _parserService.ItemAdded -= ParserServiceOnItemAdded;
            _parserService.Stop();
        }
    }
}