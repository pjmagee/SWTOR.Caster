namespace SwtorCaster.ViewModels
{
    using System.Windows;
    using System.Windows.Media;
    using Caliburn.Micro;
    using Core.Domain.Log;

    public class CombatLogViewModel : PropertyChangedBase
    {
        private string _text;

        public CombatLogEvent CombatLogEvent { get; }

        public CombatLogViewModel(CombatLogEvent combatLogEvent)
        {
            CombatLogEvent = combatLogEvent;
        }

        #region Image

        public string ImageUrl { get; set; }

        public Color ImageBorderColor { get; set; }

        public int ImageAngle { get; set; }

        #endregion

        #region Text

        public string Text
        {
            get { return _text.ToLower(); }
            set { _text = value; }
        }

        public Visibility TextVisibility { get; set; }

        public int FontSize { get; set; }

        public FontFamily FontFamily { get; set; }

        public SolidColorBrush FontColor { get; set; }

        public string TooltipText { get; set; }

        #endregion
    }
}