namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;
    using Core.Domain.Settings;
    using Core.Services.Guide;
    using Core.Services.Settings;
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class GuideOverlayViewModel : FocusableScreen, IHandle<Settings>
    {
        private readonly ISettingsService _settingsService;
        private readonly IRotationService _rotationService;

        public double Opacity => _settingsService.Settings.Opacity;

        public RotationViewModel RotationViewModel { get; set; }
        
        public GuideOverlayViewModel(IEventAggregator aggregator, ISettingsService settingsService, IRotationService rotationService)
        {
            _settingsService = settingsService;
            _rotationService = rotationService;
            aggregator.Subscribe(this);
            Initialize();
        }

        private void Initialize()
        {
            if (string.IsNullOrWhiteSpace(_settingsService.Settings.GuideFile)) return;
            RotationViewModel = _rotationService.GetRotation(_settingsService.Settings.GuideFile);
        }

        public void OnLeftMouseButtonDown(MouseButtonEventArgs e)
        {
            var window = this.GetView() as Window;
            window?.DragMove();
        }

        public void Close()
        {
            var window = GetView() as Window;
            window?.Close();
        }

        public void IncreaseOpacity()
        {
            _settingsService.Settings.Opacity = Math.Round(_settingsService.Settings.Opacity + 0.025, 2);
        }

        public void DecreaseOpacity()
        {
            _settingsService.Settings.Opacity = Math.Round(_settingsService.Settings.Opacity - 0.025, 2);
        }

        protected override void OnActivate()
        {
            var window = GetView() as Window;
            window.Topmost = true;
        }

        protected override void OnDeactivate(bool close)
        {
            if (!close)
            {
                var window = GetView() as Window;
                window.Topmost = true;
            }
        }

        public void Handle(Settings message)
        {
            Initialize();
            Refresh();
        }
    }
}