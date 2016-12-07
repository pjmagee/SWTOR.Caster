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
        private readonly ICombatLogViewModelFactory logViewModelFactory;
        private readonly ICombatLogParser logParser;
        private readonly ISettingsService settingsService;
        private readonly IEventAggregator eventAggregator;
        private readonly IEventService eventService;

        private Thread parserThread;

        public PlayBackLogService(
            ISettingsService settingsService,
            IEventAggregator eventAggregator,
            IEventService eventService,
            ICombatLogViewModelFactory logViewModelFactory,
            ICombatLogParser logParser)
        {
            this.settingsService = settingsService;
            this.eventAggregator = eventAggregator;
            this.eventService = eventService;
            this.logViewModelFactory = logViewModelFactory;
            this.logParser = logParser;
        }


        public bool IsRunning { get; private set; }

        public void Start()
        {
            if (parserThread != null) return;
            parserThread = new Thread(Run);
            parserThread.Start();
        }

        private void Run()
        {
            IsRunning = File.Exists(settingsService.Settings.CombatLogFile);
            Events.Clear();

            if (IsRunning)
            {
                var lines = File.ReadAllLines(settingsService.Settings.CombatLogFile);
                
                foreach(var line in lines)
                {
                    AddLine(line);
                }

                PlayBack();     
            }
        }

        private void PlayBack()
        {
            try
            {
                var temp = Events.Dequeue();
                eventService.Handle(temp);

                while (Events.Count > 0)
                {
                    try
                    {
                        Application.Current.Dispatcher.Invoke(() => Render(temp));

                        var next = Events.Dequeue();
                        var pause = next.TimeStamp.Subtract(temp.TimeStamp);
                        temp = next;
                        Thread.Sleep(pause);
                    }
                    catch (Exception e)
                    {
                        
                    }
                }
            }
            catch (Exception e)
            {
                
            }
        }

        private void Render(CombatLogEvent combatLogEvent)
        {
            var logLine = logViewModelFactory.Create(combatLogEvent);
            eventAggregator.PublishOnUIThread(logLine);
        }

        private Queue<CombatLogEvent> Events { get; set; } = new Queue<CombatLogEvent>();

        private void AddLine(string line)
        {
            var combatLogEvent = logParser.Parse(line);

            if (combatLogEvent.IsAbilityActivate())
            {
                Events.Enqueue(combatLogEvent);
            }            
        }

        public void Stop()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.eventAggregator.PublishOnUIThread(new ParserMessage() { ClearLog = true });
            });

            IsRunning = false;
            if (parserThread == null) return;
            parserThread.Abort();
            parserThread = null;
        }
    }
}
