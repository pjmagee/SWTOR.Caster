namespace SwtorCaster.Core.Domain
{
    using System.Windows;
    using System.Windows.Media;

    public class LogLine
    {
        public string Id { get; set; }

        public string Source { get; set; }

        public SourceTargetType SourceType { get; set; }

        public string Target { get; set; }

        public SourceTargetType TargetType { get; set; }

        public EventType EventType { get; set; }

        public EventDetailType EventDetailType { get; set; }
        
        public string Action { get; set; }
        
        public string ImageUrl { get; set; }
        
        public int Angle { get; set; }
        
        public Visibility ActionVisibility { get; set; }

        public Brush ImageBorderColor { get; set; }

        public bool IsUnknown { get; set; }

        public bool IsCurrentPlayer { get; set; }

        public bool IsEmpty => string.IsNullOrEmpty(Action);

        public LogLine(
            string id, 
            SourceTargetType sourceType, 
            SourceTargetType targetType, 
            EventType eventType, 
            EventDetailType eventDetail, 
            string action, 
            string imageUrl, 
            int angle, 
            Visibility actionVisibility,
            Color borderColor,
            bool unknown, bool isCurrentPlayer)
        {
            Id = id;
            SourceType = sourceType;
            TargetType = targetType;
            EventType = eventType;
            EventDetailType = eventDetail;
            Action = action;
            ImageUrl = imageUrl;
            Angle = angle;
            ActionVisibility = actionVisibility;
            ImageBorderColor = new SolidColorBrush(borderColor);
            IsUnknown = unknown;
            IsCurrentPlayer = isCurrentPlayer;
        }

        public LogLine()
        {
        }
    }
}