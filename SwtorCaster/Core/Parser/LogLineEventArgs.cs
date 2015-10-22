using System.Windows.Media;
using SwtorCaster.Core.Domain;

namespace SwtorCaster.Core.Parser
{
    using System.Windows;
    using Caliburn.Micro;

    public class LogLineEventArgs : PropertyChangedBase
    {
        public string Id { get; set; }

        public SourceTargetType Source { get; set; }

        public SourceTargetType Target { get; set; }

        public EventType EventType { get; set; }

        public EventDetailType EventDetailType { get; set; }
        
        public string Action { get; set; }
        
        public string ImageUrl { get; set; }
        
        public int Angle { get; set; }
        
        public Visibility ActionVisibility { get; set; }

        public Brush ImageBorderColor { get; set; }

        public LogLineEventArgs(
            string id, 
            SourceTargetType source, 
            SourceTargetType target, 
            EventType eventType, 
            EventDetailType eventDetail, 
            string action, 
            string imageUrl, 
            int angle, 
            Visibility actionVisibility,
            Color borderColor)
        {
            Id = id;
            Source = source;
            Target = target;
            EventType = eventType;
            EventDetailType = eventDetail;
            Action = action;
            ImageUrl = imageUrl;
            Angle = angle;
            ActionVisibility = actionVisibility;
            ImageBorderColor = new SolidColorBrush(borderColor);
        }

        public LogLineEventArgs()
        {
        }
    }
}