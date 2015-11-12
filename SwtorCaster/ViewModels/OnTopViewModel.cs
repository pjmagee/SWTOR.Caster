namespace SwtorCaster.ViewModels
{
    using System.Windows;
    using System.Windows.Input;
    using Caliburn.Micro;
    using Core.Services.Settings;

    public class OnTopViewModel : AbilityViewModel
    {
        private readonly ISettingsService _settingsService;

        public double Opacity => _settingsService.Settings.Opacity;

        public OnTopViewModel(ISettingsService settingsService, IEventAggregator eventAggregator) : base(settingsService, eventAggregator)
        {
            _settingsService = settingsService;
        }

        public void OnLeftMouseButtonDown(MouseButtonEventArgs e)
        {
            var window = this.GetView() as Window;
            window?.DragMove();
        }

        public void Close()
        {
            var window = this.GetView() as Window;
            window?.Close();
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
    }
}