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

        /// <summary>
        /// e.g Force Charge, Sprint, Fury, Bloodthirst, Frenzy, Beserk, Gore
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// The image inside the /Images/ directory to use
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The rotate angle in degrees to apply
        /// </summary>
        public int Angle { get; set; }

        public Visibility ActionVisibility { get; set; }

        public LogLineEventArgs(string id, SourceTargetType source, SourceTargetType target, EventType eventType, EventDetailType eventDetail, string action, string imageUrl, int angle, Visibility actionVisibility)
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
        }

        public LogLineEventArgs()
        {
        }
    }
}