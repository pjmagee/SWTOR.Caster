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
    
    public class AbilityListViewModel : Screen, IHandle<ParserMessage>, IHandle<CombatLogViewModel>, IHandle<Settings>
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

        public AbilityListViewModel(ISettingsService settingsService, IEventAggregator eventAggregator)
        {
            _settingsService = settingsService;
            eventAggregator.Subscribe(this);
        }

        private void TryAddItem(CombatLogViewModel item)
        {
            if (LogLines.Count > _settingsService.Settings.Items) LogLines.Clear();
            if (LogLines.Count == _settingsService.Settings.Items) LogLines.RemoveAt(LogLines.Count - 1);

            if (_settingsService.Settings.EnableShowCriticalHits && item.CombatLogEvent.IsApplyEffect() && item.IsCrit)
            {
                if (!IsCriticalHitHandled(item)) 
                {
                    LogLines.Insert(0, item);
                }
            }
            else
            {
                LogLines.Insert(0, item);
            }
        }

        private bool IsCriticalHitHandled(CombatLogViewModel newItem)
        {
            for (int index = 0; index < LogLines.Count; index++)
            {
                var existingItem = LogLines[index];

                if (existingItem.CombatLogEvent.Ability.EntityId == newItem.CombatLogEvent.Ability.EntityId)
                {
                    if (existingItem.CombatLogEvent.IsAbilityActivate())
                    {
                        newItem.ImageAngle = existingItem.ImageAngle;
                        LogLines[index] = newItem;
                        return true;
                    }

                    return false; 
                }
            }

            return false;
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
            if (settings.IgnoreUnknownAbilities && message.CombatLogEvent.IsUnknown()) return;
         
            if (message.CombatLogEvent.IsAbilityActivate())
            {
                TryAddItem(message);
            }
            else if (message.CombatLogEvent.IsApplyEffect() && settings.EnableShowCriticalHits && 
                message.CombatLogEvent.IsCrit && message.CombatLogEvent.IsThisPlayer())
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