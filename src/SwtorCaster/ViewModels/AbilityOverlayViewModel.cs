namespace SwtorCaster.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using Core.Services.Settings;
    using Caliburn.Micro;
    using Core.Domain.Settings;

    public class AbilityOverlayViewModel : FocusableScreen, IHandle<Settings>
    {
        private readonly ISettingsService settingsService;

        public double Opacity => settingsService.Settings.Opacity;

        public AbilityViewModel AbilityViewModel { get; }

        public override string DisplayName { get; set; } = "SWTOR Caster - Ability Logger";

        public AbilityOverlayViewModel(ISettingsService settingsService, AbilityViewModel abilityViewModel, IEventAggregator aggregator)
        {
            this.settingsService = settingsService;
            AbilityViewModel = abilityViewModel;
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
            settingsService.Settings.Opacity = Math.Round(settingsService.Settings.Opacity + 0.025, 2);
        }

        public void DecreaseOpacity()
        {
            settingsService.Settings.Opacity = Math.Round(settingsService.Settings.Opacity - 0.025, 2);
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