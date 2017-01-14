namespace SwtorCaster.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using System.Windows;
    using System.Windows.Media;
    using MahApps.Metro.Controls;
    using Caliburn.Micro;
    using Core;
    using Core.Domain;
    using Core.Domain.Settings;
    using Core.Services.Settings;

    public class AbilityViewModel : Screen, IHandle<ParserMessage>, IHandle<CombatLogViewModel>, IHandle<AppSettings>
    {
        private readonly ISettingsService settingsService;

        private Layout layout => settingsService.Settings.Layout;

        public HorizontalAlignment HorizontalAlignment
        {
            get
            {                
                if (layout == Layout.LeftToRight) return HorizontalAlignment.Left;
                if (layout == Layout.RightToLeft) return HorizontalAlignment.Right;
                return HorizontalAlignment.Center;
            }
        }

        public int ImageColumn
        {
            get
            {
                if (layout == Layout.LeftToRight) return 0;
                if (layout == Layout.RightToLeft) return 1;
                return 1;
            }
        }

        public int TextColumn
        {
            get
            {
                if (layout == Layout.LeftToRight) return 1;
                if (layout == Layout.RightToLeft) return 0;
                return 0;    
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                var control = GetView() as UserControl;
                var window = control.TryFindParent<Window>();

                if (window.AllowsTransparency)
                {
                    return new SolidColorBrush(Colors.Transparent);
                }

                return new SolidColorBrush(settingsService.Settings.AbilityLoggerBackgroundColor.FromHexToColor());
            }
        }

        public ObservableCollection<CombatLogViewModel> LogLines { get; } = new ObservableCollection<CombatLogViewModel>();

        public AbilityViewModel(ISettingsService settingsService, IEventAggregator eventAggregator)
        {
            this.settingsService = settingsService;
            eventAggregator.Subscribe(this);
        }

        private void TryAddItem(CombatLogViewModel item)
        {
            if (LogLines.Count > settingsService.Settings.Items) LogLines.Clear();
            if (LogLines.Count == settingsService.Settings.Items) LogLines.RemoveAt(LogLines.Count - 1);
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
            if (message.IsUnknown) return;

            var settings = settingsService.Settings;
            if (settings.EnableCombatClear && message.CombatLogEvent.IsExitCombat()) LogLines.Clear();
            if (!settings.EnableCompanionAbilities && message.CombatLogEvent.IsPlayerCompanion()) return;
            
         
            if (message.CombatLogEvent.IsAbilityActivate())
            {
                TryAddItem(message);
            }
        }

        public void Handle(AppSettings message)
        {
            Refresh();
        }
    }
}