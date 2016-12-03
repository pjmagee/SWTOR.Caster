namespace SwtorCaster.Core.Services.Combat
{
    using Caliburn.Micro;
    using Domain;
    using Domain.Log;
    using Events;
    using Factory;
    using Parsing;
    using Settings;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Windows;

    public class PlayBackLogService : ICombatLogService
    {
        private readonly ICombatLogViewModelFactory _logViewModelFactory;
        private readonly ICombatLogParser _logParser;
        private readonly ISettingsService _settingsService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IEventService _eventService;

        private Thread _parserThread;

        public PlayBackLogService(
            ISettingsService settingsService,
            IEventAggregator eventAggregator,
            IEventService eventService,
            ICombatLogViewModelFactory logViewModelFactory,
            ICombatLogParser logParser)
        {
            _settingsService = settingsService;
            _eventAggregator = eventAggregator;
            _eventService = eventService;
            _logViewModelFactory = logViewModelFactory;
            _logParser = logParser;
        }


        public bool IsRunning { get; private set; }

        public void Start()
        {
            if (_parserThread != null) return;
            _parserThread = new Thread(Run);
            _parserThread.Start();
        }

        private void Run()
        {
            IsRunning = File.Exists(_settingsService.Settings.CombatLogFile);
            Events.Clear();

            if (IsRunning)
            {
                var lines = File.ReadAllLines(_settingsService.Settings.CombatLogFile);
                
                foreach(var line in lines)
                {
                    AddLine(line);
                }

                PlayBack();     
            }
        }

        private void PlayBack()
        {
            var temp = Events.Dequeue();
            _eventService.Handle(temp);

            while (Events.Count > 0)
            {              
                Application.Current.Dispatcher.Invoke(() => Render(temp));

                var next = Events.Dequeue();
                var pause = next.TimeStamp.Subtract(temp.TimeStamp);
                temp = next;
                Thread.Sleep(pause);
            }
        }

        private void Render(CombatLogEvent combatLogEvent)
        {
            var logLine = _logViewModelFactory.Create(combatLogEvent);
            _eventAggregator.PublishOnUIThread(logLine);
        }

        private Queue<CombatLogEvent> Events { get; set; } = new Queue<CombatLogEvent>();

        private void AddLine(string line)
        {
            var combatLogEvent = _logParser.Parse(line);

            if (combatLogEvent.IsAbilityActivate())
            {
                Events.Enqueue(combatLogEvent);
            }            
        }

        public void Stop()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this._eventAggregator.PublishOnUIThread(new ParserMessage() { ClearLog = true });
            });

            IsRunning = false;
            if (_parserThread == null) return;
            _parserThread.Abort();
            _parserThread = null;
        }
    }
}
