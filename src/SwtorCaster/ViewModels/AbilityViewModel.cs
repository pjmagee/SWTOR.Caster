namespace SwtorCaster.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using System.Windows;
    using System.Windows.Media;
    using MahApps.Metro.Controls;
    using Caliburn.Micro;
    using Core.Domain;
    using Core.Domain.Settings;
    using Core.Services.Settings;
    using Core.Extensions;
    
    public class AbilityViewModel : Screen, IHandle<ParserMessage>, IHandle<CombatLogViewModel>, IHandle<Settings>
    {
        private readonly ISettingsService _settingsService;

        public SolidColorBrush BackgroundColor
        {
            get
            {
                var control = GetView() as UserControl;
                var window = control.TryFindParent<Window>();

                // ObsView Transpareny is False
                // OverlayView Transparency is True
                if (window.AllowsTransparency)
                {
                    return new SolidColorBrush(Colors.Transparent);
                }

                return new SolidColorBrush(_settingsService.Settings.AbilityLoggerBackgroundColor.FromHexToColor());
            }
        }

        public ObservableCollection<CombatLogViewModel> LogLines { get; } = new ObservableCollection<CombatLogViewModel>();

        public AbilityViewModel(ISettingsService settingsService, IEventAggregator eventAggregator)
        {
            _settingsService = settingsService;
            eventAggregator.Subscribe(this);
        }

        private void TryAddItem(CombatLogViewModel item)
        {
            if (LogLines.Count > _settingsService.Settings.Items) LogLines.Clear();
            if (LogLines.Count == _settingsService.Settings.Items) LogLines.RemoveAt(LogLines.Count - 1);
            LogLines.Insert(0, item);
        }

        public void CopyToClipBoard(CombatLogViewModel viewModel)
        {
            Clipboard.SetText(viewModel.CombatLogEvent.Ability.EntityId.ToString(), TextDataFormat.Text);
        }

        public void Handle(ParserMessage message)
        {
            if (message.ClearLog)
            {
                LogLines.Clear();
            }
        }

        public void Handle(CombatLogViewModel message)
        {
            if (message == null) return;

            var settings = _settingsService.Settings;

            if (settings.EnableCombatClear && message.CombatLogEvent.IsExitCombat()) LogLines.Clear();
            if (!settings.EnableCompanionAbilities && message.CombatLogEvent.IsPlayerCompanion()) return;
            if (settings.IgnoreUnknownAbilities && (message.CombatLogEvent.IsUnknown() || message.ImageUrl == null || message.ImageUrl.Contains("missing.png"))) return;
         
            if (message.CombatLogEvent.IsAbilityActivate())
            {
                TryAddItem(message);
            }
        }

        public void Handle(Settings message)
        {
            Refresh();
        }
    }
}