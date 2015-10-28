namespace SwtorCaster.Core.Services.Combat
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using Caliburn.Micro;
    using Domain;
    using Events;
    using Extensions;
    using Images;
    using Settings;

    public class DemoCombatLogService : ICombatLogService
    {
        private readonly IImageService _imageService;
        private readonly ISettingsService _settingsService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IEventService _eventService;

        public bool IsRunning { get; private set; }
        private Thread _parserThread;

        public DemoCombatLogService(
            IImageService imageService,
            ISettingsService settingsService, 
            IEventAggregator eventAggregator, 
            IEventService eventService)
        {
            _imageService = imageService;
            _settingsService = settingsService;
            _eventAggregator = eventAggregator;
            _eventService = eventService;
        }

        public void Start()
        {
            if (_parserThread != null) return;
            _parserThread = new Thread(Run);
            _parserThread.Start();
        }

        private void Run()
        {
            IsRunning = true;
            var images = _imageService.GetImages().ToList();
            var random = new Random();
            var gcd = 1000;

            while (IsRunning)
            {
                var randomImage = images[random.Next(0, images.Count)];
                var id = Path.GetFileNameWithoutExtension(randomImage);
                var image = _imageService.GetImageById(id);
                var isColor = random.Next(0, 2) > 1;
                var color = isColor ? Enum.GetValues(typeof (Colors)).Cast<Color>().PickRandom() : Colors.Transparent;
                var settings = _settingsService.Settings;
                var isPlayer = random.Next(0, 2) > 1;
                var rotate = random.Next(-settings.Rotate, settings.Rotate);
                var type = random.Next(0, 100) < 25 ? EventDetailType.Death : EventDetailType.AbilityActivate;
                var source = SourceTargetType.Self;
                var target = SourceTargetType.Other;
                var eventType = EventType.Event;
                var isForced = false;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    var logline = new LogLine(id, source, target, eventType, type, id, image, rotate, Visibility.Visible, color, false, isPlayer, isForced);
                    _eventAggregator.PublishOnUIThread(logline);
                    _eventService.Handle(logline);
                });

                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            IsRunning = false;
            if (_parserThread == null) return;
            _parserThread.Abort();
            _parserThread = null;
        }
    }
}