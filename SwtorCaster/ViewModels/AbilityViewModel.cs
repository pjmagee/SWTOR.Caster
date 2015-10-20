using SwtorCaster.Core;

namespace SwtorCaster.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using Core.Parser;
    using Core.Services;
    using System.ComponentModel;
    using System.Windows.Media;
    using SwtorCaster.Core.Domain;
    using Screens;

    public class AbilityViewModel : FocusableScreen
    {
        private readonly IParserService _parserService;
        private readonly ILoggerService _loggerService;
        private readonly ISettingsService _settingsService;

        public override string DisplayName { get; set; } = "SWTOR Caster - Abilities";

        public SolidColorBrush BackgroundColor
        {
            get
            {
                return new SolidColorBrush(_settingsService.Settings.AbilityLoggerBackgroundColor.ToColorFromRgb());
            }
        }

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
            if (_settingsService.Settings.EnableCombatClear && e.EventDetailType == EventDetailType.ExitCombat)
            {
                Application.Current.Dispatcher.Invoke(() => LogLines.Clear());
                return;
            }

            if (e.Action.Trim() == string.Empty)
            {
                _loggerService.Log($"{e.Id} was empty.");
            };

            Application.Current.Dispatcher.Invoke(() => AddItem(e));
        }

        private void AddItem(LogLineEventArgs item)
        {
            if (item.EventDetailType != EventDetailType.AbilityActivate || item.EventType != EventType.Event) return;
            if (LogLines.Count == _settingsService.Settings.Items) LogLines.RemoveAt(LogLines.Count - 1);
            LogLines.Insert(0, item);
        }

        public void CopyToClipBoard(string id)
        {
            Clipboard.SetText(id, TextDataFormat.Text);
        }

        protected override void OnActivate()
        {
            if (_parserService.IsRunning) return;
            _parserService.Clear += ParserServiceOnClear;
            _parserService.ItemAdded += ParserServiceOnItemAdded;
            _parserService.Start();
            _settingsService.Settings.PropertyChanged += SettingsOnPropertyChanged;

            _loggerService.Log($"Parser service started");
        }

        private void SettingsOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Refresh();
        }

        protected override void OnDeactivate(bool close)
        {
            _parserService.Clear -= ParserServiceOnClear;
            _parserService.ItemAdded -= ParserServiceOnItemAdded;
            _parserService.Stop();

            _loggerService.Log($"Parser service stopped");
        }
    }
}