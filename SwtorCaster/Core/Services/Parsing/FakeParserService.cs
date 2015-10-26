namespace SwtorCaster.Core.Services.Parsing
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using Caliburn.Micro;
    using Domain;
    using Images;
    using Settings;

    public class FakeParserService : IParserService
    {
        private readonly IImageService _imageService;
        private readonly ISettingsService _settingsService;
        private readonly IEventAggregator _eventAggregator;
        public bool IsRunning => _parserThread != null && _parserThread.ThreadState == ThreadState.Running;
        private Thread _parserThread;

        public FakeParserService(IImageService imageService, ISettingsService settingsService, IEventAggregator eventAggregator)
        {
            _imageService = imageService;
            _settingsService = settingsService;
            _eventAggregator = eventAggregator;
        }

        public void Start()
        {
            if (_parserThread != null) return;
            _parserThread = new Thread(Run);
            _parserThread.Start();
        }

        private void Run()
        {
            var images = _imageService.GetImages().ToList();
            var random = new Random();

            var types = new[]
            {
                EventDetailType.AbilityActivate, EventDetailType.Death, EventDetailType.ExitCombat,
                EventDetailType.EnterCombat
            };
      
            while (true)
            {
                var image = images[random.Next(0, images.Count)];
                var id = Path.GetFileNameWithoutExtension(image);
                var color = Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                var settings = _settingsService.Settings;
                
                var logline = new LogLine(id, SourceTargetType.Self, SourceTargetType.Other, EventType.Event,
                    types[random.Next(0, types.Length)],
                    "ACTION",
                    _imageService.GetImageById(id),
                    random.Next(-settings.Rotate, settings.Rotate),
                    Visibility.Visible,
                    color, false, true);

                _eventAggregator.PublishOnUIThread(logline);

                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            if (_parserThread == null) return;
            _parserThread.Abort();
            _parserThread = null;
        }
    }
}