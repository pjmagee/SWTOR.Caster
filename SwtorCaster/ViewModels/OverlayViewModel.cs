namespace SwtorCaster.ViewModels
{
    using System;
    using Screens;
    using System.Windows;
    using System.Windows.Input;
    using Core.Services.Settings;
    using Caliburn.Micro;
    using Core.Domain.Settings;

    public class OverlayViewModel : FocusableScreen, IHandle<Settings>
    {
        private readonly ISettingsService _settingsService;

        public double Opacity => _settingsService.Settings.Opacity;

        public AbilityListViewModel AbilityListViewModel { get; }

        public override string DisplayName { get; set; } = "SWTOR Caster - Ability Logger";

        public OverlayViewModel(ISettingsService settingsService, AbilityListViewModel abilityListViewModel, IEventAggregator aggregator)
        {
            _settingsService = settingsService;
            AbilityListViewModel = abilityListViewModel;
            aggregator.Subscribe(this);
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
            Refresh();
        }
    }
}