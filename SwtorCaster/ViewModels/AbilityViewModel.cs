namespace SwtorCaster.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media;
    using Caliburn.Micro;
    using Core.Domain;
    using Core.Domain.Settings;
    using Screens;
    using Core.Services.Combat;
    using Core.Services.Logging;
    using Core.Services.Providers;
    using Core.Services.Settings;
    using Core.Extensions;

    public class AbilityViewModel : FocusableScreen, IHandle<Settings>, IHandle<ParserMessage>, IHandle<CombatLogViewModel>, IHandle<ICombatLogService>
    {
        private readonly ICombatLogProvider _combatLogProvider;
        private readonly ILoggerService _loggerService;
        private readonly ISettingsService _settingsService;

        public override string DisplayName { get; set; } = "SWTOR Caster - Abilities";

        public SolidColorBrush BackgroundColor => new SolidColorBrush(_settingsService.Settings.AbilityLoggerBackgroundColor.FromHexToColor());

        public ObservableCollection<CombatLogViewModel> LogLines { get; } = new ObservableCollection<CombatLogViewModel>();

        public AbilityViewModel(
            ICombatLogProvider combatLogProvider,
            ILoggerService loggerService,
            ISettingsService settingsService,
            IEventAggregator eventAggregator)
        {
            _combatLogProvider = combatLogProvider;
            _loggerService = loggerService;
            _settingsService = settingsService;
            eventAggregator.Subscribe(this);
        }

        private void TryAddItem(CombatLogViewModel item)
        {
            if (LogLines.Count > _settingsService.Settings.Items) LogLines.Clear();
            if (LogLines.Count == _settingsService.Settings.Items) LogLines.RemoveAt(LogLines.Count - 1);

            if (_settingsService.Settings.EnableShowCriticalHits && item.CombatLogEvent.IsApplyEffect() && item.IsCrit)
            {
                // Replacement handled
                if (!IsCriticalHitHandled(item)) 
                {
                    // Replacement failed, insert new item
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
                
                // Try to replace AbilityActivate with Applied Effect Critical hit
                if (existingItem.CombatLogEvent.Ability.EntityId == newItem.CombatLogEvent.Ability.EntityId)
                {
                    if (existingItem.CombatLogEvent.IsAbilityActivate())
                    {
                        newItem.ImageAngle = existingItem.ImageAngle;
                        LogLines[index] = newItem;
                        return true;
                    }

                    // then an affect has happend after a previous ability activate, we dont want to replace an OLD one which never did crit
                    return false; 
                }
            }

            return false;
        }


        public void CopyToClipBoard(CombatLogViewModel viewModel)
        {
            Clipboard.SetText(viewModel.CombatLogEvent.Ability.EntityId.ToString(), TextDataFormat.Text);
        }

        protected override void OnActivate()
        {
            var parser = _combatLogProvider.GetCombatLogService();
            parser.Start();
        }

        protected override void OnDeactivate(bool close)
        {
            var parser = _combatLogProvider.GetCombatLogService();
            parser.Stop();

            _loggerService.Log($"Parser service stopped.");
        }

        public void Handle(Settings message)
        {
            Refresh();
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
            
            // ActivateAbility can only happen from this player. (We dont see other players ability activate in our own log)
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

        public void Handle(ICombatLogService combatLogService)
        {
            combatLogService.Start();
        }
    }
}