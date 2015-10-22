namespace SwtorCaster.Core.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using SwtorCaster.Core.Domain;
    using SwtorCaster.Core.Parser;


    public class FakeParserService : IParserService
    {
        private readonly IImageService _imageService;
        private readonly ISettingsService _settingsService;
        public bool IsRunning => _parserThread != null && _parserThread.ThreadState == ThreadState.Running;

        public event EventHandler Clear;
        public event EventHandler<LogLineEventArgs> ItemAdded;

        private Thread _parserThread;

        public FakeParserService(IImageService imageService, ISettingsService settingsService)
        {
            _imageService = imageService;
            _settingsService = settingsService;
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

            while (true)
            {
                var image = images[random.Next(0, images.Count)];
                var id = Path.GetFileNameWithoutExtension(image);
                var color = Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                var settings = _settingsService.Settings;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ItemAdded?.Invoke(this, new LogLineEventArgs(
                        id,
                        SourceTargetType.Self,
                        SourceTargetType.Other,
                        EventType.Event,
                        EventDetailType.AbilityActivate,
                        "ACTION",
                        _imageService.GetImageById(id),
                        random.Next(-settings.Rotate, settings.Rotate),
                        Visibility.Visible,
                        color));
                });

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